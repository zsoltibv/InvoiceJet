using FacturilaAPI.Models.Dto;
using FacturilaAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FacturilaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FirmController : ControllerBase
    {
        private readonly IFirmService _firmService;

        public FirmController(IFirmService firmService)
        {
            _firmService = firmService;
        }

        [HttpGet("fromAnaf/{cui}")]
        public async Task<ActionResult<FirmDto>> GetFirmDataFromAnaf(string cui)
        {
            FirmDto firmDataDto = await _firmService.GetFirmDataFromAnaf(cui);
            return Ok(firmDataDto);
        }
    }
}
