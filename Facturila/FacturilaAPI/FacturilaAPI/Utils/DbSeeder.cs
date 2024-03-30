using FacturilaAPI.Config;
using FacturilaAPI.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.Metrics;

namespace FacturilaAPI.Utils
{
    public static class DbSeeder
    {
        public static async Task SeedDocumentTypes(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<FacturilaDbContext>();
                context.Database.EnsureCreated();

                if (!context.DocumentType.Any())
                {
                    var documentTypes = new List<DocumentType> {
                        new DocumentType { Name = "Factura" },
                        new DocumentType { Name = "Factura Storno" },
                        new DocumentType { Name = "Proforma" }
                    };

                    context.DocumentType.AddRange(documentTypes);
                    await context.SaveChangesAsync();
                }
            }
        }

        public static async Task SeedDocumentSeries(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<FacturilaDbContext>();
                context.Database.EnsureCreated();

                if (!context.DocumentSeries.Any())
                {
                    var documentSeries = new List<DocumentSeries> {
                        new DocumentSeries
                        {
                            SeriesName = DateTime.Now.Year.ToString(),
                            FirstNumber = 1,
                            CurrentNumber = 1,
                            IsDefault = true,
                            DocumentType = await context.DocumentType
                                .Where(d => d.Name.Equals("Factura"))
                                .FirstOrDefaultAsync()
                        },
                        new DocumentSeries
                        {
                            SeriesName = DateTime.Now.Year.ToString(),
                            FirstNumber = 1,
                            CurrentNumber = 1,
                            IsDefault = true,
                            DocumentType = await context.DocumentType
                                .Where(d => d.Name.Equals("Factura Storno"))
                                .FirstOrDefaultAsync()
                        },
                        new DocumentSeries
                        {
                            SeriesName = DateTime.Now.Year.ToString(),
                            FirstNumber = 1,
                            CurrentNumber = 1,
                            IsDefault = true,
                            DocumentType = await context.DocumentType
                                .Where(d => d.Name.Equals("Proforma"))
                                .FirstOrDefaultAsync()
                        },
                    };

                    context.DocumentSeries.AddRange(documentSeries);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
