using System.ComponentModel.DataAnnotations;

namespace PBL3_OnlineShop.Models.ViewModels
{
    public class ForgotPasswordView
    {
        [Required(ErrorMessage = "Email or username is required.")]
        public string ForgotUsername { get; set; }
    }
}
