using FacturilaAPI.Models.Dto;

namespace FacturilaAPI.Services;

public interface IDocumentService
{
    Task<DocumentAutofillDto> GetDocumentAutofillInfo(Guid userId, int documentTypeId);
    Task<DocumentRequestDTO> AddOrEditDocument(DocumentRequestDTO documentRequestDTO);
    Task<DocumentRequestDTO> GeneratePdfDocument(DocumentRequestDTO documentRequestDTO);
    Task<byte[]> GetInvoicePdfStream(DocumentRequestDTO documentRequestDTO);
}