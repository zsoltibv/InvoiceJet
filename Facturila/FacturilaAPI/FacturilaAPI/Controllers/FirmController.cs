using FacturilaAPI.Models.Dto;
using FacturilaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FacturilaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User")]
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

        [HttpPut("addOrEditFirm")]
        public async Task<ActionResult> AddOrEditFirm([FromBody] FirmDto firmDto)
        {
            try
            {
                var updatedOrNewFirm = await _firmService.AddOrEditFirm(firmDto);
                return Ok(updatedOrNewFirm);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
} 
