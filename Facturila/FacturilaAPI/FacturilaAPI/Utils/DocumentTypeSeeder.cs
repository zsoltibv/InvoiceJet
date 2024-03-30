using FacturilaAPI.Config;
using FacturilaAPI.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.Metrics;

namespace FacturilaAPI.Utils
{
    public static class DocumentTypeSeeder
    {
        public static async Task Seed(IApplicationBuilder applicationBuilder)
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
    }
}
