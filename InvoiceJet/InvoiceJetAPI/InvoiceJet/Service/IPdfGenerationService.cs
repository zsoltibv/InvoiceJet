using InvoiceJetAPI.Models.Dto;

namespace InvoiceJetAPI.Repository
{
    public interface IPdfGenerationService
    {
        string GenerateInvoicePdf(DocumentRequestDTO invoiceData);
        byte[] GetInvoicePdfStream(DocumentRequestDTO invoiceData);
    }
}
