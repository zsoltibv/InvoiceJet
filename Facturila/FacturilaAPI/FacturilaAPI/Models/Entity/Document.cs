namespace FacturilaAPI.Models.Entity;

public class Document
{
    public int Id { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime? DueDate { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }

    public int? DocumentTypeId { get; set; }
    public DocumentType? DocumentType { get; set; }

    public int? DocumentStatusId { get; set; }
    public virtual DocumentStatus? DocumentStatus { get; set; }

    public int? ClientId { get; set; }
    public virtual Firm? Client { get; set; }
    
    public int? UserFirmId { get; set; }
    public virtual UserFirm? UserFirm { get; set; }

    public virtual ICollection<DocumentProduct>? DocumentProducts { get; set; }
}