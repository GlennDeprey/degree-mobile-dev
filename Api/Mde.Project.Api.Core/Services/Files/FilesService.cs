using Mde.Project.Api.Core.Entities.Products;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Document = QuestPDF.Fluent.Document;

namespace Mde.Project.Api.Core.Services.Files
{
    public class FilesService : IFilesService
    {
        public async Task SaveChunkAsync(Stream chunkStream, string fileName)
        {
            var tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
            using (var fileStream = new FileStream(tempFilePath, FileMode.Append, FileAccess.Write))
            {
                await chunkStream.CopyToAsync(fileStream);
            }
        }
        public Task<string> FinalizeUploadAsync(string fileName)
        {
            var tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
            var wwwroot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadsDir = Path.Combine(wwwroot, "Uploads");
            Directory.CreateDirectory(uploadsDir);

            var extension = Path.GetExtension(fileName);
            var guidFileName = $"{Guid.NewGuid()}{extension}";
            var finalFilePath = Path.Combine(uploadsDir, guidFileName);

            if (File.Exists(finalFilePath))
            {
                File.Delete(finalFilePath);
            }
            File.Move(tempFilePath, finalFilePath);

            return Task.FromResult(finalFilePath);
        }

        public async Task<string> SaveFileAsync(Stream fileStream, string fileName)
        {
            var wwwroot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadsDir = Path.Combine(wwwroot, "Uploads");
            Directory.CreateDirectory(uploadsDir);

            var extension = Path.GetExtension(fileName);
            var guidFileName = $"{Guid.NewGuid()}{extension}";
            var finalFilePath = Path.Combine(uploadsDir, guidFileName);

            using (var output = new FileStream(finalFilePath, FileMode.Create, FileAccess.Write))
            {
                await fileStream.CopyToAsync(output);
            }

            return finalFilePath;
        }
        public Task RemoveFileIfExistsAsync(string fileName)
        {
            var wwwroot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadsDir = Path.Combine(wwwroot, "Uploads");
            var filePath = Path.Combine(uploadsDir, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return Task.CompletedTask;
        }
        public bool IsValidImageFile(string fileName, string contentType)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var allowedMimeTypes = new[] { "image/jpeg", "image/png" };
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return allowedExtensions.Contains(extension) && allowedMimeTypes.Contains(contentType);
        }

        public async Task<byte[]> GenerateProductPdfAsync(Product product)
        {
            if (product == null)
                return Array.Empty<byte>();

            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10)); // Default font size for consistency

                    page.Content().AlignCenter().AlignMiddle().Element(card =>
                    {
                        card
                            .Padding(15)
                            .Width(180)
                            .Border(1)
                            .BorderColor(Colors.Black)
                            .Background(Colors.White)
                            .Column(col =>
                            {
                                // Product Name - Centered and Bold
                                col.Item().PaddingBottom(5).PaddingTop(5).AlignCenter().Text(product.Name ?? "Product")
                                    .FontSize(14)
                                    .Bold()
                                    .FontColor(Colors.Black);

                                // Price - Right Aligned, Green
                                col.Item().AlignRight().PaddingRight(5).Text(
                                    $"{(product.SalesPrice * (decimal)product.SalesTax.TaxRate) + product.SalesPrice:C}")
                                    .FontSize(12)
                                    .Bold()
                                    .FontColor(Colors.Green.Darken2);

                                // Optional horizontal separator
                                col.Item().PaddingVertical(4).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                                // Barcode (Centered)
                                col.Item().PaddingTop(10).PaddingBottom(10).AlignCenter().Text(product.Barcode ?? "")
                                    .FontFamily("Barcode")
                                    .FontSize(72);
                            });
                    });
                });
            }).GeneratePdf();

            return await Task.FromResult(pdfBytes);
        }

        public async Task<byte[]> GenerateWarehouseProductsPdfAsync(string warehouseName, IEnumerable<Product> products)
        {
            if (products == null || !products.Any())
                return Array.Empty<byte>();

            const int cardsPerRow = 2;
            const int cardWidth = 180;
            const int cardSpacing = 20;
            const int rowsPerPage = 4;
            var cardsPerPage = cardsPerRow * rowsPerPage;

            var productList = products.ToList();
            var totalCards = productList.Count;

            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header()
                        .AlignCenter()
                        .Text(warehouseName ?? string.Empty)
                        .FontSize(18)
                        .Bold()
                        .FontColor(Colors.Black);

                    page.Content().PaddingTop(10).AlignCenter().Column(mainCol =>
                    {
                        int pageIndex = 0;
                        while (pageIndex * cardsPerPage < totalCards)
                        {
                            var pageProducts = productList
                                .Skip(pageIndex * cardsPerPage)
                                .Take(cardsPerPage)
                                .ToList();

                            for (int i = 0; i < pageProducts.Count; i += cardsPerRow)
                            {
                                var rowProducts = pageProducts.Skip(i).Take(cardsPerRow).ToList();

                                mainCol.Item().Row(row =>
                                {
                                    for (int j = 0; j < rowProducts.Count; j++)
                                    {
                                        var product = rowProducts[j];
                                        var isLast = j == rowProducts.Count - 1;
                                        row.RelativeItem().PaddingRight(isLast ? 0 : cardSpacing).Element(card =>
                                        {
                                            card
                                                .Padding(15)
                                                .Width(cardWidth)
                                                .Border(1)
                                                .BorderColor(Colors.Black)
                                                .Background(Colors.White)
                                                .Column(col =>
                                                {
                                                    col.Item().PaddingBottom(5).PaddingTop(5).AlignCenter().Text(product.Name ?? "Product")
                                                        .FontSize(14)
                                                        .Bold()
                                                        .FontColor(Colors.Black);

                                                    col.Item().AlignRight().PaddingRight(5).Text(
                                                        $"{(product.SalesPrice * (decimal)product.SalesTax.TaxRate) + product.SalesPrice:C}")
                                                        .FontSize(12)
                                                        .Bold()
                                                        .FontColor(Colors.Green.Darken2);

                                                    col.Item().PaddingVertical(4).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                                                    col.Item().PaddingTop(10).PaddingBottom(10).AlignCenter().Text(product.Barcode ?? "")
                                                        .FontFamily("Barcode")
                                                        .FontSize(72);
                                                });
                                        });
                                    }
                                });

                                if (i + cardsPerRow < pageProducts.Count)
                                {
                                    mainCol.Item().Height(cardSpacing);
                                }
                            }

                            pageIndex++;
                            if (pageIndex * cardsPerPage < totalCards)
                            {
                                mainCol.Item().PageBreak();
                            }
                        }
                    });
                });
            }).GeneratePdf();

            return await Task.FromResult(pdfBytes);
        }
    }
}
