using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.Models.ViewModels;
using PBL3_OnlineShop.Data;
using PBL3_OnlineShop.SessionHelper;
using System.Drawing;

namespace PBL3_OnlineShop.Controllers
{
    public class CartController : Controller
    {
        private readonly PBL3_Db_Context _context;
        public CartController(PBL3_Db_Context context)
        {
            _context = context;
        }
        // GET: CartController
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("_UserId");
            if (userId == null) // chưa đăng nhập 
            {
                List<CartItem> cartItems = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
                CartItemView cartView = new CartItemView
                {
                    CartItems = cartItems,
                    TotalPrice = cartItems.Sum(item => item.SellingPrice * item.Quantity)
                };
                return View(cartView);
            }
            // nếu đăng nhập rồi thì lấy giỏ hàng từ db
            var cart = await _context.Carts.Include(c => c.CartItems)
                                    .ThenInclude(ci => ci.Product)
                                    .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                return View(new CartItemView
                {
                    CartItems = new List<CartItem>(),
                    TotalPrice = 0
                });
            }

            var cartViewDb = new CartItemView
            {
                CartItems = cart.CartItems.ToList(),
                TotalPrice = cart.CartItems.Sum(item => item.Quantity * item.SellingPrice)
            };

            return View(cartViewDb);

        }
        [HttpPost]
        public IActionResult Checkout()
        {
            var userId = HttpContext.Session.GetInt32("_UserId");

            if (userId == null)
            {
                TempData["Error"] = "Please login to checkout.";
                return RedirectToAction("Login", "Account");
            }

            var cart = _context.Carts.Include(c => c.CartItems).FirstOrDefault(c => c.UserId == userId);

            if (cart.CartItems == null || !cart.CartItems.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            foreach (var item in cart.CartItems)
            {
                var product = _context.ProductsSize.FirstOrDefault(ps => ps.ProductId == item.ProductId && ps.Color == item.Color && ps.Size == item.Size);

                if (product.Quantity < item.Quantity)
                {
                    TempData["Error"] = "Not enough stock for " + item.ProductName + " Color: " +item.Color + " Size: "+item.Size;
                    return RedirectToAction("Index");
                }
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user.UserName == null || user.PhoneNumber == null || user.Address == null)
            {
                TempData["Error"] = "Please enter complete contact information";
                return RedirectToAction("Profile", "Account");
            }

            return RedirectToAction("Index", "Checkout");
        }
        public async Task<IActionResult> Add(int id, string size, string color)
        {
            // Kiểm tra
            var productSize = await _context.ProductsSize.FirstOrDefaultAsync(ps => ps.ProductId == id && ps.Size == size && ps.Color == color);
            if(productSize == null)
            {
                TempData["Error"] = "Product not found or size/color not available.";
                return Redirect(Request.Headers["Referer"].ToString()); // trả về trang hiện tại
            }

            var userId = HttpContext.Session.GetInt32("_UserId");
            Product product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            if (userId == null)
            {
                List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
                CartItem cartItem = cart.FirstOrDefault(c => c.ProductId == id && c.Size == size && c.Color == color);
                if (cartItem == null)
                {
                    cartItem = new CartItem(product)
                    {
                        Color = color,
                        Size = size,
                        Quantity = 1
                    };
                    cart.Add(cartItem);
                }
                else
                {
                    cartItem.Quantity++;
                }
                HttpContext.Session.SetJson("Cart", cart);
            }
            else
            {
                // --- Đã đăng nhập: lưu DB ---
                var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
                if (cart == null)
                {
                    cart = new Cart
                    {
                        UserId = userId.Value,
                        CartItems = new List<CartItem>()
                    };
                    _context.Carts.Add(cart);
                }
                var cartItem = cart.CartItems.FirstOrDefault(c => c.ProductId == id && c.Size == size && c.Color == color);
                if (cartItem == null)
                {
                    cartItem = new CartItem
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        Size = size,
                        Color = color,
                        Quantity = 1,
                        SellingPrice = product.SellingPrice,
                        ImageUrl = product.ImageUrl
                    };
                    cart.CartItems.Add(cartItem);
                }
                else
                {
                    cartItem.Quantity++;
                }
                await _context.SaveChangesAsync();
            }
            TempData["Success"] = "Added product to Cart";
            return Redirect(Request.Headers["Referer"].ToString()); // trả về trang hiện tại
        }

        [HttpPost]
        public IActionResult Increase(int id, string size, string color)
        {
            var userId = HttpContext.Session.GetInt32("_UserId"); // Kiểm tra nếu đã đăng nhập
            if (userId != null)
            {
                // Đã đăng nhập, cập nhật trong cơ sở dữ liệu
                var cart = _context.Carts.Include(c => c.CartItems)
                                         .FirstOrDefault(c => c.UserId == userId);
                var cartItem = cart?.CartItems.FirstOrDefault(ci => ci.ProductId == id && ci.Size == size && ci.Color == color);

                if (cartItem != null)
                {
                    cartItem.Quantity++;
                    _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                }
            }
            else
            {
                // Chưa đăng nhập, lưu giỏ hàng vào session
                List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
                CartItem cartItem = cart?.FirstOrDefault(c => c.ProductId == id && c.Size == size && c.Color == color);
                if (cartItem != null)
                {
                    cartItem.Quantity++;
                    HttpContext.Session.SetJson("Cart", cart);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Decrease(int id, string size, string color)
        {
            var userId = HttpContext.Session.GetInt32("_UserId"); // Kiểm tra nếu đã đăng nhập
            if (userId != null)
            {
                // Đã đăng nhập, cập nhật trong cơ sở dữ liệu
                var cart = _context.Carts.Include(c => c.CartItems)
                                         .FirstOrDefault(c => c.UserId == userId);
                var cartItem = cart?.CartItems.FirstOrDefault(ci => ci.ProductId == id && ci.Size == size && ci.Color == color);

                if (cartItem != null)
                {
                    cartItem.Quantity--;
                    if (cartItem.Quantity <= 0)
                        _context.CartItems.Remove(cartItem); // Xóa item nếu số lượng <= 0

                    _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                }
            }
            else
            {
                // Chưa đăng nhập, lưu giỏ hàng vào session
                List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
                CartItem cartItem = cart?.FirstOrDefault(c => c.ProductId == id && c.Size == size && c.Color == color);
                if (cartItem != null)
                {
                    cartItem.Quantity--;
                    if (cartItem.Quantity <= 0)
                        cart.Remove(cartItem); // Xóa item nếu số lượng <= 0

                    HttpContext.Session.SetJson("Cart", cart);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remove(int id, string size, string color)
        {
            var userId = HttpContext.Session.GetInt32("_UserId"); // Kiểm tra nếu đã đăng nhập
            if (userId != null)
            {
                // Đã đăng nhập, xóa trong cơ sở dữ liệu
                var cart = _context.Carts.Include(c => c.CartItems)
                                         .FirstOrDefault(c => c.UserId == userId);
                var cartItem = cart?.CartItems.FirstOrDefault(ci => ci.ProductId == id && ci.Size == size && ci.Color == color);

                if (cartItem != null)
                {
                    cart.CartItems.Remove(cartItem);
                    _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                }
            }
            else
            {
                // Chưa đăng nhập, xóa từ session
                List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
                CartItem cartItem = cart?.FirstOrDefault(c => c.ProductId == id && c.Size == size && c.Color == color);
                if (cartItem != null)
                {
                    cart.Remove(cartItem);
                    HttpContext.Session.SetJson("Cart", cart);
                }
            }
            TempData["Success"] = "Removed product";
            return RedirectToAction("Index");

        }
    }
}
