using FacturilaAPI.Models.Dto;
using FacturilaAPI.Models.Entity;
using FacturilaAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FacturilaAPI.Controllers
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
        public async Task<ActionResult<User>> Register([FromBody] UserRegisterDto userDto)
        {
            User userToRegister = await _authService.RegisterUser(userDto);
            return Ok(userToRegister);
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] UserLoginDto userDto)
        {
            string token = await _authService.LoginUser(userDto);
            return Ok(token);
        }
    }
}
