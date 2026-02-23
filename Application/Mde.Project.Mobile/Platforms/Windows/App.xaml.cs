using Mde.Project.Mobile.Core.ResultModels;
using Mde.Project.Mobile.Services;
using Microsoft.Windows.AppLifecycle;
using System.Diagnostics;
using Windows.ApplicationModel.Activation;
using LaunchActivatedEventArgs = Microsoft.UI.Xaml.LaunchActivatedEventArgs;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Mde.Project.Mobile.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : MauiWinUIApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        //protected override async void OnLaunched(LaunchActivatedEventArgs args)
        //{
        //    var appInstance = AppInstance.GetCurrent();
        //    var e = appInstance.GetActivatedEventArgs();

        //    if (e.Kind != ExtendedActivationKind.Protocol ||
        //        e.Data is not ProtocolActivatedEventArgs protocol)
        //    {
        //        appInstance.Activated += OnAppActivated;

        //        base.OnLaunched(args);
        //        return;
        //    }
        //    var instances = AppInstance.GetInstances();
        //    await Task.WhenAll(instances
        //        .Select(async q => await q.RedirectActivationToAsync(e)));

        //    return;
        //}

        //private void OnAppActivated(object sender, AppActivationArguments e)
        //{
        //    if (e.Kind == ExtendedActivationKind.Protocol &&
        //        e.Data is ProtocolActivatedEventArgs protocolArgs)
        //    {
        //        var uri = protocolArgs.Uri;
        //        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);

        //        var token = query["access_token"];
        //        var error = query["error"];

        //        var result = new ResultModel<string>
        //        {
        //            Data = token,
        //            Message = error
        //        };

        //        // Complete the login process
        //        ExternalLoginService.CompleteLogin(result);
        //    }
        //}

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }

}
