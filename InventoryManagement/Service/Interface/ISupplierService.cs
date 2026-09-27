using InventoryManagement.ViewModels;

namespace InventoryManagement.Service.Interface
{
    public interface ISupplierService
    {
        Task InsertSupplier(VmSupplier supplier);
        Task<List<VmSupplier>> GetAllSuppliers(string keyword, int cursor);
        Task<VmSupplier> GetSupplierById(int id);
        Task DeleteSupplier(int id);
    }
}
