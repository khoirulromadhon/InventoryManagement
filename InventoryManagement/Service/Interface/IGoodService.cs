using InventoryManagement.ViewModels;

namespace InventoryManagement.Service.Interface
{
    public interface IGoodService
    {
        Task<List<VmGood>> GetAllGoods(string keyword, int cursor);
    }
}
