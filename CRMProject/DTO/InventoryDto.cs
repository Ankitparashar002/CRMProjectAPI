using CRMProject.Models;

namespace CRMProject.DTO;


    public partial class InventoryDto
    {
   

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

    public int CustomerId { get; set; }

    
}


