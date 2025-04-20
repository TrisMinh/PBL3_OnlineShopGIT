using System;
using System.Collections.Generic;

namespace PBL3_OnlineShop.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int CategoryId { get; set; }

    public int SupplierId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal SellingPrice { get; set; }

    public int StockQuantity { get; set; }

    public string? ImageUrl { get; set; }

    public string? Size { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<GoodsReceiptDetail> GoodsReceiptDetails { get; set; } = new List<GoodsReceiptDetail>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Supplier Supplier { get; set; } = null!;
}
