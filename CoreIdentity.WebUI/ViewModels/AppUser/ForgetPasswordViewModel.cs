using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CoreIdentity.WebUI.ViewModels.AppUser
{
    public class ForgetPasswordViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email boş geçilemez")]
        [EmailAddress(ErrorMessage = "Email doğru formatta girilmelidir")]
        public string Email { get; set; }
    }
}
