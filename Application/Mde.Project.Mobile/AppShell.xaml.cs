using Mde.Project.Mobile.Pages.App;
using Mde.Project.Mobile.Pages.App.Users;
using Mde.Project.Mobile.Pages.Authentication;

namespace Mde.Project.Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(MauiRoutes.Dashboard, typeof(DashboardPage));
            Routing.RegisterRoute(MauiRoutes.Profile, typeof(UserProfilePage));
        }
    }
}
