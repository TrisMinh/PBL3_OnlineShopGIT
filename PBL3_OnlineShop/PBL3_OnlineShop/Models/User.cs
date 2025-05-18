using System.ComponentModel.DataAnnotations;

namespace PBL3_OnlineShop.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required"), EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } = "Customer";
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string UrlAvatar { get; set; }
        public int Status { get; set; } = 1;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Cart Cart { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<CouponUsage> couponUsages { get; set; }
    }
}