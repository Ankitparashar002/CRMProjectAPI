using System;
using System.Collections.Generic;

namespace CRMProject.Models;

public partial class Inventory
{
    public int Id { get; set; }

    public string? PropertyType { get; set; }

    public string? PropertyStatus { get; set; }

    public string? Address { get; set; }

    public string? Location { get; set; }

    public string? Floor { get; set; }

    public string? Bed { get; set; }

    public decimal? Rent { get; set; }

    public decimal? PlotSize { get; set; }

    public bool? ParkFacing { get; set; }

    public bool? Lift { get; set; }

    public bool? StiltParking { get; set; }

    public bool? StaffRoom { get; set; }

    public string? Remarks { get; set; }

    public virtual ICollection<Lead> Leads { get; set; } = new List<Lead>();
}
