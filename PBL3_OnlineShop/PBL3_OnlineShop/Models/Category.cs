using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PBL3_OnlineShop.Models;

public partial class Category
{
    [Key]
    public int CategoryId { get; set; }
    [Required,MinLength(4,ErrorMessage = "Please enter CategoryName")]
    public string CategoryName { get; set; }
    public string? Description { get; set; }

    public int Status { get; set; }
}
