namespace CRMProject.DTO;

public class ChangePasswordDto
{
    public string Password { get; set; } = null!;
    public string NewPassword { get; set; }
    public string ConfirmPassword {  get; set; }
}
