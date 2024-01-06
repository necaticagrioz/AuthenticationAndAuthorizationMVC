using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationAndAuthorizationMVC.Models.DTO_s
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur!!")]
        [MinLength(3, ErrorMessage = "En az 3 karakter girmek zorundasınız!!")]
        [DisplayName("Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "E-Mail zorunludur!!")]
        [DataType(DataType.EmailAddress)]
        [DisplayName("E-Mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur!!")]
        [DataType(DataType.Password)]
        [DisplayName("Şifre")]
        public string Password { get; set; }
    }
}
