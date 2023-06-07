using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CoreIdentity.WebUI.Areas.Admin.Models
{
    public class RoleCreateViewModel
    {
        [Display(Name = "Rol adı")]
        [Required(ErrorMessage = "Role adı boş geçilemez")]
        public string Name { get; set; }
    }
}
