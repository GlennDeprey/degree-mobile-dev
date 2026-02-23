using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.ViewModels.App.Maps;

namespace Mde.Project.Mobile.Pages.App.Map;

public partial class GoogleMapsFramePage : ContentPage
{
    private readonly GoogleMapsFrameViewModel _googleMapsFrameViewModel;
	public GoogleMapsFramePage(GoogleMapsFrameViewModel googleMapsFrameViewModel)
    {
        _googleMapsFrameViewModel = googleMapsFrameViewModel;
        BindingContext = _googleMapsFrameViewModel;
		InitializeComponent();
    }
}