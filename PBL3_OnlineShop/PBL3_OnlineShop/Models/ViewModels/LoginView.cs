using System.ComponentModel.DataAnnotations;

namespace PBL3_OnlineShop.Models.ViewModels
{
    public class LoginView
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
