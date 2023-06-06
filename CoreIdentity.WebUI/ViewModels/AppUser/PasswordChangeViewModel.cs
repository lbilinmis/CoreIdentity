using System.ComponentModel.DataAnnotations;

namespace CoreIdentity.WebUI.ViewModels.AppUser
{
    public class PasswordChangeViewModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre boş geçilemez")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre")]
        [Required(ErrorMessage = "Yeni Şifre boş geçilemez")]

        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Yeni  Şifre Tekrar")]
        [Compare(nameof(NewPassword), ErrorMessage = "Şifre aynı değil.")]
        [Required(ErrorMessage = "Şifre tekrar boş geçilemez")]
        public string ConfirmPassword { get; set; }
    }
}
