using FacturilaAPI.Models.Dto;
using FacturilaAPI.Repository;
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

        [HttpPost("AddOrEditDocument")]
        public async Task<ActionResult<DocumentRequestDTO>> AddOrEditDocument(DocumentRequestDTO documentRequestDto)
        {
            try
            {
                await _documentService.AddOrEditDocument(documentRequestDto);
                return Ok(documentRequestDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GenerateDocumentPdf")]
        public async Task<ActionResult<DocumentRequestDTO>> GenerateDocument(DocumentRequestDTO documentRequestDTO)
        {
            try
            {
                await _documentService.GeneratePdfDocument(documentRequestDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}