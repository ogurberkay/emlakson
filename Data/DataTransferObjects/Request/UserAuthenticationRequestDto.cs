using System.ComponentModel.DataAnnotations;

namespace Data.DataTransferObjects.Request;

public class UserAuthenticationRequestDto
{
    [Required(ErrorMessage = "Email is required.")]
    public string? Email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}