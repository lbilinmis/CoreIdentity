using System.ComponentModel.DataAnnotations;

namespace CoreIdentity.WebUI.ViewModels.AppUser
{
    public class SignInViewModel
    {
        //[Display(Name = "Kullanıcı Adı")]
        //[Required(ErrorMessage = "Kullanıcı adı boş geçilemez")]
        //public string UserName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email boş geçilemez")]
        [EmailAddress(ErrorMessage = "Email doğru formatta girilmelidir")]
        public string Email { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre boş geçilemez")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

    }
}
