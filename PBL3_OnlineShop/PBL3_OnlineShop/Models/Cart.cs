using System;
using System.Collections.Generic;

namespace PBL3_OnlineShop.Models;

public partial class Cart
{
    public int CartId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
