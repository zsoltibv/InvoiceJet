using FacturilaAPI.Models.Dto;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace FacturilaAPI.Repository.Impl
{
    public class QuestPDFService : IQuestPDFService
    {
        public void GenerateDocument(DocumentRequestDTO documentRequestDTO)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string documentsPath = Path.Combine(basePath, "Documents");

            // Check if the Documents directory exists, create it if it doesn't
            if (!Directory.Exists(documentsPath))
            {
                Directory.CreateDirectory(documentsPath);
            }

            // Define the path for the new PDF file
            string pdfFilePath = Path.Combine(documentsPath, "hello.pdf");

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));

                    page.Header()
                        .Text("Hello PDF!")
                        .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(20);

                            x.Item().Text(Placeholders.LoremIpsum());
                            x.Item().Image(Placeholders.Image(200, 100));
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            }).GeneratePdf(pdfFilePath);
        }
    }
}
