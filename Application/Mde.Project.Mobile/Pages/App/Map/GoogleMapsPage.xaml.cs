using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Messages.Maps;
using Mde.Project.Mobile.Models.Maps;
using Mde.Project.Mobile.ViewModels.App.Maps;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace Mde.Project.Mobile.Pages.App.Map;

public partial class GoogleMapsPage : ContentPage
{
    private readonly GoogleMapsViewModel _googleMapsViewModel;
    public GoogleMapsLocationModel Location { get; set; }

    public GoogleMapsPage(GoogleMapsViewModel googleMapsViewModel)
	{
        _googleMapsViewModel = googleMapsViewModel;
        BindingContext = _googleMapsViewModel;
        InitializeComponent();

        WeakReferenceMessenger.Default.Register<SendLocationMessage>(this, async (r, m) =>
        {
            Location = new GoogleMapsLocationModel
            {
                Name = m.Name,
                Address = m.Address,
                Location = new Location
                {
                    Latitude = m.Latitude,
                    Longitude = m.Longitude
                }
            };
            Title = Location.Name;
            await InitializeMapAsync();
        });
    }

    private async Task InitializeMapAsync()
    {
        await Task.Run(() =>
        {
            MapSpan mapSpan = new MapSpan(Location.Location, 0.01, 0.01);
            var map = new Microsoft.Maui.Controls.Maps.Map(mapSpan);

            Pin pin = new Pin
            {
                Label = Location.Name,
                Address = Location.Address,
                Type = PinType.Place,
                Location = Location.Location
            };
            map.Pins.Add(pin);

            // Ensure UI updates are done on the main thread
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _googleMapsViewModel.LoadedCompleteCommand.Execute(null);
                Content = map;
            });
        });
    }
}