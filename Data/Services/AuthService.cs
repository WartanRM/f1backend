using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using F1Backend.Models;
using F1Backend.Data;

namespace F1Backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly F1DbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(F1DbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<User> RegisterAsync(RegisterDTO registerDto)
        {
            try
            {
                // Check if the user already exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == registerDto.Email);

                if (existingUser != null)
                {
                    throw new Exception("User already exists");
                }

                // Create new User object
                var user = new User
                {
                    Name = registerDto.Name,
                    Email = registerDto.Email,
                    Password = HashPassword(registerDto.Password),
                    Age = registerDto.Age
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return user;
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine("Database update error: " + dbEx.InnerException?.Message);
                throw new Exception("Database update error", dbEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General error: " + ex.Message);
                throw;
            }
        }

        public async Task<string> LoginAsync(LoginDTO loginDto)
        {
            // Find the user by email
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            // Validate the user's credentials
            if (existingUser == null || !VerifyPassword(loginDto.Password, existingUser.Password))
            {
                throw new Exception("Invalid email or password");
            }

            // Generate and return the JWT token
            var token = GenerateJwtToken(existingUser);
            return token;
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hashedInputPassword = HashPassword(password);
            return hashedInputPassword == hashedPassword;
        }
    }
}
