using Mde.Project.Mobile.Core.ResultModels;
using Mde.Project.Mobile.Services;

namespace Mde.Project.Mobile.Pages.App.Users;

public partial class ExternalLinkPage : ContentPage
{
    private string _callbackUrl;
    public ExternalLinkPage(string linkUrl, string callbackUrl)
	{
		InitializeComponent();
        _callbackUrl = callbackUrl;
        BindingContext = new { LinkUrl = linkUrl };
    }

    private void OnWebViewNavigated(object sender, WebNavigatedEventArgs e)
    {
        if (e.Url.StartsWith(_callbackUrl))
        {
            var uri = new Uri(e.Url);
            var queryParams = System.Web.HttpUtility.ParseQueryString(uri.Query);

            var result = new BaseResultModel();

            if (queryParams["error"] != null)
            {
                var error = queryParams["error"];
                result.Message = error;

                ExternalLoginService.CompleteLink(result);
            }

            Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}