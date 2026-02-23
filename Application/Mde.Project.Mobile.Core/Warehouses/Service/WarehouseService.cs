using System.Net.Http.Headers;
using System.Net.Http.Json;
using Mde.Project.Mobile.Core.Authentication.Services;
using Mde.Project.Mobile.Core.MauiHelpers;
using Mde.Project.Mobile.Core.Models.Warehouses;
using Mde.Project.Mobile.Core.ResultModels;
using Mde.Project.Mobile.Core.Warehouses.Interface;
using Mde.Project.Mobile.Core.Warehouses.RequestModels;

namespace Mde.Project.Mobile.Core.Warehouses.Service
{
    public class WarehouseService : AuthenticationService, IWarehouseService
    {
        private IHttpClientFactory _httpClientFactory;
        public WarehouseService(IHttpClientFactory httpClientFactory, ISecureStorageService secureStorageService) : base(httpClientFactory, secureStorageService)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<PagingResultModel<Warehouse>> TryGetWarehouseListAsync(string name, int pageNumber)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            var warehousesResultModel = new PagingResultModel<Warehouse>();

            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    warehousesResultModel.Message = "Token is empty";
                    return warehousesResultModel;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestUrl = $"/api/warehouses?pageNumber={pageNumber}&name={name}&withPagination=true";

                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    warehousesResultModel = await response.Content.ReadFromJsonAsync<PagingResultModel<Warehouse>>();
                }
                else
                {
                    warehousesResultModel.Message = "Failed to retrieve warehouse list";
                }
            }
            catch (Exception)
            {
                warehousesResultModel.Message = "Failed to retrieve warehouse list";
            }

            return warehousesResultModel;
        }
        public async Task<CollectionResultModel<Warehouse>> TryGetWarehouseOptionsAsync()
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            var warehousesResultModel = new CollectionResultModel<Warehouse>();
            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    warehousesResultModel.Message = "Token is empty";
                    return warehousesResultModel;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestUrl = $"/api/warehouses";
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    warehousesResultModel =
                        await response.Content.ReadFromJsonAsync<CollectionResultModel<Warehouse>>();
                }
                else
                {
                    warehousesResultModel.Message = "Failed to retrieve warehouse list";
                }
            }
            catch (Exception)
            {
                warehousesResultModel.Message = "Failed to retrieve warehouse list";
            }

            return warehousesResultModel;
        }

        public async Task<BaseResultModel> TryRemoveWarehouseAsync(Guid warehouseId)
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
                var requestUrl = $"/api/warehouses/{warehouseId}";

                HttpResponseMessage response = await httpClient.DeleteAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    resultModel.Message = "Failed to remove warehouse";
                }

                return resultModel;
            }
            catch (Exception)
            {
                resultModel.Message = "Failed to remove warehouse";
                return resultModel;
            }
        }

        public async Task<BaseResultModel> TryUpsertWarehouseAsync(WarehouseUpsertRequestModel warehouse)
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

                var requestUrl = "/api/warehouses";
                HttpResponseMessage response = null;
                if (warehouse.Id != null && warehouse.Id != Guid.Empty)
                {
                    requestUrl = $"{requestUrl}/{warehouse.Id}";
                    response = await httpClient.PutAsJsonAsync(
                        requestUrl, warehouse); ;
                }
                else
                {
                    response = await httpClient.PostAsJsonAsync(
                        requestUrl, warehouse);
                }

                if (!response.IsSuccessStatusCode)
                {
                    resultModel.Message = "Failed to upsert product";
                }

                return resultModel;
            }
            catch (Exception)
            {
                resultModel.Message = "Failed to upsert product";
                return resultModel;
            }
        }

        public async Task<ResultModel<Warehouse>> TryGetWarehouseByIdAsync(Guid warehouseId)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            var warehouseResultModel = new ResultModel<Warehouse>();
            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    warehouseResultModel.Message = "Token is empty";
                    return warehouseResultModel;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestUrl = $"/api/warehouses/{warehouseId}";
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    warehouseResultModel.Data =
                        await response.Content.ReadFromJsonAsync<Warehouse>();
                }
                else
                {
                    warehouseResultModel.Message = "Failed to retrieve warehouse";
                }
            }
            catch (Exception)
            {
                warehouseResultModel.Message = "Failed to retrieve warehouse";
            }

            return warehouseResultModel;
        }
    }
}
