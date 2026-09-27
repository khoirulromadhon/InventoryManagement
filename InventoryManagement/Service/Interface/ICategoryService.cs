using InventoryManagement.ViewModels;

namespace InventoryManagement.Service.Interface
{
    public interface ICategoryService
    {
        Task InsertCategory(VmCategory category);
        Task<List<VmCategory>> GetAllCategories(string keyword, int cursor);
        Task<VmCategory> GetCategoryById(int id);
        Task DeleteCategory(int id);
    }
}
