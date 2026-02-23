using Mde.Project.Mobile.Core.Authentication.Services;
using Mde.Project.Mobile.Core.MauiHelpers;
using Mde.Project.Mobile.Core.Models.Reports;
using Mde.Project.Mobile.Core.Reports.Service.Interfaces;
using Mde.Project.Mobile.Core.ResultModels;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Mde.Project.Mobile.Core.Reports.Service
{
    public class ReportsService : AuthenticationService, IReportsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ReportsService(IHttpClientFactory httpClientFactory, ISecureStorageService secureStorageService) : base(httpClientFactory, secureStorageService)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<PagingResultModel<Report>> TryGetReportListAsync(Guid warehouseId, int pageNumber = 1, int pageSize = 10)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            var productsResultModel = new PagingResultModel<Report>();

            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    productsResultModel.Message = "Token is empty";
                    return productsResultModel;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestUrl = $"/api/reports?warehouseId={warehouseId}&pageSize={pageSize}&pageNumber={pageNumber}";

                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    productsResultModel = await response.Content.ReadFromJsonAsync<PagingResultModel<Report>>();
                }
                else
                {
                    productsResultModel.Message = "Failed to retrieve report list";
                }
            }
            catch (Exception)
            {
                productsResultModel.Message = "Failed to retrieve report list";
            }

            return productsResultModel;
        }

        public async Task<ResultModel<Report>> TryGetReportByIdAsync(Guid reportId)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            var reportResultModel = new ResultModel<Report>();
            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    reportResultModel.Message = "Token is empty";
                    return reportResultModel;
                }
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestUrl = $"/api/reports/{reportId}";
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
                if (response.IsSuccessStatusCode)
                {
                    reportResultModel.Data = await response.Content.ReadFromJsonAsync<Report>();
                }
                else
                {
                    reportResultModel.Message = "Failed to retrieve report";
                }
            }
            catch (Exception)
            {
                reportResultModel.Message = "Failed to retrieve report";
            }
            return reportResultModel;
        }
    }
}
