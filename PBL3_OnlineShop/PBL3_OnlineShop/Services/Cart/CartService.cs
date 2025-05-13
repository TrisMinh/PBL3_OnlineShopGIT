using PBL3_OnlineShop.Data;
using PBL3_OnlineShop.Models.ViewModels;
using PBL3_OnlineShop.Models;
using PBL3_OnlineShop.SessionHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis;
using System.Drawing;


namespace PBL3_OnlineShop.Services.Cart
{
    public class CartService : ICartService
    {
        private readonly PBL3_Db_Context _context;

        public CartService(PBL3_Db_Context context)
        {
            _context = context;
        }

        public CartItemView GetCartView(int? userId, ISession session)
        {
            if (userId == null)
            {
                var cartItems = session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
                return new CartItemView
                {
                    CartItems = cartItems,
                    TotalPrice = cartItems.Sum(item => item.SellingPrice * item.Quantity)
                };
            }

            var cart = _context.Carts.Include(c => c.CartItems)
                                     .ThenInclude(ci => ci.Product)
                                     .FirstOrDefault(c => c.UserId == userId);

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                return new CartItemView { CartItems = new List<CartItem>(), TotalPrice = 0 };
            }

            return new CartItemView
            {
                CartItems = cart.CartItems.ToList(),
                TotalPrice = cart.CartItems.Sum(item => item.Quantity * item.SellingPrice)
            };
        }

        public Models.Cart GetCartByIdUser(int? userId)
        {
            return _context.Carts.Include(c => c.CartItems).FirstOrDefault(c => c.UserId == userId);
        }

        public Models.ProductSize GetProductSizeByIdSizeColor(int productId, string size, string color)
        {
            return _context.ProductsSize.FirstOrDefault(ps => ps.ProductId == productId && ps.Color == color && ps.Size == size);
        }

        public Models.User GetUserById(int? userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }

        public Models.Product GetProductByID(int productId)
        {
            return _context.Products.Find(productId);
        }

        public string CheckCartItem(int? userId)
        {
            var cart = _context.Carts.Include(c => c.CartItems).FirstOrDefault(c => c.UserId == userId);

            if (cart.CartItems == null || !cart.CartItems.Any())
            {
                return "Your cart is empty.";
            }
            foreach (var item in cart.CartItems)
            {
                var product = _context.ProductsSize.FirstOrDefault(ps => ps.ProductId == item.ProductId && ps.Color == item.Color && ps.Size == item.Size);

                if (product.Quantity < item.Quantity)
                {
                    return "Not enough stock for " + item.ProductName + " Color: " + item.Color + " Size: " + item.Size;
                }
            }
            return "OK";
        }

        public void AddToCart(int? userId, int id, string size, string color, ISession session, Models.Product product)
        {
            if (userId == null)
            {
                var cart = session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
                var cartItem = cart.FirstOrDefault(c => c.ProductId == id && c.Size == size && c.Color == color);
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
                session.SetJson("Cart", cart);
            }
            else
            {
                var cart = _context.Carts.Include(c => c.CartItems).FirstOrDefault(c => c.UserId == userId);
                if (cart == null)
                {
                    cart = new Models.Cart
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
                _context.SaveChanges();
            }

        }

        public void IncreaseQuantity(int? userId, int id, string size, string color, ISession session)
        {
            if (userId != null)
            {
                var cart = _context.Carts.Include(c => c.CartItems).FirstOrDefault(c => c.UserId == userId);
                var cartItem = cart?.CartItems.FirstOrDefault(c => c.ProductId == id && c.Size == size && c.Color == color);
                if (cartItem != null)
                {
                    cartItem.Quantity++;
                    _context.SaveChanges();
                }
            }
            else
            {
                var cart = session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
                var cartItem = cart.FirstOrDefault(c => c.ProductId == id && c.Size == size && c.Color == color);
                if (cartItem != null)
                {
                    cartItem.Quantity++;
                    session.SetJson("Cart", cart);
                }
            }
        }

        public void DecreaseQuantity(int? userId, int id, string size, string color, ISession session)
        {
            if (userId != null)
            {
                var cart = _context.Carts.Include(c => c.CartItems).FirstOrDefault(c => c.UserId == userId);
                var cartItem = cart?.CartItems.FirstOrDefault(c => c.ProductId == id && c.Size == size && c.Color == color);
                if (cartItem != null)
                {
                    cartItem.Quantity--;
                    if (cartItem.Quantity <= 0) _context.CartItems.Remove(cartItem);
                    _context.SaveChanges();
                }
            }
            else
            {
                var cart = session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
                var cartItem = cart.FirstOrDefault(c => c.ProductId == id && c.Size == size && c.Color == color);
                if (cartItem != null)
                {
                    cartItem.Quantity--;
                    if (cartItem.Quantity <= 0) cart.Remove(cartItem);
                    session.SetJson("Cart", cart);
                }
            }
        }

        public void RemoveItem(int? userId, int id, string size, string color, ISession session)
        {
            if (userId != null)
            {
                var cart = _context.Carts.Include(c => c.CartItems).FirstOrDefault(c => c.UserId == userId);
                var cartItem = cart?.CartItems.FirstOrDefault(c => c.ProductId == id && c.Size == size && c.Color == color);
                if (cartItem != null)
                {
                    cart.CartItems.Remove(cartItem);
                    _context.SaveChanges();
                }
            }
            else
            {
                var cart = session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
                var cartItem = cart.FirstOrDefault(c => c.ProductId == id && c.Size == size && c.Color == color);
                if (cartItem != null)
                {
                    cart.Remove(cartItem);
                    session.SetJson("Cart", cart);
                }
            }
        }
    }
    }
