using Mde.Project.Mobile.Core.ResultModels;
using Mde.Project.Mobile.Services;

namespace Mde.Project.Mobile.Pages.Authentication;

public partial class ExternalLoginPage : ContentPage
{
    private string _callbackUrl;
    public ExternalLoginPage(string loginUrl, string callbackUrl)
	{
		InitializeComponent();
        _callbackUrl = callbackUrl;
        BindingContext = new { LoginUrl = loginUrl };
    }

    private void OnWebViewNavigated(object sender, WebNavigatedEventArgs e)
    {
        if (e.Url.StartsWith(_callbackUrl))
        {
            var uri = new Uri(e.Url);
            var queryParams = System.Web.HttpUtility.ParseQueryString(uri.Query);

            if (queryParams["access_token"] != null)
            {
                var token = queryParams["access_token"];
                ExternalLoginService.CompleteLogin(new ResultModel<string>
                {
                    Data = token,
                });
            }
            else if (queryParams["error"] != null)
            {
                var error = queryParams["error"];
                ExternalLoginService.CompleteLogin(new ResultModel<string>
                {
                    Message = error,
                });
            }

            Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}