using Mde.Project.Mobile.Core;
using Mde.Project.Mobile.Core.Authentication.Services;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using Mde.Project.Mobile.Core.Google;
using Mde.Project.Mobile.Core.Google.Interface;
using Mde.Project.Mobile.Core.MauiHelpers;
using Mde.Project.Mobile.Core.Products.Service;
using Mde.Project.Mobile.Core.Products.Service.Interfaces;
using Mde.Project.Mobile.Core.Warehouses.Interface;
using Mde.Project.Mobile.Core.Warehouses.Service;
using Mde.Project.Mobile.Pages.Authentication;
using Mde.Project.Mobile.ViewModels.Authentication;
using Mde.Project.Mobile.Services;
using Mde.Project.Mobile.Pages.App;
using Mde.Project.Mobile.Pages.App.Map;
using Mde.Project.Mobile.Pages.App.Operations;
using Mde.Project.Mobile.Pages.App.Products;
using Mde.Project.Mobile.Pages.App.Reports;
using Mde.Project.Mobile.Pages.App.Scanner;
using Mde.Project.Mobile.Pages.App.Statistics;
using Mde.Project.Mobile.Pages.App.Warehouses;
using Mde.Project.Mobile.ViewModels.App;
using Mde.Project.Mobile.ViewModels.App.Maps;
using Mde.Project.Mobile.ViewModels.App.Operations;
using Mde.Project.Mobile.ViewModels.App.Reports;
using Mde.Project.Mobile.ViewModels.App.Statistics;
using Mde.Project.Mobile.ViewModels.App.Products;
using Mde.Project.Mobile.ViewModels.App.Scanner;
using Mde.Project.Mobile.ViewModels.App.Warehouses;
using Mde.Project.Mobile.Services.Interface;
using Mde.Project.Mobile.Core.Items.Interfaces;
using Mde.Project.Mobile.Core.Items.Service;
using Mde.Project.Mobile.Core.Reports.Service.Interfaces;
using Mde.Project.Mobile.Core.Reports.Service;
using Mde.Project.Mobile.Pages.App.Users;
using Mde.Project.Mobile.ViewModels.App.Users;
using Mde.Project.Mobile.Core.Statistics.Interface;
using Mde.Project.Mobile.Core.Statistics.Service;
using SkiaSharp;

namespace Mde.Project.Mobile
{
    public static class MauiServiceHelper
    {
        public static MauiAppBuilder ConfigureServices(this MauiAppBuilder mauiAppBuilder)
        {
            // Services
            mauiAppBuilder.Services.AddHttpClient(Constants.ProjectClientName, config =>
            {
                config.BaseAddress = new Uri(Constants.ProjectApiUrl);
            });

            mauiAppBuilder.Services.AddHttpClient(Constants.GooglePlaceClientName, config =>
            {
                config.BaseAddress = new Uri(Constants.GooglePlaceUrl);
            });

            mauiAppBuilder.Services.AddSingleton<ISecureStorageService, SecureStorageService>();
            mauiAppBuilder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
            mauiAppBuilder.Services.AddTransient<IProductService, ProductService>();
            mauiAppBuilder.Services.AddTransient<IWarehouseService, WarehouseService>();
            mauiAppBuilder.Services.AddTransient<IWarehouseItemsService, WarehouseItemsService>();
            mauiAppBuilder.Services.AddTransient<IReportsService, ReportsService>();
            mauiAppBuilder.Services.AddTransient<IGoogleApiService, GoogleApiService>();
            mauiAppBuilder.Services.AddTransient<IStatisticsService, StatisticsService>();

            // External Login
            mauiAppBuilder.Services.AddSingleton<IExternalLoginService, ExternalLoginService>();

            // Services - Charts
            mauiAppBuilder.Services.AddSingleton<IChartsService, ChartsService>();

            // Hubs
            mauiAppBuilder.Services.AddSingleton<IWarehouseHubService, WarehouseHubService>();

            // Pages - AUTH
            mauiAppBuilder.Services.AddTransient<SignInPage>();
            mauiAppBuilder.Services.AddTransient<SignUpPage>();
            mauiAppBuilder.Services.AddTransient<VerifyCodePage>();
            mauiAppBuilder.Services.AddTransient<ResetPasswordPage>();
            mauiAppBuilder.Services.AddTransient<ForgetPasswordPage>();

            // ViewModels - AUTH
            mauiAppBuilder.Services.AddTransient<SignInViewModel>();
            mauiAppBuilder.Services.AddTransient<SignUpViewModel>();
            mauiAppBuilder.Services.AddTransient<VerifyCodeViewModel>();
            mauiAppBuilder.Services.AddTransient<ResetPasswordViewModel>();
            mauiAppBuilder.Services.AddTransient<ForgetPasswordViewModel>();

            // Pages - Profile
            mauiAppBuilder.Services.AddTransient<UserProfilePage>();

            // ViewModels - Profile
            mauiAppBuilder.Services.AddTransient<UserProfileViewModel>();

            // Pages - Dashboard
            mauiAppBuilder.Services.AddTransient<DashboardPage>();

            // ViewModels - Dashboard
            mauiAppBuilder.Services.AddTransient<DashboardViewModel>();

            // Pages - SCANNER
            mauiAppBuilder.Services.AddTransient<SelectWarehousePage>();
            mauiAppBuilder.Services.AddTransient<ScannerPage>();
            mauiAppBuilder.Services.AddTransient<ScanProductPage>();

            // ViewModels - SCANNER
            mauiAppBuilder.Services.AddTransient<SelectWarehouseViewModel>();
            mauiAppBuilder.Services.AddTransient<ScannerViewModel>();

            // Pages - PRODUCTS
            mauiAppBuilder.Services.AddTransient<ProductsPage>();
            mauiAppBuilder.Services.AddTransient<ProductUpsertPage>();
            mauiAppBuilder.Services.AddTransient<ProductDeletePage>();
            mauiAppBuilder.Services.AddTransient<ProductDetailPage>();

            mauiAppBuilder.Services.AddTransient<CategoriesPage>();
            mauiAppBuilder.Services.AddTransient<CategoryUpsertPage>();
            mauiAppBuilder.Services.AddTransient<CategoryDeletePage>();
            mauiAppBuilder.Services.AddTransient<CategoryDetailPage>();

            mauiAppBuilder.Services.AddTransient<BrandsPage>();
            mauiAppBuilder.Services.AddTransient<BrandUpsertPage>();
            mauiAppBuilder.Services.AddTransient<BrandDeletePage>();
            mauiAppBuilder.Services.AddTransient<BrandDetailPage>();

            // ViewModels - PRODUCTS
            mauiAppBuilder.Services.AddTransient<ProductsViewModel>();
            mauiAppBuilder.Services.AddTransient<ProductUpsertViewModel>();
            mauiAppBuilder.Services.AddTransient<ProductDeleteViewModel>();
            mauiAppBuilder.Services.AddTransient<ProductDetailViewModel>();

            mauiAppBuilder.Services.AddTransient<CategoriesViewModel>();
            mauiAppBuilder.Services.AddTransient<CategoryUpsertViewModel>();
            mauiAppBuilder.Services.AddTransient<CategoryDeleteViewModel>();
            mauiAppBuilder.Services.AddTransient<CategoryDetailViewModel>();

            mauiAppBuilder.Services.AddTransient<BrandsViewModel>();
            mauiAppBuilder.Services.AddTransient<BrandUpsertViewModel>();
            mauiAppBuilder.Services.AddTransient<BrandDeleteViewModel>();
            mauiAppBuilder.Services.AddTransient<BrandDetailViewModel>();

            // Pages - WAREHOUSES
            mauiAppBuilder.Services.AddTransient<WarehousesPage>();
            mauiAppBuilder.Services.AddTransient<WarehouseDetailPage>();
            mauiAppBuilder.Services.AddTransient<WarehouseUpsertPage>();
            mauiAppBuilder.Services.AddTransient<WarehouseDeletePage>();

            // ViewModels - WAREHOUSES
            mauiAppBuilder.Services.AddTransient<WarehousesViewModel>();
            mauiAppBuilder.Services.AddTransient<WarehouseDetailViewModel>();
            mauiAppBuilder.Services.AddTransient<WarehouseUpsertViewModel>();
            mauiAppBuilder.Services.AddTransient<WarehouseDeleteViewModel>();

            // Pages - MAPS
            mauiAppBuilder.Services.AddTransient<GoogleMapsPage>();
            mauiAppBuilder.Services.AddTransient<GoogleMapsFramePage>();

            // ViewModels - MAPS
            mauiAppBuilder.Services.AddTransient<GoogleMapsViewModel>();
            mauiAppBuilder.Services.AddTransient<GoogleMapsFrameViewModel>();

            // Pages - OPERATIONS
            mauiAppBuilder.Services.AddTransient<StockConfigurationPage>();
            mauiAppBuilder.Services.AddTransient<StockSettingsPage>();
            mauiAppBuilder.Services.AddTransient<StockOrderPage>();
            mauiAppBuilder.Services.AddTransient<StockTransferPage>();

            // ViewModels - OPERATIONS
            mauiAppBuilder.Services.AddTransient<StockConfigurationViewModel>();
            mauiAppBuilder.Services.AddTransient<StockSettingsViewModel>();
            mauiAppBuilder.Services.AddTransient<StockOrderViewModel>();
            mauiAppBuilder.Services.AddTransient<StockTransferViewModel>();

            // Pages - REPORTS
            mauiAppBuilder.Services.AddTransient<ReportsPage>();

            // ViewModels - REPORTS
            mauiAppBuilder.Services.AddTransient<ReportsViewModel>();

            // Pages - Statistics
            mauiAppBuilder.Services.AddTransient<StatisticsPage>();

            // ViewModels - Statistics
            mauiAppBuilder.Services.AddTransient<StatisticsViewModel>();

            return mauiAppBuilder;
        }
        public static Stream ResizeImage(Stream inputStream, int maxWidth, int maxHeight)
        {
            using var original = SKBitmap.Decode(inputStream);
            if (original == null)
                return null;

            float widthRatio = (float)maxWidth / original.Width;
            float heightRatio = (float)maxHeight / original.Height;
            float scaleRatio = Math.Min(widthRatio, heightRatio);

            int newWidth = (int)(original.Width * scaleRatio);
            int newHeight = (int)(original.Height * scaleRatio);

            using var resizedBitmap = original.Resize(new SKImageInfo(newWidth, newHeight), SKSamplingOptions.Default);
            using var image = SKImage.FromBitmap(resizedBitmap);
            using var data = image.Encode(SKEncodedImageFormat.Jpeg, 75);

            var resizedStream = new MemoryStream();
            data.SaveTo(resizedStream);
            resizedStream.Position = 0;
            return resizedStream;
        }
    }
}
