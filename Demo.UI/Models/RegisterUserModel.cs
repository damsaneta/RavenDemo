using System.ComponentModel.DataAnnotations;
using Demo.Domain.Users;

namespace Demo.UI.Models
{
    public class RegisterUserModel
    {
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [MaxLength(50, ErrorMessage = "Pole {0} jest za dlugie")]
        [Display(Name = "Login")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [StringLength(20, ErrorMessage = "Pole {0} jest za długie")]
        //[Remote("LoginExist", "Account", ErrorMessageResourceName = "LoginIsUsed", ErrorMessageResourceType = typeof(ValidationMessages))]
        //[RegularExpression(@"^[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ]+$", ErrorMessage = "")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [StringLength(20, ErrorMessage = "Pole {0} jest za długie")]
        //[RegularExpression(@"^[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ]+$", ErrorMessage = "")]
        [Display(Name = "Nazwisko")]
        public string LastName { get;  set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Display(Name = "Powtórz hasło")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Hasła się różnią")]
        public string ConfirmPassword { get; set; }   

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Display(Name = "Rola")]
        public Role Role { get; set; }
    }
}