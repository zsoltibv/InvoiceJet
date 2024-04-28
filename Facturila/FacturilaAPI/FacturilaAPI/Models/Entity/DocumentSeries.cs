namespace FacturilaAPI.Models.Entity
{
    public class DocumentSeries
    {
        public int Id { get; set; }
        public string SeriesName { get; set; } = string.Empty;
        public int FirstNumber { get; set; }
        public int CurrentNumber { get; set; }
        public bool IsDefault { get; set; }

        public int? DocumentTypeId { get; set; }
        public virtual DocumentType? DocumentType { get; set; }

        public int? UserFirmId { get; set; }
        public virtual UserFirm? UserFirm { get; set; }
    }
}
