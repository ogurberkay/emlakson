using System.ComponentModel.DataAnnotations;

namespace Data.DataTransferObjects.Request;

public class UserRegistrationRequestDto
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }
        
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string? Email { get; set; }
        
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
        
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string? ConfirmPassword { get; set; }

    public string RoleName { get; set; } = "User";
    //TODO nullable düzenle
    //TODO USER ALL endpointi ekle
    //TODO kullanıcı crud yetkisi superadmine verilecek
    //TODO responseExpception exception silinecek
    //TODO advertdescription => Description olacak
    //TODO ilanı sadece yükleyen kişinin güncelleyip silebilm3si
    //TODO Şifre yenileme mekanizması
    //TODO Şifremi unuttum
    
    
}