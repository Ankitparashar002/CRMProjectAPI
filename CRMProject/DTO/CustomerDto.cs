using CRMProject.Models;
using System.ComponentModel.DataAnnotations;

namespace CRMProject.DTO;

public partial class CustomerDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Mobile number is required.")]
    [RegularExpression(@"^[6789]\d{9}$", ErrorMessage = "Invalid mobile number format. It should be a 10-digit number starting with 7, 8, or 9.")]
    public long Mobile { get; set; }

    [Required]
    [RegularExpression(@"^[a-z0-9][a-z0-9._%+-]+@(gmail\.com)$", ErrorMessage = "Invalid email format.")]
    [StringLength(50)]
    public string Email { get; set; } = null!;

    public string? Address { get; set; }

    public string? Need { get; set; }

    public string? Remarks { get; set; }

    public string? Property { get; set; }

    public string? Status { get; set; }

    public bool InventoryStatus { get; set; }



}

