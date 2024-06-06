namespace InvoiceJet.Domain.Exceptions;

public class ProductAssociatedWithInvoiceException : Exception
{
    public ProductAssociatedWithInvoiceException() : base("Can't delete. Product is associated with an invoice.")
    {
    }
}