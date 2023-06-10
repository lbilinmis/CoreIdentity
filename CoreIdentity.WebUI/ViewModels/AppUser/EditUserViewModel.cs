using System.ComponentModel.DataAnnotations;

namespace CoreIdentity.WebUI.ViewModels.AppUser
{
    public class EditUserViewModel
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
        public string? PhoneNumber { get; set; }

        [Display(Name = "Doğum Tarihi")]
        public DateTime? BirthDate { get; set; }


        [Display(Name = "Şehir")]
        public string? City { get; set; }

        [Display(Name = "Resim")]
        public IFormFile? Picture { get; set; }


        [Display(Name = "Cinsiyet")]
        public Gender? Gender { get; set; }

    }
}
