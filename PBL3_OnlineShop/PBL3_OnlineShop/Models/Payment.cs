using System;
using System.Collections.Generic;

namespace PBL3_OnlineShop.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    public DateTime PaymentDate { get; set; }

    public decimal Amount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string PaymentStatus { get; set; } = null!;

    public string? TransactionId { get; set; }

    public string? PaymentGatewayResponse { get; set; }

    public string? Notes { get; set; }

    public virtual Order Order { get; set; } = null!;
}
