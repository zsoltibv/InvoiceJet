using FacturilaAPI.Models.Dto;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace FacturilaAPI.Repository.Impl
{
    public class InvoiceDocument : IDocument
    {
        public DocumentRequestDTO Model { get; }

        public InvoiceDocument(DocumentRequestDTO model)
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
                        .Item().Text($"Invoice #{Model.DocumentSeries.CurrentNumber}")
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
                    row.RelativeItem().Component(new AddressComponent("From", Model.Seller));
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
                    table.Cell().Element(cell => cell.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5)).Text($"{index}");
                    table.Cell().Element(cell => cell.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5)).Text(item.Name);
                    table.Cell().Element(cell => cell.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5)).AlignRight().Text($"{item.UnitPrice:C}");
                    table.Cell().Element(cell => cell.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5)).AlignRight().Text($"{item.Quantity}");
                    table.Cell().Element(cell => cell.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5)).AlignRight().Text($"{item.TotalPrice:C}");
                }
            });
        }
    }

    public class AddressComponent : IComponent
    {
        private string Title { get; }
        private FirmDto Address { get; }

        public AddressComponent(string title, FirmDto address)
        {
            Title = title;
            Address = address;
        }

        public void Compose(IContainer container)
        {
            container.ShowEntire().Column(column =>
            {
                column.Spacing(2);

                column.Item().Text(Title).SemiBold();
                column.Item().PaddingBottom(5).LineHorizontal(1);

                column.Item().Text(Address.Name);
                column.Item().Text(Address.Address);
                column.Item().Text($"{Address.City}, {Address.County}");
                if (!string.IsNullOrEmpty(Address.CUI))
                {
                    column.Item().Text($"CUI: {Address.CUI}");
                }
                if (!string.IsNullOrEmpty(Address.RegCom))
                {
                    column.Item().Text($"Reg. Com: {Address.RegCom}");
                }
                // Optionally add more details like Email and Phone if available and needed
            });
        }
    }
}

