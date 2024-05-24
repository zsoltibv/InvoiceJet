using FacturilaAPI.Models.Entity;
using Microsoft.VisualBasic;

namespace FacturilaAPI.Models.Dto
{
    public class DocumentRequestDTO
    {
        public int Id { get; set; }
        public string? DocumentNumber { get; set; }
        public FirmDto? Seller { get; set; }
        public FirmDto Client { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DocumentSeriesDto? DocumentSeries { get; set; }
        public DocumentType? DocumentType { get; set; }
        public DocumentStatus? DocumentStatus { get; set; }
        public List<DocumentProductRequestDTO> Products { get; set; }
    } 
}
