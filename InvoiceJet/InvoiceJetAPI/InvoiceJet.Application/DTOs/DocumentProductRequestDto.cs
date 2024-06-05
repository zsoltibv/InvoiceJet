namespace InvoiceJetAPI.Models.Dto
{
    public class DocumentProductRequestDTO
    {
        public int Id {get;set;}
        public string Name { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public bool ContainsTVA { get; set; }
        public string UnitOfMeasurement { get; set; } = string.Empty;
        public decimal TVAValue { get; set; }
        public int Quantity { get; set; }
    }
}
