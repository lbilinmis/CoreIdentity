using System.ComponentModel.DataAnnotations;

namespace CoreIdentity.WebUI.ViewModels.AppUser
{
    public class ResetPasswordViewModel
    {
        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre boş geçilemez")]
        public string Password { get; set; }


        [Display(Name = "Şifre Tekrar")]
        [Required(ErrorMessage = "Şifre boş geçilemez")]
        public string PasswordConfirm { get; set; }

    }
}
