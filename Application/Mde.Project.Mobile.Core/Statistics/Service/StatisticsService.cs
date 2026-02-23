using Mde.Project.Mobile.Core.Authentication.Services;
using Mde.Project.Mobile.Core.MauiHelpers;
using Mde.Project.Mobile.Core.Models.Statistics;
using Mde.Project.Mobile.Core.ResultModels;
using Mde.Project.Mobile.Core.Statistics.Interface;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Mde.Project.Mobile.Core.Statistics.Service
{
    public class StatisticsService : AuthenticationService, IStatisticsService
    {
        private IHttpClientFactory _httpClientFactory;
        public StatisticsService(IHttpClientFactory httpClientFactory, ISecureStorageService secureStorageService) : base(httpClientFactory, secureStorageService)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CollectionResultModel<WarehouseStats>> TryGetWarehouseStatsAsync(Guid warehouseId)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            var warehouseResultModel = new CollectionResultModel<WarehouseStats>();
            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    warehouseResultModel.Message = "Token is empty";
                    return warehouseResultModel;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestUrl = $"/api/statistics?warehouseId={warehouseId}";
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    warehouseResultModel.Message = "Failed to retrieve warehouse statistics";
                    return warehouseResultModel;
                }
                
                warehouseResultModel = await response.Content.ReadFromJsonAsync<CollectionResultModel<WarehouseStats>>();

            }
            catch (Exception)
            {
                warehouseResultModel.Message = "Failed to retrieve warehouse statistics";
            }

            return warehouseResultModel;
        }
    }
}
