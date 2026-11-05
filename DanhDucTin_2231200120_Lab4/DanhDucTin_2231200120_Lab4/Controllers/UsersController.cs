using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab4.Models; 
using System;
using System.Threading.Tasks;

namespace Lab4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly BookManagementContext _context;

        public UsersController(BookManagementContext context)
        {
            _context = context;
        }

        // POST /api/users/register: Register new users (hash password, generate ActiveCode, and simulate sending activation email).
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            // Kiểm tra xem email đã tồn tại trong hệ thống chưa
            var emailExists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (emailExists)
            {
                return BadRequest("This email address has already been used");
            }

            // Băm mật khẩu bằng thư viện BCrypt.Net-Next (cài bằng Install-Package BCrypt.Net-Next)
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // Sinh mã kích hoạt ngẫu nhiên 
            string activationCode = Guid.NewGuid().ToString();

            // Tạo đối tượng User mới để lưu xuống Database (Dùng bảng Users số nhiều)
            var newUser = new Users
            {
                Fullname = dto.Username,
                Email = dto.Email,
                Password = passwordHash,
                ActiveCode = activationCode,
                IsActive = false, // Mới đăng ký thì tài khoản chưa được kích hoạt
                CreatedDate = DateTime.UtcNow
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // GIẢ LẬP GỬI EMAIL
            Console.WriteLine($"[SIMULATE EMAIL] Send to: {newUser.Email}");
            Console.WriteLine($"Your activation link (GET): https://localhost:44328/api/users/activate?code={activationCode}");
            Console.WriteLine($"Or activation code (POST JSON): {activationCode}");

            return Ok(new { message = "Registration successful! Please check the Console for your activation code" });
        }


        // GET /api/users/activate?code=xxx:
        [HttpGet("activate")]
        public async Task<IActionResult> ActivateByGet([FromQuery] string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("Activation code cannot be empty");
            }

            // Tìm người dùng có mã kích hoạt trùng với mã trên URL
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ActiveCode == code);

            if (user == null)
            {
                return BadRequest("The activation code is incorrect or does not exist");
            }

            if (user.IsActive)
            {
                return BadRequest("This account was already activated");
            }

            // Cập nhật trạng thái kích hoạt tài khoản
            user.IsActive = true;
            user.ActiveCode = null; // Xóa mã kích hoạt sau khi dùng xong để bảo mật

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Account activation successful via GET! You can now log in." });
        }

        
        // 3. POST: /api/users/activate 
        [HttpPost("activate")]
        public async Task<IActionResult> ActivateByPost([FromBody] ActivateDto dto)
        {
            // Tìm người dùng có mã kích hoạt trùng với mã trong body JSON gửi lên
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ActiveCode == dto.Code);

            if (user == null)
            {
                return BadRequest("The activation code is incorrect or does not exist");
            }

            if (user.IsActive)
            {
                return BadRequest("This account was already activated");
            }

            // Cập nhật trạng thái kích hoạt tài khoản
            user.IsActive = true;
            user.ActiveCode = null; // Xóa mã kích hoạt sau khi dùng xong để bảo mật

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Account activation successful via POST JSON! You can now log in." });
        }
    }
}