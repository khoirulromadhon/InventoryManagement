using System;
using System.Collections.Generic;

namespace InventoryManagement.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public bool? IsDelete { get; set; }

    public virtual ICollection<GoodMutation> GoodMutations { get; set; } = new List<GoodMutation>();

    public virtual ICollection<Good> Goods { get; set; } = new List<Good>();
}
