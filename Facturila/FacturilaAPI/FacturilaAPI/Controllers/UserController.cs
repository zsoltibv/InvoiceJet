using FacturilaAPI.Models.Dto;
using FacturilaAPI.Models.Entity;
using FacturilaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FacturilaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetUserByEmail")]
        public async Task<ActionResult<User>> GetUserByEmail(string email)
        {
            User user = await _userService.GetUserByEmail(email);
            return Ok(user);
        }
    }
}
