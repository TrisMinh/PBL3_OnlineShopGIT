using System;
using System.Collections.Generic;

namespace PBL3_OnlineShop.Models;

public partial class GoodsReceiptDetail
{
    public int ReceiptDetailId { get; set; }

    public int ReceiptId { get; set; }

    public int ProductId { get; set; }

    public int QuantityReceived { get; set; }

    public decimal PurchasePrice { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual GoodsReceipt Receipt { get; set; } = null!;
}
