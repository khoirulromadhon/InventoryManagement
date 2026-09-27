using System;
using System.Collections.Generic;

namespace InventoryManagement.Models;

public partial class Good
{
    public int GoodId { get; set; }

    public int CategoryId { get; set; }

    public int SupplierId { get; set; }

    public string GoodCode { get; set; } = null!;

    public string GoodName { get; set; } = null!;

    public int? GoodStock { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<GoodMutation> GoodMutations { get; set; } = new List<GoodMutation>();

    public virtual Supplier Supplier { get; set; } = null!;
}
