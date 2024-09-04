using CRMProject.Models;

namespace CRMProject.DTO;

public partial class CustomerDto
{
 
    public string Name { get; set; } = null!;

    public string? Mobile { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? Need { get; set; }

    public string? Remarks { get; set; }

    public string? Property { get; set; }

    public string? Status { get; set; }

    public int InventoryId { get; set; }

   
}

