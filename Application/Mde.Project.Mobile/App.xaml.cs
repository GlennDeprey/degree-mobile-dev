using Mde.Project.Mobile.Core;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using Mde.Project.Mobile.Services.Interface;

namespace Mde.Project.Mobile
{
    public partial class App : Application
    {
        private readonly IAuthenticationService _authenticationService;
        public App(IAuthenticationService authenticationService)
        {
            InitializeComponent();
            MainPage = new AuthShell();
            _authenticationService = authenticationService;
        }

        protected async override void OnStart() 
        {
            base.OnStart();
            //var authenticated = await AuthenticationCheck();
            //if (authenticated)
            //{
            //    await AuthorizeCheck();
            //}

        }

        protected async override void OnResume()
        {
            base.OnResume();
            //await AuthenticationCheck();
        }

        private async Task AuthorizeCheck()
        {
            if (!await _authenticationService.IsInRole(Constants.CustomerRole, true))
            {
                MainPage = new AppShell();
                return;
            }

            MainPage = new ScannerShell();
        }

        private async Task<bool> AuthenticationCheck()
        {
            bool isAuthenticated = await _authenticationService.IsAuthenticatedAsync();
            if (!isAuthenticated)
            {
                MainPage = new AuthShell();
            }

            return isAuthenticated;
        }
    }
}
