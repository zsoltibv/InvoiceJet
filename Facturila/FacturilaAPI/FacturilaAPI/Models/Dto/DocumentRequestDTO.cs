using Microsoft.VisualBasic;

namespace FacturilaAPI.Models.Dto
{
    public class DocumentRequestDTO
    {
        public FirmDto Firm { get; set; }
        public DocumentSeriesDto DocumentSeriesDto { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate {  get; set; }
        public List<DocumentProductRequestDTO> Products { get; set; }
    } 
}
