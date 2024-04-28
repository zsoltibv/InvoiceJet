namespace FacturilaAPI.Models.Entity;

public class Invoice
{
    public int Id { get; set; }
    public DateOnly IssueDate { get; set; }
    public DateOnly DueDate { get; set; }
    
    public int DocumentSeriesId { get; set; }
    public DocumentSeries? DocumentSeries { get; set; }

    public int ClientId { get; set; }
    public virtual Firm? Client { get; set; }
    
    public int UserFirmId { get; set; }
    public virtual UserFirm? UserFirm { get; set; }
}