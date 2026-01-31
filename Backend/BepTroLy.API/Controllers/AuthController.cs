using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BepTroLy.Application.DTOs;
using BepTroLy.Application.Services;

namespace BepTroLy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, message, token, user) = await _authService.RegisterAsync(request.Email, request.Password, request.Name);

            if (!success)
            {
                return BadRequest(new AuthResponse { Success = false, Message = message });
            }

            return Ok(new AuthResponse
            {
                Success = true,
                Message = message,
                Token = token,
                User = new UserDto { Id = user.Id, Email = user.Email, Name = user.Name }
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, message, token, user) = await _authService.LoginAsync(request.Email, request.Password);

            if (!success)
            {
                return Unauthorized(new AuthResponse { Success = false, Message = message });
            }

            return Ok(new AuthResponse
            {
                Success = true,
                Message = message,
                Token = token,
                User = new UserDto { Id = user.Id, Email = user.Email, Name = user.Name }
            });
        }
    }
}

