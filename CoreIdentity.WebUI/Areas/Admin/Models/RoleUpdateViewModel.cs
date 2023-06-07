using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CoreIdentity.WebUI.Areas.Admin.Models
{
    public class RoleUpdateViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Rol adı")]
        [Required(ErrorMessage = "Role adı boş geçilemez")]
        public string Name { get; set; }
    }
}
