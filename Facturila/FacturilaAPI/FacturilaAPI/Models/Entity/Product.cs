using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacturilaAPI.Models.Entity
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public bool ContainsTVA { get; set; }
        public string? UnitOfMeasurement { get; set;}
        public int TVAValue { get; set; }

        public int UserFirmId { get; set; }
        public virtual UserFirm? UserFirm { get; set; }
    }
}
