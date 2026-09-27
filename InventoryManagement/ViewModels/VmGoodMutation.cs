using InventoryManagement.Models;

namespace InventoryManagement.ViewModels
{
    public class VmGoodMutation
    {
        public int? MutationId { get; set; }

        public int GoodId { get; set; }

        public string? GoodName { get; set; }

        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public int SupplierId { get; set; }

        public string? SupplierName { get; set; }

        public DateTime MutationDate { get; set; }

        public string Status { get; set; } = null!;

        public int Amount { get; set; }
    }
}
