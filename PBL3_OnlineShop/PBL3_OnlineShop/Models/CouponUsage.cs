﻿using System.ComponentModel.DataAnnotations;

namespace PBL3_OnlineShop.Models
{
    public class CouponUsage
    {
        [Key]
        public int CouponUsageId { get; set; }
        public int CouponId { get; set; }
        public int UserId { get; set; }
    }

}
