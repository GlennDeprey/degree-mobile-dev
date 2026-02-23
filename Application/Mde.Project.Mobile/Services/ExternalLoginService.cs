using Mde.Project.Mobile.Core;
using Mde.Project.Mobile.Core.MauiHelpers;
using Mde.Project.Mobile.Core.ResultModels;
using Mde.Project.Mobile.Pages.App.Users;
using Mde.Project.Mobile.Pages.Authentication;
using Mde.Project.Mobile.Services.Interface;
using Mde.Project.Mobile.Services.Types;
using System.Net.Http.Headers;

namespace Mde.Project.Mobile.Services
{
    class ExternalLoginService : IExternalLoginService
    {
        private readonly string _loginGoogleUrl;
        private readonly string _linkGoogletUrl;
        private readonly string _unlinkGoogleUrl;
        private readonly string _callbackUrl;
        private readonly string _windowsGoogleCallbackUri;
        private readonly string _windowsGoogleLinkCallbackUri;
        private readonly string _tokenIdentifier;
        private readonly string _errorIdentifier;

        private static TaskCompletionSource<ResultModel<string>> _loginCompletionSource;
        private static TaskCompletionSource<BaseResultModel> _linkCompletionSource;

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISecureStorageService _secureStorageService;
        public ExternalLoginService(ISecureStorageService secureStorageService, IHttpClientFactory httpClientFactory)
        {
            _secureStorageService = secureStorageService;
            _httpClientFactory = httpClientFactory;
            _loginGoogleUrl = $"{Constants.ProjectApiUrl}/api/Accounts/google/login?isLogin=true";
            _linkGoogletUrl = $"{Constants.ProjectApiUrl}/api/Accounts/google/login?isLogin=false";
            _unlinkGoogleUrl = $"{Constants.ProjectApiUrl}/api/Accounts/google/link/remove";
            _windowsGoogleCallbackUri = $"{Constants.ProjectApiUrl}/api/Accounts/google/login/callback";
            _windowsGoogleLinkCallbackUri = $"{Constants.ProjectApiUrl}/api/Accounts/google/link/callback";
            _callbackUrl = "myapp://";
            _tokenIdentifier = "access_token";
            _errorIdentifier = "error";
        }

        public async Task<BaseResultModel> LoginAsync(ExternalLoginType loginType)
        {
            var result = new BaseResultModel();
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                var windowsResult = loginType == ExternalLoginType.Google ? 
                    await LoginOnGoogleWindowsAsync() : await LoginOnMicrosoftWindowsAsync();
                result = windowsResult;
                return result;
            }
            else
            {
                var androidResult = loginType == ExternalLoginType.Google ? 
                    await LoginOnGoogleMobileAsync() : await LoginOnMicrosoftMobileAsync();
                result = androidResult;
                return result;
            }
        }

        public async Task<BaseResultModel> LinkAsync(ExternalLoginType linkType)
        {
            var result = new BaseResultModel();
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                var windowsResult = linkType == ExternalLoginType.Google ?
                    await LinkOnGoogleWindowsAsync() : await LinkOnMicrosoftWindowsAsync();
                result = windowsResult;
                return result;
            }
            else
            {
                var androidResult = linkType == ExternalLoginType.Google ?
                    await LinkOnGoogleMobileAsync() : await LinkOnMicrosoftMobileAsync();
                result = androidResult;
                return result;
            }
        }

        public async Task<BaseResultModel> UnlinkAsync(ExternalLoginType linkType)
        {
            var result = linkType == ExternalLoginType.Google ?
                await UnlinkGoogleAsync() : await UnlinkMicrosoftAsync();

            return result;
        }

        private async Task<BaseResultModel> UnlinkGoogleAsync()
        {
            var result = new BaseResultModel();
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            try
            {
                var token = await _secureStorageService.GetAsync(Constants.TokenKey);
                if (string.IsNullOrEmpty(token))
                {
                    result.Message = "Token is empty";
                    return result;
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await httpClient.DeleteAsync(_unlinkGoogleUrl);
                if (!response.IsSuccessStatusCode)
                { 
                    var responseContent = await response.Content.ReadAsStringAsync();
                    result.Message = responseContent ?? "Failed to unlink Google Account";
                }

            }
            catch (Exception ex)
            {
                result.Message = $"Google unlink failed: {ex.Message}";
            }

            return result;
        }

        private async Task<BaseResultModel> UnlinkMicrosoftAsync()
        {
            var result = new BaseResultModel();
            result.Message = "Microsoft unlinking is currently unavailable on windows.";
            return result;
        }

        private async Task<BaseResultModel> LinkOnGoogleWindowsAsync()
        {
            var result = new BaseResultModel();
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            try
            {
                var token = await _secureStorageService.GetAsync(Constants.TokenKey);
                if (string.IsNullOrEmpty(token))
                {
                    result.Message = "Token is empty";
                    return result;
                }

                var linkGoogleUrl = $"{_linkGoogletUrl}&token={token}&redirectUri={Uri.EscapeDataString(_windowsGoogleLinkCallbackUri)}";
                _linkCompletionSource = new TaskCompletionSource<BaseResultModel>();

                await Application.Current.MainPage.Navigation.PushAsync(new ExternalLinkPage(linkGoogleUrl, _windowsGoogleLinkCallbackUri));

                var linkResult = await _linkCompletionSource.Task;

                if (!linkResult.IsSuccess)
                {
                    result.Message = linkResult.Message ?? "Google Link Error.. Please try again later";
                }
            }
            catch (Exception ex)
            {
                result.Message = $"Google link failed: {ex.Message}";
            }

            return result;
        }

        private async Task<BaseResultModel> LinkOnGoogleMobileAsync()
        {
            var result = new BaseResultModel();
            var httpClient = _httpClientFactory.CreateClient(Constants.ProjectClientName);
            try
            {
                var token = await _secureStorageService.GetAsync(Constants.TokenKey);
                if (string.IsNullOrEmpty(token))
                {
                    result.Message = "Token is empty";
                    return result;
                }

                var linkGoogleUrl = $"{_linkGoogletUrl}&token={token}";

                var authResult = await WebAuthenticator.AuthenticateAsync(new WebAuthenticatorOptions
                {
                    Url = new Uri(linkGoogleUrl),
                    CallbackUrl = new Uri(_callbackUrl)                  
                });

                if (authResult.Properties.TryGetValue(_errorIdentifier, out var error))
                {
                    result.Message = string.IsNullOrWhiteSpace(error) ? "Google Link Error.. Please try again later" : error;
                }
            }
            catch (Exception ex)
            {
                result.Message = $"Google link failed: {ex.Message}";
            }

            return result;
        }

        private async Task<BaseResultModel> LinkOnMicrosoftWindowsAsync()
        {
            var result = new BaseResultModel();
            result.Message = "Microsoft linking is currently unavailable on windows.";

            return result;
        }

        private async Task<BaseResultModel> LinkOnMicrosoftMobileAsync()
        {
            var result = new BaseResultModel();
            result.Message = "Microsoft linking is currently unavailable on windows.";

            return result;
        }

        private async Task<BaseResultModel> LoginOnMicrosoftMobileAsync()
        {
            var result = new BaseResultModel();
            result.Message = "Microsoft login is currently unavailable on mobile.";

            return result;
        }

        private async Task<BaseResultModel> LoginOnMicrosoftWindowsAsync()
        {
            var result = new BaseResultModel();
            result.Message = "Microsoft login is currently unavailable on windows.";

            return result;
        }

        private async Task<BaseResultModel> LoginOnGoogleMobileAsync()
        {
            var result = new BaseResultModel();
            try
            {
                var authResult = await WebAuthenticator.AuthenticateAsync(new WebAuthenticatorOptions
                {
                    Url = new Uri(_loginGoogleUrl),
                    CallbackUrl = new Uri(_callbackUrl)
                });

                if (authResult.Properties.TryGetValue(_tokenIdentifier, out var token))
                {
                    await _secureStorageService.SetAsync(Constants.TokenKey, token);
                    return result;
                }

                authResult.Properties.TryGetValue(_errorIdentifier, out var error);
                result.Message = string.IsNullOrWhiteSpace(error) ? "Google Auth Error.. Please try again later" : error;

            }
            catch(Exception ex)
            {
                result.Message = $"Authentication failed: {ex.Message}";
            }

            return result;
        }

        private async Task<BaseResultModel> LoginOnGoogleWindowsAsync()
        {
            var result = new BaseResultModel();
            try
            {
                string loginUrl = $"{_loginGoogleUrl}&redirectUri={Uri.EscapeDataString(_windowsGoogleCallbackUri)}";

                _loginCompletionSource = new TaskCompletionSource<ResultModel<string>>();

                await Application.Current.MainPage.Navigation.PushAsync(new ExternalLoginPage(loginUrl, _windowsGoogleCallbackUri));

                var loginResult = await _loginCompletionSource.Task;

                if (string.IsNullOrWhiteSpace(loginResult.Data))
                {
                    result.Message = loginResult.Message ?? "Google Auth Error.. Please try again later";
                }
                else
                {
                    await _secureStorageService.SetAsync(Constants.TokenKey, loginResult.Data);
                }
            }
            catch (Exception ex)
            {
                result.Message = $"Authentication failed: {ex.Message}";
            }

            return result;
        }

        public static void CompleteLogin(ResultModel<string> result)
        {
            if (_loginCompletionSource != null && !_loginCompletionSource.Task.IsCompleted)
            {
                _loginCompletionSource.SetResult(result);
            }
        }

        public static void CompleteLink(BaseResultModel result)
        {
            if (_linkCompletionSource != null && !_linkCompletionSource.Task.IsCompleted)
            {
                _linkCompletionSource.SetResult(result);
            }
        }
    }
}
