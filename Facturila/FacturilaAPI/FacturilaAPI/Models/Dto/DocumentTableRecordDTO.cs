namespace FacturilaAPI.Models.Dto
{
    public class DocumentTableRecordDTO
    {
        public int Id { get; set; }
        public string DocumentNumber { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal TotalValue { get; set; }
    }
}
