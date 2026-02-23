using backend.DTO;
using backend.Models;
using backend.Servies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using BCryptNet = BCrypt.Net.BCrypt;

namespace backend.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;
        private readonly CnpmContext _context;
        private readonly JwtService _jwtService;
        private readonly AuthService _authService;

        public AuthController(CnpmContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _jwtService = new JwtService(_config);
            _authService = new AuthService(_context, _jwtService, _config);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == req.Email);

            if (user == null) // kiểm tra tài khoản email tồn tại không
            {
                return BadRequest(new
                {
                    success = false,
                    message = "No account"
                });
            }
            bool isValidPassword = BCryptNet.Verify(req.Password, user.PasswordHash);

            if (!isValidPassword) // Kiểm tra password
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Wrong password"
                });
            }

            var (accessToken, refreshToken) = await _authService.IssueTokensAsync(user);

            Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddDays(7)
            });

            return Ok(new AuthResponse
            {
                AccessToken = accessToken,
                ExpiresAt = DateTime.Now.AddMinutes(15)
            });

        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            var u = await _context.Users.FirstOrDefaultAsync(u => u.Email == req.Email);

            if (u != null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Tài khoản đã tồn tại"
                });
            }

            User user = new User
            {
                Id = Guid.NewGuid(),
                Email = req.Email,
                PasswordHash = BCryptNet.HashPassword(req.PasswordHash),
                Name = req.Name,
                Avt = "/Img/User_Icon.png",
                Role = "User",
                CreatedAt = DateTime.Now,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var (accessToken, refreshToken) = await _authService.IssueTokensAsync(user);
            Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddDays(7)
            });

            return Ok(new AuthResponse
            {
                AccessToken = accessToken,
                ExpiresAt = DateTime.Now.AddMinutes(15)
            });
        }

        [Authorize]
        [HttpPost("LogOut")]
        public async Task<IActionResult> Logout()
        {
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["RefreshToken"];

                // kiểm tra access token
                var authHeader = Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                    return Unauthorized("Không tìm thấy Access Token cũ");

                var oldAccessToken = authHeader.Replace("Bearer ", "");
                var principal = _jwtService.GetPrincipalFromExpiredToken(oldAccessToken);

                var userIdStr = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdStr)) return Unauthorized("Token rác!");

                var userId = Guid.Parse(userIdStr);
                var isValid = await _jwtService.ValidateRefreshTokenAsync(_context, userId, refreshToken);
                // kiểm tra refresh token
                if (!isValid) return Unauthorized("Refresh Token đã hết hạn hoặc không hợp lệ!");
                var user = await _context.Users.FindAsync(userId);
                var (newAccessToken, newRefreshToken) = await _authService.IssueTokensAsync(user);

                // cấp mới
                Response.Cookies.Append("RefreshToken", newRefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.Now.AddDays(7)
                });

                return Ok(new AuthResponse
                {
                    AccessToken = newAccessToken,
                    ExpiresAt = DateTime.Now.AddMinutes(15)
                });
            }
            catch (Exception)
            {
                return Unauthorized("Không refresh thành công");
            }
        }

        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword()
        {


            return Ok();
        }

        [Authorize]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword()
        {
            return Ok();
        }
    }
}
