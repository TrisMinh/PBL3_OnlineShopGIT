using System;
using System.Collections.Generic;

namespace PBL3_OnlineShop.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public string ShippingAddress { get; set; } = null!;

    public int? CouponId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = null!;

    public virtual Coupon? Coupon { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
