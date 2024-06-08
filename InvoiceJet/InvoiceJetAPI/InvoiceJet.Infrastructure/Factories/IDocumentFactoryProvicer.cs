using InvoiceJet.Domain.Enums;
using InvoiceJet.Infrastructure.Factories.Impl;

namespace InvoiceJet.Infrastructure.Factories;

public class DocumentFactoryProvider
{
    public IDocumentFactory GetDocumentFactory(int documentTypeId)
    {
        return documentTypeId switch
        {
            (int)DocumentTypeEnum.Invoice => new InvoiceDocumentFactory(),
            (int)DocumentTypeEnum.ProformaInvoice => new ProformaDocumentFactory(),
            _ => throw new Exception("Invalid document type")
        };
    }
}