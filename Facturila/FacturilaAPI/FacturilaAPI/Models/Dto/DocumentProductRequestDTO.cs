namespace FacturilaAPI.Models.Dto
{
    public class DocumentProductRequestDTO
    {
        public int Id {get;set;}
        public string Name { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public bool ContainsTVA { get; set; }
        public string UnitOfMeasurement { get; set; } = string.Empty;
        public string TVAValue { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
