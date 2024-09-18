using System.ComponentModel.DataAnnotations;

namespace CRMProject.DTO;

public class MyProfileDto
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    [Required]
    [RegularExpression(@"^[a-z0-9][a-z0-9._%+-]+@(gmail\.com)$", ErrorMessage = "Invalid email format.")]
    [StringLength(50)]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Mobile number is required.")]
    [RegularExpression(@"^[6789]\d{9}$", ErrorMessage = "Invalid mobile number format. It should be a 10-digit number starting with 7, 8, or 9.")]
    public string Mobile { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string? ProfileUrl { get; set; }
}
