using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Messages.Maps;
using Mde.Project.Mobile.Models.Maps;
using Constants = Mde.Project.Mobile.Core.Constants;

namespace Mde.Project.Mobile.ViewModels.App.Maps
{
    public partial class GoogleMapsFrameViewModel : ObservableObject
    {
        [ObservableProperty] private bool _isBusy;

        [ObservableProperty] private GoogleMapsLocationModel _location;

        [ObservableProperty] private string _title;

        [ObservableProperty] private HtmlWebViewSource _source;
        public GoogleMapsFrameViewModel()
        {
            IsBusy = true;
            WeakReferenceMessenger.Default.Register<SendLocationMessage>(this, async (r, m) =>
            {
                Location = new GoogleMapsLocationModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Address = m.Address,
                    Location = new Location
                    {
                        Latitude = m.Latitude,
                        Longitude = m.Longitude
                    }
                };
                Title = Location.Name;
                Source = new HtmlWebViewSource
                {
                    Html = $@"
            <iframe src='https://www.google.be/maps/embed/v1/place?key={Constants.GoogleApiKey}&center={Location.Location.Latitude},{Location.Location.Longitude}&zoom=15&q={Location.Address}' frameborder='0' width='100%' height='100%'></iframe>"
                };

                IsBusy = false;
            });
            
        }
    }
}
