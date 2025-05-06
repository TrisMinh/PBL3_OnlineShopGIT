using System.ComponentModel.DataAnnotations;

namespace PBL3_OnlineShop.Models
{
    public class ProductSize
    {
        [Key]
        public int Id { get; set; }
        public string Size { get; set; } // Ví dụ: "S", "M", "L", "XL"
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public Products Product { get; set; }
    }

}
