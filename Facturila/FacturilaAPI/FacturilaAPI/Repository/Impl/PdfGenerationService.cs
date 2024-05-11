using FacturilaAPI.Models.Dto;
using QuestPDF.Fluent;

namespace FacturilaAPI.Repository.Impl
{
    public class PdfGenerationService : IPdfGenerationService
    {
        public string GenerateInvoicePdf(DocumentRequestDTO invoiceData)
        {
            try
            {
                string filePath = GetInvoicePdfPath(invoiceData.DocumentSeries.CurrentNumber);

                InvoiceDocument document = new InvoiceDocument(invoiceData);

                document.GeneratePdf(filePath);

                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error generating PDF: " + ex.Message);
                return null;
            }
        }

        public byte[] GetInvoicePdfStream(DocumentRequestDTO invoiceData)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    InvoiceDocument document = new InvoiceDocument(invoiceData);
                    document.GeneratePdf(memoryStream);

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
}
