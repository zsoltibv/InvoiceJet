using FacturilaAPI.Models.Entity;

namespace FacturilaAPI.Models.Dto;

public class DocumentAutofillDto
{
    public List<Firm> Clients { get; set; }
    public List<DocumentSeries> DocumentSeries { get;set; }
    public List<DocumentStatus> DocumentStatuses { get; set; }
    public List<Product> Products { get; set; }
}