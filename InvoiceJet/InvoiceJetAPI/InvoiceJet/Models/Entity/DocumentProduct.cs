﻿using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceJetAPI.Models.Entity;

public class DocumentProduct
{
    public int Id { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }

    public int? DocumentId { get; set; }
    public virtual Document? Document { get; set; }

    public int? ProductId { get; set; }
    public virtual Product? Product { get; set; }
}