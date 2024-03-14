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

        [HttpPut("addOrEditFirm/{userId}/{isClient}")]
        public async Task<ActionResult> AddOrEditFirm([FromBody] FirmDto firmDto, Guid userId, bool isClient)
        {
            var updatedOrNewFirm = await _firmService.AddOrEditFirm(firmDto, userId, isClient);
            return Ok(updatedOrNewFirm);
        }

        [HttpGet("GetUserActiveFirmById/{userId}")]
        public async Task<ActionResult> GetUserActiveFirmById(Guid userId)
        {
            var firm = await _firmService.GetUserActiveFirmById(userId);
            return Ok(firm);
        }

        [HttpGet("GetUserClientFirmsById/{userId}")]
        public async Task<ActionResult> GetUserClientFirmsById(Guid userId)
        {
            var firm = await _firmService.GetUserClientFirmsById(userId);
            return Ok(firm);
        }
    }
}