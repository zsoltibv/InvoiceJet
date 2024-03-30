using System.ComponentModel.DataAnnotations.Schema;

namespace FacturilaAPI.Models.Dto
{
    public class ProductDto
    {
            public int Id { get; set; }
            public string Name { get; set; }
            [Column(TypeName = "decimal(18,2)")]
            public decimal Price { get; set; }
            public bool ContainsTVA { get; set; }
            public string? UnitOfMeasurement { get; set; }
            public int TVAValue { get; set; }
    }
}
