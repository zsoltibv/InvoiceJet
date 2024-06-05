using InvoiceJet.Application.DTOs;
using InvoiceJet.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceJet.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "User")]
public class DocumentSeriesController : ControllerBase
{
    private readonly IDocumentSeriesService _documentSeriesService;

    public DocumentSeriesController(IDocumentSeriesService documentSeriesService)
    {
        _documentSeriesService = documentSeriesService;
    }

    [HttpGet("GetAllDocumentSeriesForUserId/{userId}")]
    public async Task<ActionResult<DocumentSeriesDto>> GetAllDocumentSeriesForUserId(Guid userId)
    {
        var bankAccountDto = await _documentSeriesService.GetAllDocumentSeriesForUserId(userId);
        return Ok(bankAccountDto);
    }
}