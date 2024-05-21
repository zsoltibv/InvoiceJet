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

        [HttpPost("GetInvoicePdfStream")]
        public async Task<IActionResult> GetInvoicePdfStream(DocumentRequestDTO documentRequestDTO)
        {
            try
            {
                DocumentStreamDto documentStreamDto = await _documentService.GetInvoicePdfStream(documentRequestDTO);
                if (documentStreamDto.PdfContent == null)
                {
                    return BadRequest("Failed to generate the PDF document.");
                }

                return File(documentStreamDto.PdfContent, "application/pdf", $"Invoice_{documentStreamDto.DocumentNumber}.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetDocumentTableRecords/{documentTypeId}")]
        public async Task<IActionResult> GetDocumentTableRecords(int documentTypeId)
        {
            try
            {
                var documents = await _documentService.GetDocumentTableRecords(documentTypeId);
                return Ok(documents);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetDocumentById/{documentId}")]
        public async Task<IActionResult> GetDocumentById(int documentId)
        {
            try
            {
                var document = await _documentService.GetDocumentById(documentId);
                return Ok(document);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}