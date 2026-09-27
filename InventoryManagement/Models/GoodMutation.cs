using System;
using System.Collections.Generic;

namespace InventoryManagement.Models;

public partial class GoodMutation
{
    public int MutationId { get; set; }

    public int GoodId { get; set; }

    public int CategoryId { get; set; }

    public int SupplierId { get; set; }

    public DateTime? MutationDate { get; set; }

    public string Status { get; set; } = null!;

    public int Amount { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Good Good { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;
}
