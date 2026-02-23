using Mde.Project.Mobile.Pages.App;
using Mde.Project.Mobile.Pages.App.Map;
using Mde.Project.Mobile.Pages.App.Warehouses;
using Mde.Project.Mobile.Pages.Authentication;

namespace Mde.Project.Mobile
{
    public partial class WarehousesShell : Shell
    {
        public WarehousesShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(MauiRoutes.Warehouses, typeof(WarehousesPage));
            Routing.RegisterRoute(MauiRoutes.WarehouseDetail, typeof(WarehouseDetailPage));
            Routing.RegisterRoute(MauiRoutes.WarehouseUpsert, typeof(WarehouseUpsertPage));
            Routing.RegisterRoute(MauiRoutes.WarehouseDelete, typeof(WarehouseDeletePage));
            Routing.RegisterRoute(MauiRoutes.GoogleMaps, typeof(GoogleMapsPage));
            Routing.RegisterRoute(MauiRoutes.GoogleMapsFrame, typeof(GoogleMapsFramePage));
        }
    }
}
