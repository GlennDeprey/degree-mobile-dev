using Mde.Project.Mobile.Pages.App.Scanner;
using Mde.Project.Mobile.Pages.App.Users;

namespace Mde.Project.Mobile
{
    public partial class ScannerShell : Shell
    {
        public ScannerShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(MauiRoutes.ScannerSelectWarehouse, typeof(SelectWarehousePage));
            Routing.RegisterRoute(MauiRoutes.ScannerScan, typeof(ScannerPage));
            Routing.RegisterRoute(MauiRoutes.ScanProduct, typeof(ScanProductPage));
            Routing.RegisterRoute(MauiRoutes.Profile, typeof(UserProfilePage));
        }
    }
}
