using System;
using System.Collections.Generic;

namespace PBL3_OnlineShop.Models;

public partial class GoodsReceipt
{
    public int ReceiptId { get; set; }

    public int SupplierId { get; set; }

    public DateTime ReceiptDate { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<GoodsReceiptDetail> GoodsReceiptDetails { get; set; } = new List<GoodsReceiptDetail>();

    public virtual Supplier Supplier { get; set; } = null!;
}
