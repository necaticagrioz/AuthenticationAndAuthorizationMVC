

using System.ComponentModel.DataAnnotations;

namespace AuthenticationAndAuthorizationMVC.Models.DTO_s
{
    public class RoleDTO
    {
        [Required(ErrorMessage = "Rol adı zorunludur!!")]
        [MinLength(3, ErrorMessage = "En az 3 karakter girmelisiniz!!")]
        public string Name { get; set; }
    }
}
