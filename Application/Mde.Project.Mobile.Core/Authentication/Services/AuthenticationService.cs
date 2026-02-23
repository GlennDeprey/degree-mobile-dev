using Mde.Project.Mobile.Core.Authentication.Dtos;
using Mde.Project.Mobile.Core.Authentication.Models;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using Mde.Project.Mobile.Core.MauiHelpers;
using Mde.Project.Mobile.Core.ResultModels;
using Microsoft.AspNetCore.WebUtilities;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;

namespace Mde.Project.Mobile.Core.Authentication.Services
{
    public class AuthenticationService : IAuthenticationService
    { 
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISecureStorageService _secureStorage;
        public AuthenticationService(IHttpClientFactory httpClientFactory, ISecureStorageService secureStorageService)
        {
            _httpClientFactory = httpClientFactory;
            _secureStorage = secureStorageService;
        }

        public async Task<string> GetTokenAsync()
        {
            return await _secureStorage.GetAsync(Constants.TokenKey);
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            string encodedToken = await GetTokenAsync();
            if (encodedToken == null) { return false; }
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(encodedToken) as JwtSecurityToken;
            return jsonToken.ValidTo > DateTime.UtcNow;
        }

        public async Task<bool> IsInRole(string role, bool onlyRole = false)
        {
            var result = await TryGetProfileAsync();
            if (!result.IsSuccess)
            {
                return false;
            }

            var roles = result.Data.Roles;
            if (onlyRole)
            {
                if (roles.Count() == 1 && roles.First().Equals(role, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            else
            {
                if (roles.Any(r => r.Equals(role, StringComparison.CurrentCultureIgnoreCase)))
                {
                    return true;
                }
            }

            return false;
        }

        public bool SignOut()
        {
            bool success = _secureStorage.Remove(Constants.TokenKey);
            return success;
        }

        public async Task<ResultModel<UserProfileModel>> TryGetProfileAsync()
        {
            var resultModel = new ResultModel<UserProfileModel>();
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    resultModel.Message = "Token is empty";
                    return resultModel;
                }
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestUrl = $"/api/accounts";
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    resultModel.Data = await response.Content.ReadFromJsonAsync<UserProfileModel>();
                }
                else
                {
                    resultModel.Message = await response.Content.ReadAsStringAsync();
                }
            }
            catch
            {
                resultModel.Message = "There was a issue connecting to the server.";
                return resultModel;
            }

            return resultModel;
        }

        public async Task<BaseResultModel> TryUploadProfilePictureAsync(Stream imageStream, string fileName)
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

                var requestUrl = "/api/accounts";
                HttpResponseMessage response = null;

                using (var form = new MultipartFormDataContent())
                {
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
                        form.Add(fileContent, "profilePicture", fileName);

                        response = await httpClient.PutAsync(requestUrl, form);
                        if (!response.IsSuccessStatusCode)
                        {
                            resultModel.Message = await response.Content.ReadAsStringAsync();
                        }
                    }
                }
            }
            catch
            {
                resultModel.Message = "There was a issue connecting to the server.";
            }

            return resultModel;
        }

        public async Task<bool> TrySignInAsync(string email, string password)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                "/api/accounts/login",
                new SignInRequestDto { Email = email, Password = password });

            if (response.IsSuccessStatusCode)
            {
                SignInResponseDto signInResponseDto = await
                response.Content.ReadFromJsonAsync<SignInResponseDto>();
                await StoreToken(signInResponseDto.Token);
            }

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> TrySignUpAsync(string email, string firstname, string lastname, string password)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                "/api/accounts/register",
                new SignUpRequestDto { Email = email, FirstName=firstname, LastName=lastname, Password = password });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> TryForgotPasswordAsync(string email)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                "/api/accounts/forgot-password",
                new ForgotPasswordRequestDto { Email = email });
            return response.IsSuccessStatusCode;
        }

        public async Task<string> TryVerifyCodeAsync(string email, string code)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                "/api/accounts/verify-code",
                new VerifyCodeRequestDto { Email = email, Code = code });
            if (response.IsSuccessStatusCode)
            {
                VerifyCodeResponseDto verifyCodeResponseDto = await
                response.Content.ReadFromJsonAsync<VerifyCodeResponseDto>();

                // Decode the token
                string decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(verifyCodeResponseDto.Token));

                return decodedToken;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
            }

            return null;
        }

        public async Task<bool> TryResetPasswordAsync(string email, string password, string token)
        {
            string encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                "/api/accounts/reset-password",
                new ResetPasswordRequestDto { Email = email, Password = password, Token = encodedToken });
            return response.IsSuccessStatusCode;
        }

        public async Task<UserInfoModel> GetUserInfoAsync()
        {
            var userModel = new UserInfoModel();
            string encodedToken = await GetTokenAsync();
            if (encodedToken == null) { return userModel; }
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(encodedToken) as JwtSecurityToken;

            try
            {
                userModel.Email = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                userModel.FirstName = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
                userModel.LastName = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
                userModel.IsSuccess = true;
                return userModel;
            }
            catch
            {
                return userModel;
            }
        }

        private async Task StoreToken(string token)
        {
            await _secureStorage.SetAsync(Constants.TokenKey, token);
        }
    }
}
