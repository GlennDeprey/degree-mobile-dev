using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using UraniumUI;
using InputKit.Handlers;
using SkiaSharp.Views.Maui.Controls.Hosting;
using ZXing.Net.Maui.Controls;
using LiveChartsCore.SkiaSharpView.Maui;

namespace Mde.Project.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()
                .UseLiveCharts()
                .UseMauiCommunityToolkit()
                .UseUraniumUI()
                .UseUraniumUIMaterial()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("LibreBarcodeEAN13Text-Regular.ttf", "Barcode");
                    fonts.AddFontAwesomeIconFonts();
                    fonts.AddMaterialIconFonts();
                })
                .ConfigureMauiHandlers(handlers =>
                {
                    handlers.AddInputKitHandlers();
#if ANDROID
                    handlers.AddMauiMaps();
#endif
                })
                
                .ConfigureServices();

#if ANDROID
            builder
                .UseMauiMaps()
                .UseBarcodeReader();
#endif

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
