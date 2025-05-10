using System.ComponentModel.DataAnnotations;

namespace PBL3_OnlineShop.Models
{
    public class Cart
    {
        public Cart()
        {
            CartItems = new List<CartItem>();
        }       
        [Key]
        public int CartId { get; set; }
        public int UserId { get; set; }
        public ICollection<CartItem> CartItems { get; set; } 
        public User User { get; set; } 
    }
}
