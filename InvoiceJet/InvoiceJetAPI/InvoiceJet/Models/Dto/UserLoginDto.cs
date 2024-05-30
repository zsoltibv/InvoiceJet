using Microsoft.AspNetCore.Mvc;

namespace InvoiceJetAPI.Models.Dto
{
    public class UserLoginDto 
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
