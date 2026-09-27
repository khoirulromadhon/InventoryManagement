using InventoryManagement.Models;
using InventoryManagement.Service.Interface;
using InventoryManagement.ViewModels;
using Microsoft.EntityFrameworkCore;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace InventoryManagement.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<VmCategory>> GetAllCategories(string keyword, int cursor)
        {
            try
            {
                var categories = await _context.Database
                .SqlQuery<VmCategory>($"""
                    EXEC GetCategory
                       @Keyword = {keyword},  
                       @CursorId = {cursor}
                    """)
                .ToListAsync();

                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving categories.", ex);
            }
        }

        public async Task<VmCategory> GetCategoryById(int id)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);

                if (category == null)
                {
                    throw new Exception($"Category with ID {id} not found.");
                }

                VmCategory vmCategory = new VmCategory
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName
                };

                return vmCategory;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving categories.", ex);
            }
        }

        public async Task InsertCategory(VmCategory vmcategory)
        {
            try
            {
                if (vmcategory.CategoryId == null || vmcategory.CategoryId == 0)
                {
                    Category category = new Category
                    {
                        CategoryName = vmcategory.CategoryName
                    };

                    _context.Categories.Add(category);
                }
                else
                {
                    Category existingCategory = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == vmcategory.CategoryId);

                    if (existingCategory == null)
                    {
                        throw new InvalidOperationException("Category not found");
                    }

                    existingCategory.CategoryName = vmcategory.CategoryName;
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving categories.", ex);
            }
        }

        public async Task DeleteCategory(int id)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);

                if (category == null)
                {
                    throw new Exception($"Category with ID {id} not found.");
                }

                category.IsDelete = true;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the category.", ex);
            }
        }
    }
}
