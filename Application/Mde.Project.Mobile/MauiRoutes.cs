using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile
{
    public static class MauiRoutes
    {
        // Auth
        public const string AuthSignIn = "//signIn";
        public const string AuthSignUp = "//signUp";
        public const string AuthForgotPassword = "//forgotpassword";
        public const string AuthVerifyCode = "//verifycode";
        public const string AuthResetPassword = "//resetpassword";

        // Profile
        public const string Profile = "profile";

        // Dashboard
        public const string Dashboard = "//dashboard";

        // Scanner
        public const string ScannerSelectWarehouse = "//selectwarehouse";
        public const string ScannerScan = "scan";
        public const string ScanProduct = "scanProduct";

        // Products
        public const string Products = "//products";
        public const string ProductUpsert = "productUpsert";
        public const string ProductDelete = "productDelete";
        public const string ProductDetail = "productDetail";
        public const string Brands = "//brands";
        public const string BrandUpsert = "brandUpsert";
        public const string BrandDelete = "brandDelete";
        public const string BrandDetail = "brandDetail";
        public const string Categories = "//categories";
        public const string CategoryUpsert = "categoryUpsert";
        public const string CategoryDelete = "categoryDelete";
        public const string CategoryDetail = "categoryDetail";

        // Warehouses
        public const string Warehouses = "//warehouses";
        public const string WarehouseUpsert = "warehouseUpsert";
        public const string WarehouseDelete = "warehouseDelete";
        public const string WarehouseDetail = "warehouseDetail";

        // Maps
        public const string GoogleMaps = "maps";
        public const string GoogleMapsFrame = "mapsframe";

        // Operations
        public const string StockManagement = "//stockManagement";
        public const string StockSettings = "stockSettings";
        public const string StockOrder = "//stockOrder";
        public const string StockTransfer = "//stockTransfer";

        // Reports
        public const string Reports = "//reports";

        // Statistics
        public const string Statistics = "//statistics";
    }
}
