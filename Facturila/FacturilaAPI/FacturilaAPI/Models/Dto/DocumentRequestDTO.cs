using Microsoft.VisualBasic;

namespace FacturilaAPI.Models.Dto
{
    public class DocumentRequestDTO
    {
        public int Id { get; set; }
        public FirmDto Client { get; set; }
        public DocumentSeriesDto DocumentSeries { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? DueDate {  get; set; }
        public List<DocumentProductRequestDTO> Products { get; set; }
    } 
}
