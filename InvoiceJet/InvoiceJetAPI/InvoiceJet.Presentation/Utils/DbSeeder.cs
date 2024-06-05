using InvoiceJet.Domain.Models;
using InvoiceJet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InvoiceJet.Presentation.Utils;

public static class DbSeeder
{
    public static async Task SeedDocumentTypes(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<InvoiceJetDbContext>();
            context.Database.EnsureCreated();

            if (!context.DocumentType.Any())
            {
                var documentTypes = new List<DocumentType>
                {
                    new DocumentType { Name = "Factura" },
                    new DocumentType { Name = "Factura Storno" },
                    new DocumentType { Name = "Proforma" }
                };

                context.DocumentType.AddRange(documentTypes);
                await context.SaveChangesAsync();
            }

            if (!context.DocumentStatus.Any())
            {
                var documentStatuses = new List<DocumentStatus>
                {
                    new DocumentStatus { Status = "Unpaid" },
                    new DocumentStatus { Status = "Paid" },
                };

                context.DocumentStatus.AddRange(documentStatuses);
                await context.SaveChangesAsync();
            }
        }
    }

    public static async Task SeedDocumentSeries(InvoiceJetDbContext context, int userFirmId)
    {
        context.Database.EnsureCreated();
        if (!context.DocumentSeries.Any())
        {
            var documentSeries = new List<DocumentSeries>
            {
                new DocumentSeries
                {
                    SeriesName = DateTime.Now.Year.ToString(),
                    FirstNumber = 1,
                    CurrentNumber = 1,
                    IsDefault = true,
                    DocumentType = await context.DocumentType
                        .Where(d => d.Name.Equals("Factura"))
                        .FirstOrDefaultAsync(),
                    UserFirmId = userFirmId
                },
                new DocumentSeries
                {
                    SeriesName = DateTime.Now.Year.ToString(),
                    FirstNumber = 1,
                    CurrentNumber = 1,
                    IsDefault = true,
                    DocumentType = await context.DocumentType
                        .Where(d => d.Name.Equals("Factura Storno"))
                        .FirstOrDefaultAsync(),
                    UserFirmId = userFirmId
                },
                new DocumentSeries
                {
                    SeriesName = DateTime.Now.Year.ToString(),
                    FirstNumber = 1,
                    CurrentNumber = 1,
                    IsDefault = true,
                    DocumentType = await context.DocumentType
                        .Where(d => d.Name.Equals("Proforma"))
                        .FirstOrDefaultAsync(),
                    UserFirmId = userFirmId
                },
            };

            context.DocumentSeries.AddRange(documentSeries);
            await context.SaveChangesAsync();
        }
    }
}