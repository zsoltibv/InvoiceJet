using FacturilaAPI.Models.Dto;
using FacturilaAPI.Models.Entity;
using FacturilaAPI.Services;
using FacturilaAPI.Services.Impl;
using Microsoft.AspNetCore.Mvc;

namespace FacturilaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] UserDto userDto)
        {
            User userToRegister = await _authService.RegisterUser(userDto);
            return Ok(userToRegister);
        } 
    }
}
