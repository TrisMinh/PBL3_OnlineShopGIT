using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Models;

namespace PBL3_OnlineShop.Data
{
    public class SeedData
    {
        public static void SeedingData(PBL3_Db_Context _context)
        {
            _context.Database.Migrate();

            if (!_context.Users.Any())
            {
                var passwordHasher = new PasswordHasher<User>();
                var adminUser = new User
                {
                    UserName = "Admin",
                    Gender = "Man",
                    Email = "admin@gmail.com",
                    UrlAvatar = "/avatar/def.jpg",
                    Role = "Admin",
                    Status = 1,
                    CreatedAt = DateTime.Now,
                };

                adminUser.Password = passwordHasher.HashPassword(adminUser, "123");

                _context.Users.Add(adminUser);
            }

            if (!_context.Categories.Any())
            {
                _context.Categories.AddRange(
                    new Category { CategoryName = "All", Status = 1},
                    new Category { CategoryName = "Jackets", Status = 1},
                    new Category { CategoryName = "T-Shirts", Status = 1},
                    new Category { CategoryName = "Polo Shirts", Status = 1},
                    new Category { CategoryName = "Suits", Status = 1},
                    new Category { CategoryName = "Jeans", Status = 1},
                    new Category { CategoryName = "Trousers", Status = 1},
                    new Category { CategoryName = "Shorts", Status = 1},
                    new Category { CategoryName = "Sports", Status = 1},
                    new Category { CategoryName = "Sportswear", Status = 1},
                    new Category { CategoryName = "Dresses", Status = 1},
                    new Category { CategoryName = "Skirts", Status = 1},
                    new Category { CategoryName = "Coats", Status = 1 }
                );
            }

            if (!_context.Products.Any())
            {
                var product1 = new Product
                {
                    CategoryId = 3,
                    ProductName = "Áo thun cộc tay",
                    Description = "Áo cổ tròn cơ bản, in graphic trước ngực năng động, trẻ trung",
                    SellingPrice = 100,
                    StockQuantity = 80,
                    ImageUrl = "~/imagesProducts/Aothuncoctay1.png,~/imagesProducts/Aothuncoctay2.png,~/imagesProducts/Aothuncoctay3.png,~/imagesProducts/Aothuncoctay4.png",
                    CreatedAt = DateTime.Now,
                    Status = "1,3",
                    SalePercentage = 0,
                    Collections = "Summer",
                    Gender = "male",
                    ProductSizes = new List<ProductSize>
                    {
                        new ProductSize { Size = "S", Color = "Blue", Quantity = 20 },
                        new ProductSize { Size = "M", Color = "Blue", Quantity = 30 },
                        new ProductSize { Size = "M", Color = "Yellow", Quantity = 30 }
                    }
                };

                var product2 = new Product
                {
                    CategoryId = 6,
                    ProductName = "Quần sooc jeans nữ ",
                    Description = "Quần nữ suông vừa trẻ trung. Thiết kế túi chéo sườn, túi ốp sau, gấu xắn tua bản to trẻ trung. ",
                    SellingPrice = 100,
                    StockQuantity = 40,
                    ImageUrl = "~/imagesProducts/Quansoocjeans1.png, ~/imagesProducts/Quansoocjeans2.png",
                    CreatedAt = DateTime.Now,
                    Status = "1,3,2",
                    SalePercentage = 0.2m,
                    Collections = "Summer",
                    Gender = "Female",
                    ProductSizes = new List<ProductSize>
                    {
                        new ProductSize { Size = "S", Color = "Blue", Quantity = 15 },
                        new ProductSize { Size = "M", Color = "Blue", Quantity = 25 }
                    }
                };

                var product3 = new Product
                {
                    CategoryId = 11,
                    ProductName = "Áo Sơmi Cotton",
                    Description = "Sử dụng vải Cotton có thành phần siêu thấm hút, mềm mại, dẻo dai, độ bền cao Khả năng thấm hút của vải tạo cảm giác khô ráo, thoáng mát",
                    SellingPrice = 120,
                    StockQuantity = 40,
                    ImageUrl = "~/imagesProducts/Aosomi.png, ~/imagesProducts/Aosomi1.png",
                    CreatedAt = DateTime.Now,
                    Status = "1,3",
                    SalePercentage = 0,
                    Collections = "Spring",
                    Gender = "Male",
                    ProductSizes = new List<ProductSize>
                    {
                        new ProductSize { Size = "S", Color = "Yellow", Quantity = 15 },
                        new ProductSize { Size = "M", Color = "Yellow", Quantity = 25 }
                    }
                };

                _context.Products.AddRange(product1, product2, product3);
            }

            if (!_context.Coupons.Any())
            {
                _context.Coupons.Add(new Coupon { Name = "VOUCHER5", Description = "New opening discount of $5 for orders", Quantity = 100, Discount = 2, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(7), status = 1 });
            }

            _context.SaveChanges();
        }
    }
}
