using InvoiceJet.Application.DTOs;
using InvoiceJet.Infrastructure.Services.IQuestPDFDocument.Component;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace InvoiceJet.Infrastructure.Services.IQuestPDFDocument;

public class InvoiceDocument : IDocument
{
    public DocumentRequestDto Model { get; }

    public string FullDocumentNumber
    {
        get
        {
            if (Model.DocumentSeries != null)
            {
                string seriesName = Model.DocumentSeries.SeriesName;
                string formattedNumber = Model.DocumentSeries.CurrentNumber.ToString("D4");
                return seriesName + formattedNumber;
            }
            else
            {
                return Model.DocumentNumber;
            }
        }
    }

    public InvoiceDocument(DocumentRequestDto model)
    {
        Model = model;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);

                page.Footer().AlignCenter().Text(text =>
                {
                    text.CurrentPageNumber();
                    text.Span(" / ");
                    text.TotalPages();
                });
            });
    }

    private void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column
                    .Item().Text($"Invoice #{FullDocumentNumber}")
                    .FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                column.Item().Text(text =>
                {
                    text.Span("Issue date: ").SemiBold();
                    text.Span($"{Model.IssueDate:d}");
                });

                if (Model.DueDate.HasValue)
                {
                    column.Item().Text(text =>
                    {
                        text.Span("Due date: ").SemiBold();
                        text.Span($"{Model.DueDate.Value:d}");
                    });
                }
            });
            
            row.ConstantItem(100).Element(e =>
            {
                // Check if the document status is "Paid"
                if (Model.DocumentStatus?.Status == "Paid")
                {
                    e.Background(Colors.Green.Medium) // Green background for paid
                        .AlignCenter()
                        .AlignMiddle()
                        .Padding(5)
                        .Text("Paid") // Display "Paid"
                        .FontSize(14)
                        .Bold();
                }
                else
                {
                    e.Background(Colors.Red.Medium) // Red background for unpaid
                        .AlignCenter()
                        .AlignMiddle()
                        .Padding(5)
                        .Text("Unpaid") // Display "Unpaid"
                        .FontSize(14)
                        .Bold();
                }
            });
            
            // Optional: Add company logo if available
            // row.ConstantItem(175).Image(LogoImage);
        });
    }

    private void ComposeContent(IContainer container)
    {
        container.PaddingVertical(40).Column(column =>
        {
            column.Spacing(20);

            column.Item().Row(row =>
            {
                row.RelativeItem().Component(new AddressComponent("From", Model.Seller, Model.BankAccount));
                row.ConstantItem(50);
                row.RelativeItem().Component(new AddressComponent("For", Model.Client));
            });

            column.Item().Element(ComposeTable);

            var totalPrice = Model.Products.Sum(x => x.TotalPrice);
            column.Item().PaddingRight(5).AlignRight().Text($"Grand total: {totalPrice:C}").SemiBold();

            // Optional: Add comments or additional sections as needed
        });
    }

    private void ComposeTable(IContainer container)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(25);
                columns.RelativeColumn(3);
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
            });

            table.Header(header =>
            {
                header.Cell().Text("#");
                header.Cell().Text("Product").Style(TextStyle.Default.SemiBold());
                header.Cell().AlignRight().Text("Unit price").Style(TextStyle.Default.SemiBold());
                header.Cell().AlignRight().Text("Quantity").Style(TextStyle.Default.SemiBold());
                header.Cell().AlignRight().Text("Total").Style(TextStyle.Default.SemiBold());

                header.Cell().ColumnSpan(5).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Black);
            });

            int index = 0;
            foreach (var item in Model.Products)
            {
                index++;
                table.Cell().Element(cell => cell.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5))
                    .Text($"{index}");
                table.Cell().Element(cell => cell.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5))
                    .Text(item.Name);
                table.Cell().Element(cell => cell.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5))
                    .AlignRight().Text($"{item.UnitPrice:C}");
                table.Cell().Element(cell => cell.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5))
                    .AlignRight().Text($"{item.Quantity}");
                table.Cell().Element(cell => cell.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5))
                    .AlignRight().Text($"{item.TotalPrice:C}");
            }
        });
    }
}