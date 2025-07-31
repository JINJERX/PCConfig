using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using MauiApp3.Models;
using Microsoft.Maui.Graphics;


namespace MauiApp3.Services
{
    public class PdfService
    {
        public static async Task<string> ExportBuildToPdfAsync(PCBuild build)
        {
            string fileName = $"Сборка_{build.Name}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);
                    page.PageColor(QuestPDF.Helpers.Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(14));

                    page.Header().Text($"Сборка: {build.Name}")
                                 .FontSize(20).Bold().FontColor(QuestPDF.Helpers.Colors.Blue.Medium);


                    page.Content().Column(col =>
                    {
                        col.Item().Text($"CPU: {build.CPU}");
                        col.Item().Text($"GPU: {build.GPU}");
                        col.Item().Text($"RAM: {build.RAM}");
                        col.Item().Text($"Storage: {build.Storage}");
                        col.Item().Text($"PSU: {build.PSU}");
                        col.Item().Text($"Case: {build.Case}");
                        col.Item().Text($"Motherboard: {build.Motherboard}");
                        col.Item().Text($"Cooling: {build.Cooling}");
                    });

                    page.Footer().AlignCenter().Text($"Сгенерировано: {DateTime.Now:g}");
                });
            }).GeneratePdf(filePath);

            return filePath;
        }
    }
}
