using System.ComponentModel.DataAnnotations;

namespace Demo.UI.Models
{
    public class SignInUserModel
    {
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Display(Name = "Zapamiętaj mnie")]
        public bool RememberMe { get; set; }
    }
}