using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Models;

namespace PBL3_OnlineShop.Repository
{
    public class PBL3_Db_Context : DbContext
    {
        public PBL3_Db_Context(DbContextOptions<PBL3_Db_Context> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductSize> ProductsSize { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
    }
}
