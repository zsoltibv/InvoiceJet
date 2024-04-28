using System.ComponentModel.DataAnnotations.Schema;

namespace FacturilaAPI.Models.Entity;

public class InvoiceProduct
{
    public int Id { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    public int InvoiceId { get; set; }
    public virtual Invoice? Invoice { get; set; }

    public int ProductId { get; set; }
    public virtual Product? Product { get; set; }
}