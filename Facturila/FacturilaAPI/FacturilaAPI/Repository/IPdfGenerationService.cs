using FacturilaAPI.Models.Dto;

namespace FacturilaAPI.Repository
{
    public interface IPdfGenerationService
    {
        string GenerateInvoicePdf(DocumentRequestDTO invoiceData);
    }
}
