using FacturilaAPI.Models.Dto;

namespace FacturilaAPI.Services;

public interface IDocumentService
{
    Task<DocumentAutofillDto> GetDocumentAutofillInfo(Guid userId, int documentTypeId);
    Task<DocumentRequestDTO> AddDocument(DocumentRequestDTO documentRequestDTO);
    Task<DocumentRequestDTO> EditDocument(DocumentRequestDTO documentRequestDTO);
    Task<DocumentRequestDTO> GeneratePdfDocument(DocumentRequestDTO documentRequestDTO);
    Task<DocumentStreamDto> GetInvoicePdfStream(DocumentRequestDTO documentRequestDTO);
    Task<List<DocumentTableRecordDTO>> GetDocumentTableRecords(int documentTypeId);
    Task<DocumentRequestDTO> GetDocumentById(int documentId);
    Task DeleteDocuments(int[] documentIds);
    Task<DashboardStatsDto> GetDashboardStats();
    Task TransformToStorno(int[] documentIds);
}