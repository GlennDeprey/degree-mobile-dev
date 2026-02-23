using Mde.Project.Mobile.Core.Models.Products;
using Mde.Project.Mobile.Core.ResultModels;
using Mde.Project.Mobile.Core.Products.RequestModels;

namespace Mde.Project.Mobile.Core.Products.Service.Interfaces;

public interface IProductService
{
    Task<PagingResultModel<Product>> TryGetProductListAsync(string name, int pageNumber, Guid? brandId = null, Guid? categoryId = null);
    Task<PagingResultModel<Category>> TryGetCategoryListAsync(int pageNumber, string name);
    Task<PagingResultModel<Brand>> TryGetBrandListAsync(int pageNumber, string name);
    Task<ResultModel<Product>> TryGetProductByIdAsync(Guid productId);
    Task<ResultModel<Brand>> TryGetBrandByIdAsync(Guid brandId);
    Task<ResultModel<Category>> TryGetCategoryByIdAsync(Guid categoryId);
    Task<BaseResultModel> TryUpsertProductAsync(ProductUpsertRequestModel product, Stream imageStream = null, string fileName = null);
    Task<BaseResultModel> TryUpsertBrandAsync(BrandUpsertRequestModel brand);
    Task<BaseResultModel> TryUpsertCategoryAsync(CategoryUpsertRequestModel category);
    Task<BaseResultModel> TryRemoveProductAsync(Guid productId);
    Task<BaseResultModel> TryRemoveCategoryAsync(Guid categoryId);
    Task<BaseResultModel> TryRemoveBrandAsync(Guid brandId);
    Task<CollectionResultModel<Category>> TryGetCategoriesOptionsAsync();
    Task<CollectionResultModel<Brand>> TryGetBrandsOptionsAsync();
    Task<CollectionResultModel<TaxRate>> TryGetTaxRateOptionsAsync();
    Task<ResultModel<byte[]>> TryGetProductPdf(Guid productId);
}

