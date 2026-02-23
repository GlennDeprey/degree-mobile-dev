using Mde.Project.Mobile.Pages.App.Operations;
using Mde.Project.Mobile.Services.Interface;

namespace Mde.Project.Mobile
{
    public partial class OperationsShell : Shell
    {

        public OperationsShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(MauiRoutes.StockManagement, typeof(StockConfigurationPage));
            Routing.RegisterRoute(MauiRoutes.StockSettings, typeof(StockSettingsPage));
            Routing.RegisterRoute(MauiRoutes.StockOrder, typeof(StockOrderPage));
            Routing.RegisterRoute(MauiRoutes.StockTransfer, typeof(StockTransferPage));
        }
    }
}
