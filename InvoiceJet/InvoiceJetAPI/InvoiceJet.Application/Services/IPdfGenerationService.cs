using InvoiceJet.Application.DTOs;

namespace InvoiceJet.Infrastructure.Services;

public interface IPdfGenerationService
{
    string GenerateInvoicePdf(DocumentRequestDto invoiceData);
    byte[] GetInvoicePdfStream(DocumentRequestDto invoiceData);
}