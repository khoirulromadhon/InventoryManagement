using System;
using System.Collections.Generic;

namespace InventoryManagement.Models;

public partial class GoodMutation
{
    public int MutationId { get; set; }

    public int GoodId { get; set; }

    public DateTime? MutationDate { get; set; }

    public string Status { get; set; } = null!;

    public int Amount { get; set; }

    public virtual Good Good { get; set; } = null!;
}
