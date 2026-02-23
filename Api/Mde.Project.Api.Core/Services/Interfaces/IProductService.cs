using Mde.Project.Api.Core.Entities.Products;
using Mde.Project.Api.Core.Services.Interfaces.Base;
using Mde.Project.Api.Core.Services.Interfaces.Products;

namespace Mde.Project.Api.Core.Services.Interfaces
{
    public interface IProductService : ICrudService<Product>
    {
        IProductBrandService Brands { get; }
        IProductCategoryService Categories { get; }
        IProductTaxService Taxes { get; }
        Task<string> GenerateBarcodeAsync();
    }
}
