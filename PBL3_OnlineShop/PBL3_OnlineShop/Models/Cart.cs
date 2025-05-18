using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [ForeignKey("User")]
        public int UserId { get; set; }
        public ICollection<CartItem> CartItems { get; set; } 
        public User User { get; set; } 
    }
}
