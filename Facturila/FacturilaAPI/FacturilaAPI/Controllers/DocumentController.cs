using FacturilaAPI.Models.Dto;
using FacturilaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FacturilaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }
        
        [HttpGet("GetDocumentAutofillInfo/{userId}/{documentTypeId}")]
        public async Task<ActionResult<DocumentAutofillDto>> GetDocumentAutofillInfo(Guid userId, int documentTypeId)
        {
            try
            {
                var documentAutofillDto = await _documentService.GetDocumentAutofillInfo(userId, documentTypeId);
                return Ok(documentAutofillDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}