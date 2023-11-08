using Microsoft.AspNetCore.Mvc;

namespace FacturilaAPI.Models.Dto
{
    public class UserLoginDto 
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
