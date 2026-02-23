using Mde.Project.Api.Core.Entities.Products;

namespace Mde.Project.Api.Core.Services.Files
{
    public interface IFilesService
    {
        Task SaveChunkAsync(Stream chunkStream, string fileName);
        Task<string> FinalizeUploadAsync(string fileName);
        Task<string> SaveFileAsync(Stream fileStream, string fileName);
        Task RemoveFileIfExistsAsync(string fileName);
        bool IsValidImageFile(string fileName, string contentType);
        Task<byte[]> GenerateProductPdfAsync(Product product);
        Task<byte[]> GenerateWarehouseProductsPdfAsync(string warehouseName, IEnumerable<Product> products);
    }
}
