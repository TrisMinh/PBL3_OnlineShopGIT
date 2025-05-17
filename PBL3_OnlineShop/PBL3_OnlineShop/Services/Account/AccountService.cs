using Microsoft.AspNetCore.Identity;
using PBL3_OnlineShop.Data;
using PBL3_OnlineShop.Models.ViewModels;
using PBL3_OnlineShop.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace PBL3_OnlineShop.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly PBL3_Db_Context _context;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public AccountService(PBL3_Db_Context context)
        {
            _context = context;
        }

        public bool IsUserNameExists(string username)
        {
            return _context.Users.Any(u => u.UserName == username);
        }
            

        public bool IsEmailExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }
            

        public void CreateUser(RegisterView model)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password,
                Gender = "Man",
                UrlAvatar = "/avatar/def.jpg",
                Role = "Customer",
                CreatedAt = DateTime.Now,
                Status = 1,
                Cart = new Models.Cart()
            };
            user.Password = _passwordHasher.HashPassword(user, model.Password);
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User FindByUsernameOrEmail(string input) =>
            _context.Users.FirstOrDefault(u => u.UserName == input || u.Email == input);

        public void ResetPassword(User user, string newPassword)
        {
            user.Password = _passwordHasher.HashPassword(user, newPassword);
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public User FindByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == username);
        }

        public bool VerifyPassword(User user, string password)
        {
            return _passwordHasher.VerifyHashedPassword(user, user.Password, password) != PasswordVerificationResult.Failed;
        }

        public User GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public bool UpdateProfile(int userId, string userName, string email, string phoneNumber, string gender, int day, int month, int year, string address)
        {
            try
            {
                var user = _context.Users.Find(userId);
                if (user == null)
                {
                    return false;
                }
                
                // Cập nhật thông tin người dùng từ tham số
                user.UserName = userName;
                user.Email = email;
                
                // Cập nhật số điện thoại
                user.PhoneNumber = phoneNumber;
                
                // Lưu giới tính
                user.Gender = gender;
                
                // Xử lý ngày sinh
                try
                {
                    user.DateOfBirth = new DateTime(year, month, day);
                }
                catch
                {
                    return false;
                }
                
                // Lưu địa chỉ
                user.Address = address;
                
                // Lưu thay đổi
                _context.SaveChanges();
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ChangePassword(int userId, string currentPassword, string newPassword)
        {
            try
            {
                var user = _context.Users.Find(userId);
                if (user == null)
                {
                    return false;
                }
                if (!VerifyPassword(user, currentPassword))
                {
                    return false;
                }

                // Mã hóa và lưu mật khẩu mới
                user.Password = _passwordHasher.HashPassword(user, newPassword);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<(bool success, string message, string avatarUrl)> UploadAvatar(int userId, IFormFile file)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return (false, "Không tìm thấy người dùng", null);
                }

                if (file == null || file.Length == 0)
                {
                    return (false, "Vui lòng chọn ảnh", null);
                }

                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                string fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return (false, "Chỉ chấp nhận định dạng JPG, JPEG, PNG hoặc GIF", null);
                }

                // Tạo tên file duy nhất để tránh trùng lặp
                string uniqueFileName = $"{userId}_{DateTime.Now.ToString("yyyyMMddHHmmss")}{fileExtension}";
                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatar");
                
                // Tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                // Lưu file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Xóa avatar cũ nếu có và không phải ảnh mặc định
                if (!string.IsNullOrEmpty(user.UrlAvatar) && !user.UrlAvatar.Contains("def"))
                {
                    string oldAvatarPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.UrlAvatar.TrimStart('/'));
                    if (System.IO.File.Exists(oldAvatarPath))
                    {
                        System.IO.File.Delete(oldAvatarPath);
                    }
                }

                // Cập nhật đường dẫn avatar trong database
                user.UrlAvatar = $"/avatar/{uniqueFileName}";
                await _context.SaveChangesAsync();

                return (true, "Cập nhật avatar thành công", user.UrlAvatar);
            }
            catch (Exception ex)
            {
                return (false, "Đã xảy ra lỗi: " + ex.Message, null);
            }
        }

        public void ApplySessionCartToUser(string tempCart, int? userId, ISession session)
        {
            if (!string.IsNullOrEmpty(tempCart))
            {
                var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(tempCart);
                var cart = _context.Carts.Include(c => c.CartItems).FirstOrDefault(c => c.UserId == userId);

                if (cart == null)
                {
                    cart = new Models.Cart { UserId = userId.Value };
                    _context.Carts.Add(cart);
                }

                foreach (var item in cartItems)
                {
                    var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == item.ProductId && ci.Size == item.Size && ci.Color == item.Color);
                    if (existingItem != null)
                    {
                        existingItem.Quantity += item.Quantity;
                    }
                    else
                    {
                        cart.CartItems.Add(item);
                    }
                }

                _context.SaveChanges();
                session.Remove("Cart");  // Xóa giỏ hàng tạm thời sau khi đã chuyển vào cơ sở dữ liệu
            }
        }
    }
}
