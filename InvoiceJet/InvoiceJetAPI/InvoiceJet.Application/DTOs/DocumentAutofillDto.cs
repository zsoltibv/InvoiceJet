using InvoiceJet.Domain.Models;

namespace InvoiceJet.Application.DTOs;

public class DocumentAutofillDto
{
    public List<Firm> Clients { get; set; } = null!;
    public List<DocumentSeries> DocumentSeries { get; set; } = null!;
    public List<DocumentStatus> DocumentStatuses { get; set; } = null!;
    public List<Product> Products { get; set; } = null!;
}