using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PBL3_OnlineShop.Models
{
    public class CouponUsage
    {
        [Key]
        public int CouponUsageId { get; set; }
        public int CouponId { get; set; }
        [ForeignKey("CouponId")]
        public Coupon Coupon { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }

}
