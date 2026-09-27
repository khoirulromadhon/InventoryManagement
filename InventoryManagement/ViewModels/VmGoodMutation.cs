using InventoryManagement.Models;

namespace InventoryManagement.ViewModels
{
    public class VmGoodMutation
    {
        public int? MutationId { get; set; }

        public int? GoodId { get; set; }

        public string? GoodName { get; set; } 

        public DateTime? MutationDate { get; set; }

        public string? Status { get; set; } 

        public int? Amount { get; set; }

        public virtual Good? Good { get; set; }
    }
}
