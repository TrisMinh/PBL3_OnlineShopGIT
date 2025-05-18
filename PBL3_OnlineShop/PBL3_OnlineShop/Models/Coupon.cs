using System.ComponentModel.DataAnnotations;

namespace PBL3_OnlineShop.Models
{
    public class Coupon
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "The coupon name field is required.\r\n")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The Description is required.\r\n")]
        public string Description { get; set; }
        [Required(ErrorMessage = "The quantity is required.\r\n")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "The Start Date is required.\r\n")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "The End Date is required.\r\n")]
        public DateTime EndDate { get; set; }
        public int status { get; set; }
        public decimal Discount { get; set; }
        public ICollection<CouponUsage> couponUsages { get; set; }
    }
}
