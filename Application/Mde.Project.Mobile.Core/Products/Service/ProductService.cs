using System.Net.Http.Headers;
using System.Net.Http.Json;
using Mde.Project.Mobile.Core.Authentication.Services;
using Mde.Project.Mobile.Core.MauiHelpers;
using Mde.Project.Mobile.Core.Models.Products;
using Mde.Project.Mobile.Core.Products.RequestModels;
using Mde.Project.Mobile.Core.Products.Service.Interfaces;
using Mde.Project.Mobile.Core.ResultModels;

namespace Mde.Project.Mobile.Core.Products.Service
{
    public class ProductService : AuthenticationService, IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory, ISecureStorageService secureStorageService) : base(httpClientFactory, secureStorageService)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<PagingResultModel<Product>> TryGetProductListAsync(string name, int pageNumber, Guid? brandId = null, Guid? categoryId = null)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            var productsResultModel = new PagingResultModel<Product>();

            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    productsResultModel.Message = "Token is empty";
                    return productsResultModel;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var queryParameters = new List<string> { $"pageNumber={pageNumber}" };
                if (brandId.HasValue)
                {
                    queryParameters.Add($"brandId={brandId}");
                }

                if (categoryId.HasValue)
                {
                    queryParameters.Add($"categoryId={categoryId}");
                }

                if (!string.IsNullOrWhiteSpace(name))
                {
                    queryParameters.Add($"name={name}");
                }

                var queryString = string.Join("&", queryParameters);
                var requestUrl = $"/api/products?{queryString}";

                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    productsResultModel = await response.Content.ReadFromJsonAsync<PagingResultModel<Product>>();
                }
                else
                {
                    productsResultModel.Message = "Failed to retrieve product list";
                }
            }
            catch (Exception)
            {
                productsResultModel.Message = "Failed to retrieve product list";
            }

            return productsResultModel;
        }

        public async Task<PagingResultModel<Category>> TryGetCategoryListAsync(int pageNumber, string name)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            var categoriesResultModel = new PagingResultModel<Category>();
            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    categoriesResultModel.Message = "Token is empty";
                    return categoriesResultModel;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestUrl =
                    $"/api/categories?name={name}&withProductCount=true&withPagination=true&pageNumber{pageNumber}";
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    categoriesResultModel = await response.Content.ReadFromJsonAsync<PagingResultModel<Category>>();
                }
                else
                {
                    categoriesResultModel.Message = "Failed to retrieve category list";
                }
            }
            catch (Exception)
            {
                categoriesResultModel.Message = "Failed to retrieve category list";
            }

            return categoriesResultModel;
        }

        public async Task<PagingResultModel<Brand>> TryGetBrandListAsync(int pageNumber, string name)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            var brandsResultModel = new PagingResultModel<Brand>();
            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    brandsResultModel.Message = "Token is empty";
                    return brandsResultModel;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestUrl =
                    $"/api/brands?name={name}&withProductCount=true&withPagination=true&pageNumber{pageNumber}";
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    brandsResultModel = await response.Content.ReadFromJsonAsync<PagingResultModel<Brand>>();
                }
                else
                {
                    brandsResultModel.Message = "Failed to retrieve brand list";
                }
            }
            catch (Exception)
            {
                brandsResultModel.Message = "Failed to retrieve brand list";
            }

            return brandsResultModel;
        }

        public async Task<CollectionResultModel<Category>> TryGetCategoriesOptionsAsync()
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            var categoriesResultModel = new CollectionResultModel<Category>();
            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    categoriesResultModel.Message = "Token is empty";
                    return categoriesResultModel;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestUrl = $"/api/categories";
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    categoriesResultModel =
                        await response.Content.ReadFromJsonAsync<CollectionResultModel<Category>>();
                }
                else
                {
                    categoriesResultModel.Message = "Failed to retrieve category list";
                }
            }
            catch (Exception)
            {
                categoriesResultModel.Message = "Failed to retrieve category list";
            }

            return categoriesResultModel;
        }

        public async Task<CollectionResultModel<Brand>> TryGetBrandsOptionsAsync()
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            var brandsResultModel = new CollectionResultModel<Brand>();
            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    brandsResultModel.Message = "Token is empty";
                    return brandsResultModel;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestUrl = $"/api/brands";
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    brandsResultModel = await response.Content.ReadFromJsonAsync<CollectionResultModel<Brand>>();
                }
                else
                {
                    brandsResultModel.Message = "Failed to retrieve brand list";
                }
            }
            catch (Exception)
            {
                brandsResultModel.Message = "Failed to retrieve brand list";
            }

            return brandsResultModel;
        }

        public async Task<CollectionResultModel<TaxRate>> TryGetTaxRateOptionsAsync()
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            var taxRateResultModel = new CollectionResultModel<TaxRate>();
            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    taxRateResultModel.Message = "Token is empty";
                    return taxRateResultModel;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestUrl = $"/api/taxes";
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    taxRateResultModel = await response.Content.ReadFromJsonAsync<CollectionResultModel<TaxRate>>();
                }
                else
                {
                    taxRateResultModel.Message = "Failed to retrieve Tax Rates list";
                }
            }
            catch (Exception)
            {
                taxRateResultModel.Message = "Failed to retrieve Tax Rates list";
            }

            return taxRateResultModel;
        }

        public async Task<ResultModel<Product>> TryGetProductByIdAsync(Guid productId)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            var productResultModel = new ResultModel<Product>();
            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    productResultModel.Message = "Token is empty";
                    return productResultModel;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestUrl = $"/api/products/{productId}";
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    productResultModel.Data =
                        await response.Content.ReadFromJsonAsync<Product>();
                }
                else
                {
                    productResultModel.Message = "Failed to retrieve product";
                }
            }
            catch (Exception)
            {
                productResultModel.Message = "Failed to retrieve product";
            }

            return productResultModel;
        }

        public async Task<ResultModel<Brand>> TryGetBrandByIdAsync(Guid brandId)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            var brandResultModel = new ResultModel<Brand>();
            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    brandResultModel.Message = "Token is empty";
                    return brandResultModel;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestUrl = $"/api/brands/{brandId}";
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    brandResultModel.Data =
                        await response.Content.ReadFromJsonAsync<Brand>();
                }
                else
                {
                    brandResultModel.Message = "Failed to retrieve brand";
                }
            }
            catch (Exception)
            {
                brandResultModel.Message = "Failed to retrieve brand";
            }

            return brandResultModel;
        }

        public async Task<ResultModel<Category>> TryGetCategoryByIdAsync(Guid categoryId)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            var categoryResultModel = new ResultModel<Category>();
            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    categoryResultModel.Message = "Token is empty";
                    return categoryResultModel;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestUrl = $"/api/categories/{categoryId}";
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    categoryResultModel.Data =
                        await response.Content.ReadFromJsonAsync<Category>();
                }
                else
                {
                    categoryResultModel.Message = "Failed to retrieve category";
                }
            }
            catch (Exception)
            {
                categoryResultModel.Message = "Failed to retrieve category";
            }

            return categoryResultModel;
        }

        public async Task<BaseResultModel> TryUpsertProductAsync(ProductUpsertRequestModel product, Stream imageStream = null, string fileName = null)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            var resultModel = new BaseResultModel();
            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    resultModel.Message = "Token is empty";
                    return resultModel;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var requestUrl = "/api/products";
                HttpResponseMessage response = null;

                using (var form = new MultipartFormDataContent())
                {
                    form.Add(new StringContent(product.Name), "Name");
                    form.Add(new StringContent(product.BrandId.ToString()), "BrandId");
                    form.Add(new StringContent(product.Description), "Description");
                    form.Add(new StringContent(product.SalesPrice.ToString()), "Price");
                    form.Add(new StringContent(product.SalesTaxId.ToString()), "SalesTaxId");
                    form.Add(new StringContent(product.CategoryId.ToString()), "CategoryId");

                    if (imageStream != null)
                    {
                        var fileContent = new StreamContent(imageStream);
                        string contentType = "application/octet-stream";
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            var extension = Path.GetExtension(fileName).ToLowerInvariant();
                            switch (extension)
                            {
                                case ".jpg":
                                case ".jpeg":
                                    contentType = "image/jpeg";
                                    break;
                                case ".png":
                                    contentType = "image/png";
                                    break;
                            }
                        }

                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
                        form.Add(fileContent, "ImageFile", fileName);
                    }

                    if (product.Id.HasValue && product.Id != Guid.Empty)
                    {
                        requestUrl = $"{requestUrl}/{product.Id}";
                        response = await httpClient.PutAsync(requestUrl, form);
                    }
                    else
                    {
                        response = await httpClient.PostAsync(requestUrl, form);
                    }

                    if (!response.IsSuccessStatusCode)
                    {
                        resultModel.Message = response.Content.ToString();
                    }
                }
            }
            catch (Exception)
            {
                resultModel.Message = "Failed to upsert product";
            }

            return resultModel;
        }

        public async Task<BaseResultModel> TryUpsertBrandAsync(BrandUpsertRequestModel brand)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            var resultModel = new BaseResultModel();
            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    resultModel.Message = "Token is empty";
                    return resultModel;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var requestUrl = "/api/brands";
                HttpResponseMessage response = null;
                if (brand.Id.HasValue && brand.Id != Guid.Empty)
                {
                    requestUrl = $"{requestUrl}/{brand.Id}";
                    response = await httpClient.PutAsJsonAsync(
                        requestUrl, brand); ;
                }
                else
                {
                    response = await httpClient.PostAsJsonAsync(
                        requestUrl, brand);
                }

                if (!response.IsSuccessStatusCode)
                {
                    resultModel.Message = "Failed to upsert brand";
                }

                return resultModel;
            }
            catch (Exception)
            {
                resultModel.Message = "Failed to upsert brand";
                return resultModel;
            }
        }

        public async Task<BaseResultModel> TryUpsertCategoryAsync(CategoryUpsertRequestModel category)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            var resultModel = new BaseResultModel();
            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    resultModel.Message = "Token is empty";
                    return resultModel;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var requestUrl = "/api/categories";
                HttpResponseMessage response = null;
                if (category.Id.HasValue && category.Id != Guid.Empty)
                {
                    requestUrl = $"{requestUrl}/{category.Id}";
                    response = await httpClient.PutAsJsonAsync(
                        requestUrl, category); ;
                }
                else
                {
                    response = await httpClient.PostAsJsonAsync(
                        requestUrl, category);
                }

                if (!response.IsSuccessStatusCode)
                {
                    resultModel.Message = "Failed to upsert category";
                }

                return resultModel;
            }
            catch (Exception)
            {
                resultModel.Message = "Failed to upsert category";
                return resultModel;
            }
        }

        public async Task<BaseResultModel> TryRemoveProductAsync(Guid productId)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            var resultModel = new BaseResultModel();
            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    resultModel.Message = "Token is empty";
                    return resultModel;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestUrl = $"/api/products/{productId}";

                HttpResponseMessage response = await httpClient.DeleteAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    resultModel.Message = "Failed to remove product";
                }

                return resultModel;
            }
            catch (Exception)
            {
                resultModel.Message = "Failed to remove product";
                return resultModel;
            }
        }
        public async Task<BaseResultModel> TryRemoveBrandAsync(Guid brandId)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            var resultModel = new BaseResultModel();
            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    resultModel.Message = "Token is empty";
                    return resultModel;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestUrl = $"/api/brands/{brandId}";

                HttpResponseMessage response = await httpClient.DeleteAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    resultModel.Message = "Failed to remove brand";
                }

                return resultModel;
            }
            catch (Exception)
            {
                resultModel.Message = "Failed to remove brand";
                return resultModel;
            }
        }

        public async Task<BaseResultModel> TryRemoveCategoryAsync(Guid categoryId)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            var resultModel = new BaseResultModel();
            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    resultModel.Message = "Token is empty";
                    return resultModel;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestUrl = $"/api/categories/{categoryId}";

                HttpResponseMessage response = await httpClient.DeleteAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    resultModel.Message = "Failed to remove category";
                }

                return resultModel;
            }
            catch (Exception)
            {
                resultModel.Message = "Failed to remove category";
                return resultModel;
            }
        }

        public async Task<ResultModel<byte[]>> TryGetProductPdf(Guid productId)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            var resultModel = new ResultModel<byte[]>();
            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    resultModel.Message = "Token is empty";
                    return resultModel;
                }
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestUrl = $"/api/products/{productId}/pdf";
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
                if (response.IsSuccessStatusCode)
                {
                    resultModel.Data = await response.Content.ReadAsByteArrayAsync();
                }
                else
                {
                    resultModel.Message = "Failed to retrieve product PDF";
                }
            }
            catch (Exception)
            {
                resultModel.Message = "Failed to retrieve product PDF";
            }
            return resultModel;
        }
    }
}
