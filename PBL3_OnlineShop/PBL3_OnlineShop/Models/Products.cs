using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PBL3_OnlineShop.Models;

public partial class Products
{
    [Key]
    public int ProductId { get; set; }
    public int CategoryId { get; set; }
    [Required, MinLength(4, ErrorMessage = "Please enter ProductName")]
    public string ProductName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal SellingPrice { get; set; }

    public int StockQuantity { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string Status { get; set; } = null!;

    public decimal? SalePercentage { get; set; }

    public string? Collections { get; set; }

    public string? Colors { get; set; }

    public string? Gender { get; set; }

    public Category Category { get; set; }
    public ICollection<ProductSize> ProductSizes { get; set; }
}
