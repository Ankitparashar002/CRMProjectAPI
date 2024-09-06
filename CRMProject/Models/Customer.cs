using System;
using System.Collections.Generic;

namespace CRMProject.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public long Mobile { get; set; }

    public string Email { get; set; } = null!;

    public string? Address { get; set; }

    public string? Need { get; set; }

    public string? Remarks { get; set; }

    public string? Property { get; set; }

    public string? Status { get; set; }

    public bool InventoryStatus { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}
