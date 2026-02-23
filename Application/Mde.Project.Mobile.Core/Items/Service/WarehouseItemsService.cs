using Mde.Project.Mobile.Core.Authentication.Services;
using Mde.Project.Mobile.Core.Items.Interfaces;
using Mde.Project.Mobile.Core.Items.RequestModels;
using Mde.Project.Mobile.Core.MauiHelpers;
using Mde.Project.Mobile.Core.Models.Warehouses;
using Mde.Project.Mobile.Core.ResultModels;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Mde.Project.Mobile.Core.Items.Service
{
    public class WarehouseItemsService : AuthenticationService, IWarehouseItemsService
    {
        private IHttpClientFactory _httpClientFactory;
        public WarehouseItemsService(IHttpClientFactory httpClientFactory, ISecureStorageService secureStorageService) : base(httpClientFactory, secureStorageService)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<PagingResultModel<WarehouseItem>> TryGetItemsAsync(string name, Guid warehouseId,
            int pageNumber, Guid? brandId = null, Guid? categoryId = null)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            var warehouseProductsResultModel = new PagingResultModel<WarehouseItem>();
            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    warehouseProductsResultModel.Message = "Token is empty";
                    return warehouseProductsResultModel;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestUrl = $"/api/items/{warehouseId}/products?pageNumber={pageNumber}&name={name}&brandId={brandId}&categoryId={categoryId}";
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    warehouseProductsResultModel =
                        await response.Content.ReadFromJsonAsync<PagingResultModel<WarehouseItem>>();
                }
                else
                {
                    warehouseProductsResultModel.Message = "Failed to retrieve warehouse items";
                }
            }
            catch (Exception)
            {
                warehouseProductsResultModel.Message = "Failed to contact the server";
            }

            return warehouseProductsResultModel;
        }

        public async Task<ResultModel<WarehouseItem>> TryGetItemByIdAsync(Guid warehouseId, Guid productId)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            var warehouseProductsResultModel = new ResultModel<WarehouseItem>();
            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    warehouseProductsResultModel.Message = "Token is empty";
                    return warehouseProductsResultModel;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestUrl = $"/api/items/{warehouseId}/products/{productId}";
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    warehouseProductsResultModel.Data =
                        await response.Content.ReadFromJsonAsync<WarehouseItem>();
                }
                else
                {
                    warehouseProductsResultModel.Message = "Failed to retrieve warehouse item";
                }
            }
            catch (Exception)
            {
                warehouseProductsResultModel.Message = "Failed to contact the server";
            }

            return warehouseProductsResultModel;
        }

        public async Task<ResultModel<WarehouseItem>> TryGetItemByBarcodeAsync(Guid warehouseId, string barcode, int? currentCount = null)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            var productScanResultModel = new ResultModel<WarehouseItem>();
            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    productScanResultModel.Message = "Token is empty";
                    return productScanResultModel;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestUrl = $"/api/items/{warehouseId}/products/{barcode}";

                if (currentCount.HasValue)
                {
                    requestUrl += $"?currentCount={currentCount.Value}";
                }

                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    productScanResultModel.Data =
                        await response.Content.ReadFromJsonAsync<WarehouseItem>();
                }
                else
                {
                    productScanResultModel.Message = "Warehouse does not contain the scanned product.";
                }
            }
            catch (Exception)
            {
                productScanResultModel.Message = "Failed to contact the server";
            }

            return productScanResultModel;
        }

        public async Task<ResultModel<IEnumerable<WarehouseItemStock>>> TryGetWarehousesWithProductStockAsync(Guid productId, Guid? excludeWarehouseId, int minQuantity = 1)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            var productScanResultModel = new ResultModel<IEnumerable<WarehouseItemStock>>();
            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    productScanResultModel.Message = "Token is empty";
                    return productScanResultModel;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestUrl = $"/api/items/{productId}/warehouses";

                var queryParameters = new List<string>();

                if (excludeWarehouseId.HasValue)
                {
                    queryParameters.Add($"excludeWarehouseId={excludeWarehouseId.Value}");
                }

                if (minQuantity > 1)
                {
                    queryParameters.Add($"minQuantity={minQuantity}");
                }

                if (queryParameters.Any())
                {
                    requestUrl += "?" + string.Join("&", queryParameters);
                }

                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    productScanResultModel.Data =
                        await response.Content.ReadFromJsonAsync<IEnumerable<WarehouseItemStock>>();
                }
                else
                {
                    productScanResultModel.Message = "Failed to receive warehouse information.";
                }
            }
            catch (Exception)
            {
                productScanResultModel.Message = "Failed to contact the server";
            }

            return productScanResultModel;
        }

        public async Task<BaseResultModel> TryAddItemAsync(Guid warehouseId, CreateItemRequestModel createItemRequest)
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
                var requestUrl = $"/api/items/{warehouseId}/products";
                HttpResponseMessage response = await httpClient.PostAsJsonAsync(requestUrl, createItemRequest);
                if (!response.IsSuccessStatusCode)
                {
                    resultModel.Message = "Failed to add item";
                }
            }
            catch (Exception)
            {
                resultModel.Message = "Failed to contact the server";
            }

            return resultModel;
        }

        public async Task<BaseResultModel> TryUpdateItemAsync(Guid warehouseId, Guid productId, UpdateItemRequestModel updateItemRequest)
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
                var requestUrl = $"/api/items/{warehouseId}/products/{productId}";
                HttpResponseMessage response = await httpClient.PutAsJsonAsync(requestUrl, updateItemRequest);
                if (!response.IsSuccessStatusCode)
                {
                    resultModel.Message = "Failed to update item";
                }
            }
            catch (Exception)
            {
                resultModel.Message = "Failed to contact the server";
            }
            return resultModel;
        }
        public async Task<BaseResultModel> TryDeleteItemAsync(Guid warehouseId, Guid productId)
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
                var requestUrl = $"/api/items/{warehouseId}/products/{productId}";
                HttpResponseMessage response = await httpClient.DeleteAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    resultModel.Message = "Failed to delete item";
                }
            }
            catch (Exception)
            {
                resultModel.Message = "Failed to contact the server";
            }
            return resultModel;
        }

        public async Task<ResultModel<byte[]>> TryGetWarehouseProductsPdf(Guid warehouseId)
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
                var requestUrl = $"/api/items/{warehouseId}/pdf";
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
                if (response.IsSuccessStatusCode)
                {
                    resultModel.Data = await response.Content.ReadAsByteArrayAsync();
                }
                else
                {
                    resultModel.Message = await response.Content.ReadAsStringAsync();
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
