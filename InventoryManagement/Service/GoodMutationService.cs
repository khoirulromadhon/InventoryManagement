using InventoryManagement.Models;
using InventoryManagement.Service.Interface;
using InventoryManagement.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Service
{
    public class GoodMutationService : IGoodMutationService
    {
        private readonly AppDbContext _context;

        public GoodMutationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task Mutation(VmGoodMutation vmGoodMutation)
        {
            try
            {
                if (vmGoodMutation.Status.Equals("INBOUND"))
                {
                    Good existingGood = await _context.Goods.FirstOrDefaultAsync(x =>
                        x.GoodId == vmGoodMutation.GoodId &&
                        x.CategoryId == vmGoodMutation.CategoryId &&
                        x.SupplierId == vmGoodMutation.SupplierId
                    );

                    //Good goodName = await _context.Goods.FirstOrDefaultAsync(x => x.GoodId == vmGoodMutation.GoodId);
                    Category category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == vmGoodMutation.CategoryId);
                    Supplier supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.SupplierId == vmGoodMutation.SupplierId);

                    if (category == null)
                    {
                        throw new Exception("Category not found.");
                    }

                    if (supplier == null)
                    {
                        throw new Exception("Supplier not found.");
                    }

                    if (existingGood != null) { 
                        existingGood.GoodStock += vmGoodMutation.Amount;

                        _context.Goods.Update(existingGood);

                        _context.GoodMutations.Add(new GoodMutation
                        {
                            GoodId = vmGoodMutation.GoodId,
                            CategoryId = vmGoodMutation.CategoryId,
                            SupplierId = vmGoodMutation.SupplierId,
                            MutationDate = DateTime.Now,
                            Status = vmGoodMutation.Status,
                            Amount = vmGoodMutation.Amount
                        });
                    }
                    else
                    {
                        string goodCode = "G" + 
                            vmGoodMutation.CategoryId.ToString() + 
                            vmGoodMutation.SupplierId.ToString() + 
                            category.CategoryName.Substring(0, 1);

                        Good good = new Good
                        {
                            CategoryId = vmGoodMutation.CategoryId,
                            SupplierId = vmGoodMutation.SupplierId,
                            GoodCode = goodCode,
                            GoodName = vmGoodMutation.GoodName,
                            GoodStock = vmGoodMutation.Amount
                        };

                        _context.Goods.Add(good);
                        await _context.SaveChangesAsync();

                        _context.GoodMutations.Add(new GoodMutation
                        {
                            GoodId = good.GoodId,
                            CategoryId = vmGoodMutation.CategoryId,
                            SupplierId = vmGoodMutation.SupplierId,
                            MutationDate = DateTime.Now,
                            Status = vmGoodMutation.Status,
                            Amount = vmGoodMutation.Amount
                        });
                    }
                }
                else if (vmGoodMutation.Status.Equals("OUTBOUND"))
                {
                    Good existingGood = await _context.Goods.FirstOrDefaultAsync(
                        x => x.SupplierId == vmGoodMutation.SupplierId && 
                        x.CategoryId == vmGoodMutation.CategoryId &&
                        x.GoodName == vmGoodMutation.GoodName
                    );

                    if (existingGood == null)
                    {
                        throw new Exception("Good not found.");
                    }

                    if (existingGood.GoodStock < vmGoodMutation.Amount)
                    {
                        throw new Exception("Insufficient stock for the outbound mutation.");
                    } 
                    else {
                        existingGood.GoodStock -= vmGoodMutation.Amount;
                        _context.Goods.Update(existingGood);

                        _context.GoodMutations.Add(new GoodMutation
                        {
                            GoodId = existingGood.GoodId,
                            CategoryId = existingGood.CategoryId,
                            SupplierId = existingGood.SupplierId,
                            MutationDate = DateTime.Now,
                            Status = vmGoodMutation.Status,
                            Amount = vmGoodMutation.Amount
                        });
                    }
                }
                else
                {
                    throw new Exception("Invalid mutation status.");
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving categories.", ex);
            }
        }

        public async Task<List<VmGoodMutation>> MutationHistory(string keyword, int cursor)
        {
            try
            {
                var mutations = await _context.Database
                .SqlQuery<VmGoodMutation>($"""
                    EXEC GetMutation
                       @Keyword = {keyword},  
                       @CursorId = {cursor}
                    """)
                .ToListAsync();

                return mutations;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving mutations.", ex);
            }
        }
    }
}
