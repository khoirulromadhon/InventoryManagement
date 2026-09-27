namespace InventoryManagement.ViewModels
{
    public class VmSupplier
    {
        public int SupplierId { get; set; }

        public string SupplierName { get; set; } = null!;

        public bool? IsDelete { get; set; }
    }
}
