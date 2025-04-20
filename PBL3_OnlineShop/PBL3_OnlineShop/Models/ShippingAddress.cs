using System;
using System.Collections.Generic;

namespace PBL3_OnlineShop.Models;

public partial class ShippingAddress
{
    public int AddressId { get; set; }

    public int UserId { get; set; }

    public string RecipientName { get; set; } = null!;

    public string AddressLine { get; set; } = null!;

    public string City { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public bool IsDefault { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User User { get; set; } = null!;
}
