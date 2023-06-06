using System.ComponentModel.DataAnnotations;

namespace CoreIdentity.WebUI.ViewModels.AppUser
{
    public class SignUpViewModel
    {
        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Kullanıcı adı boş geçilemez")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email boş geçilemez")]
        [EmailAddress(ErrorMessage = "Email doğru formatta girilmelidir")]
        public string Email { get; set; }

        [Display(Name = "Telefon No")]
        [Required(ErrorMessage = "Telefon No boş geçilemez")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre boş geçilemez")]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Şifre aynı olmalıdır.")]
        [Display(Name = "Şifre Tekrar")]
        [Required(ErrorMessage = "Şifre Tekrar boş geçilemez")]
        public string PasswordConfirm { get; set; }
    }
}
