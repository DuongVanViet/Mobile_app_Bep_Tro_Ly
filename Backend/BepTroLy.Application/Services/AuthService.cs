using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BepTroLy.Domain.Entities;
using BepTroLy.Infrastructure.Persistence;
using BepTroLy.Infrastructure.Security;
using BC = BCrypt.Net.BCrypt;

namespace BepTroLy.Application.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, string Token, User User)> RegisterAsync(string email, string password, string name);
        Task<(bool Success, string Message, string Token, User User)> LoginAsync(string email, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly AppDbContext _dbContext;
        private readonly JwtTokenProvider _jwtTokenProvider;

        public AuthService(AppDbContext dbContext, JwtTokenProvider jwtTokenProvider)
        {
            _dbContext = dbContext;
            _jwtTokenProvider = jwtTokenProvider;
        }

        public async Task<(bool Success, string Message, string Token, User User)> RegisterAsync(string email, string password, string name)
        {
            // Check if user already exists
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser != null)
            {
                return (false, "User with this email already exists", string.Empty, null!);
            }

            // Hash password
            var hashedPassword = BC.HashPassword(password);

            // Create new user
            var user = new User
            {
                Email = email,
                Name = name,
                PasswordHash = hashedPassword,
                CreatedAt = System.DateTime.UtcNow
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            // Generate JWT token
            var token = _jwtTokenProvider.GenerateToken(user.Id, user.Email ?? "", user.Name ?? "");

            return (true, "User registered successfully", token, user);
        }

        public async Task<(bool Success, string Message, string Token, User User)> LoginAsync(string email, string password)
        {
            // Find user by email
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return (false, "Invalid email or password", string.Empty, null!);
            }

            // Verify password
            if (!BC.Verify(password, user.PasswordHash ?? ""))
            {
                return (false, "Invalid email or password", string.Empty, null!);
            }

            // Generate JWT token
            var token = _jwtTokenProvider.GenerateToken(user.Id, user.Email ?? "", user.Name ?? "");

            return (true, "Login successful", token, user);
        }
    }
}
