using InvoiceJetAPI.Models.Entity;

namespace InvoiceJetAPI.Models.Dto
{
    public class DocumentSeriesDto
    {
        public int Id { get; set; }
        public string SeriesName { get; set; } = string.Empty;
        public int FirstNumber { get; set; }
        public int CurrentNumber { get; set; }
        public bool IsDefault { get; set; }
        public int? DocumentTypeId { get; set; }
        public DocumentType? DocumentType { get; set; }
    }
}