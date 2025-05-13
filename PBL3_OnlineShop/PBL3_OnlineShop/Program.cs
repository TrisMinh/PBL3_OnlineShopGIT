using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PBL3_OnlineShop.Data;
using PBL3_OnlineShop.Services.Account;
using PBL3_OnlineShop.Services.Admin.Category;
using PBL3_OnlineShop.Services.Admin.Coupon;
using PBL3_OnlineShop.Services.Admin.Order;
using PBL3_OnlineShop.Services.Admin.Product;
using PBL3_OnlineShop.Services.Admin.Statistic;
using PBL3_OnlineShop.Services.Admin.User;
using PBL3_OnlineShop.Services.Cart;
using PBL3_OnlineShop.Services.Checkout;
using PBL3_OnlineShop.Services.Product;
using PBL3_OnlineShop.Services.Order;

namespace PBL3_OnlineShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<PBL3_Db_Context>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Đk dịch vụ cho các controller 
            builder.Services.AddScoped<IStatisticService, StatisticService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<Services.Admin.Product.IProductService, Services.Admin.Product.ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICouponService, CouponService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<ICheckoutService, CheckoutService>();
            builder.Services.AddScoped<IOrderCusService, OrderCusService>();
            // Đăng ký dịch vụ ProductService cho phía người dùng
            builder.Services.AddScoped<Services.Product.IProductService, Services.Product.ProductService>();
            builder.Services.AddDistributedMemoryCache(); // Bộ nhớ để lưu session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // thời gian sống của session
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            app.UseSession();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseAuthorization();

            app.MapControllerRoute(
               name: "areas",
               pattern: "{area:exists}/{controller=Products}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}