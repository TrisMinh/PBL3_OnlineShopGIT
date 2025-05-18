using PBL3_OnlineShop.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PBL3_OnlineShop.Models;

public partial class Product
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
    public string? Gender { get; set; }
    [ForeignKey("CategoryId")]
    public Category Category { get; set; }
    public ICollection<ProductSize> ProductSizes { get; set; }
    [NotMapped]
    [FileExtensionAttributes]
    public List<IFormFile> ImageUpload { get; set; }
}
