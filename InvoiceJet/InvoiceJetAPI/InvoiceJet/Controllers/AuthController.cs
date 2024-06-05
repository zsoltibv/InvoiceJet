using InvoiceJet.Application.DTOs;
using InvoiceJet.Application.Services;
using InvoiceJet.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceJet.Presentation.Controllers;

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
        var userToRegister = await _authService.RegisterUser(userDto);
        return Ok(userToRegister);
    }

    [HttpPost("login")]
    public async Task<ActionResult<User>> Login([FromBody] UserLoginDto userDto)
    {
        try
        {
            string token = await _authService.LoginUser(userDto);
            return Ok(token);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
