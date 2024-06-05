using InvoiceJet.Application.DTOs;
using InvoiceJet.Application.Services;
using InvoiceJet.Infrastructure.Services.IQuestPDFDocument;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace InvoiceJet.Infrastructure.Services;

public class PdfGenerationService : IPdfGenerationService
{
    public string GenerateInvoicePdf(DocumentRequestDto invoiceData)
    {
        try
        {
            string filePath = GetInvoicePdfPath(invoiceData.DocumentSeries.CurrentNumber);

            IDocument document = new InvoiceDocument(invoiceData);

            document.GeneratePdf(filePath);

            return filePath;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error generating PDF: " + ex.Message);
            return null;
        }
    }

    public byte[] GetInvoicePdfStream(DocumentRequestDto invoiceData)
    {
        try
        {
            using (var memoryStream = new MemoryStream())
            {
                switch (invoiceData.DocumentType!.Id)
                {
                    case 1:
                        if (invoiceData.DocumentStatus!.Id == 3)
                        {
                            IDocument stornoDocument = new InvoiceStorno(invoiceData);
                            stornoDocument.GeneratePdf(memoryStream);
                        }
                        else
                        {
                            IDocument document = new InvoiceDocument(invoiceData);
                            document.GeneratePdf(memoryStream);
                        }

                        break;
                    case 2:
                        IDocument proformaDocument = new InvoiceProforma(invoiceData);
                        proformaDocument.GeneratePdf(memoryStream);
                        break;
                    default:
                        throw new Exception("Invalid document type");
                }

                return memoryStream.ToArray();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error generating PDF: " + ex.Message);
            return null;
        }
    }

    private string GetInvoicePdfPath(int invoiceNumber)
    {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string documentsPath = Path.Combine(basePath, "Documents");
        if (!Directory.Exists(documentsPath))
        {
            Directory.CreateDirectory(documentsPath);
        }

        return Path.Combine(documentsPath, $"Invoice_{invoiceNumber}.pdf");
    }
}