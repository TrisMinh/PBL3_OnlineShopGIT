using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.Repository;
using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.Models.ViewModels;

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
        public ActionResult Index()
        {
            List<CartItem> cartItems = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            CartItemView cartView = new CartItemView
            {
                CartItems = cartItems,
                TotalPrice = cartItems.Sum(item => item.SellingPrice * item.Quantity)
            };
            return View(cartView);
        }
        public ActionResult Checkout()
        {
            return View();
        }
        public async Task<IActionResult> Add(int id, string size)
        {
            Product product = await _context.Products.FindAsync(id);
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            CartItem cartItem = cart.FirstOrDefault(c => c.ProductId == id && c.Size == size);
            if (cartItem == null)
            {
                cartItem = new CartItem(product)
                {
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
            TempData["Success"] = "Added product to Cart";
            return Redirect(Request.Headers["Referer"].ToString()); // trả về trang hiện tại
        }

        [HttpPost]
        public IActionResult Increase(int id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            CartItem item = cart?.FirstOrDefault(x => x.ProductId == id);
            if (item != null)
            {
                item.Quantity++;
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Decrease(int id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            CartItem item = cart?.FirstOrDefault(x => x.ProductId == id);
            if (item != null)
            {
                item.Quantity--;
                if (item.Quantity <= 0)
                    cart.Remove(item);

                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            CartItem item = cart?.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                cart.Remove(item);
                HttpContext.Session.SetJson("Cart", cart);
            }
            TempData["Success"] = "Removed product";
            return RedirectToAction("Index");

        }
    }
}
