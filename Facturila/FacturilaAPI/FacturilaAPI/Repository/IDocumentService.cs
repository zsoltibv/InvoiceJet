using FacturilaAPI.Models.Dto;

namespace FacturilaAPI.Services;

public interface IDocumentService
{
    Task<DocumentAutofillDto> GetDocumentAutofillInfo(Guid userId, int documentTypeId);
}