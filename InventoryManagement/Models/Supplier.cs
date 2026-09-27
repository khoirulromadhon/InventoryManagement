using System;
using System.Collections.Generic;

namespace InventoryManagement.Models;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string SupplierName { get; set; } = null!;

    public bool? IsDelete { get; set; }

    public virtual ICollection<Good> Goods { get; set; } = new List<Good>();
}
