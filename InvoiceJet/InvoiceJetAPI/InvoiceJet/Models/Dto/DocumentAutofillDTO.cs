using InvoiceJetAPI.Models.Entity;

namespace InvoiceJetAPI.Models.Dto;

public class DocumentAutofillDto
{
    public List<Firm> Clients { get; set; }
    public List<DocumentSeries> DocumentSeries { get;set; }
    public List<DocumentStatus> DocumentStatuses { get; set; }
    public List<Product> Products { get; set; }
}