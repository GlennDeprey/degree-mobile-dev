using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Core;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using Mde.Project.Mobile.Core.Google.Interface;
using Mde.Project.Mobile.Core.Models.Google;
using Mde.Project.Mobile.Core.Models.Warehouses;
using Mde.Project.Mobile.Core.Warehouses.Interface;
using Mde.Project.Mobile.Core.Warehouses.RequestModels;
using Mde.Project.Mobile.Messages;
using Mde.Project.Mobile.Messages.Products;
using Mde.Project.Mobile.Messages.Warehouses;
using Mde.Project.Mobile.Models.Google;
using Mde.Project.Mobile.Models.Warehouse;
using Mde.Project.Mobile.ViewModels.Base;

namespace Mde.Project.Mobile.ViewModels.App.Warehouses
{
    public partial class WarehouseUpsertViewModel : UserViewModel
    {
        [ObservableProperty]
        private WarehouseDetailModel _warehouse;

        [ObservableProperty]
        private ObservableCollection<string> _autoCompleteText;

        [ObservableProperty]
        private string _selectedAutoCompleteText;

        [ObservableProperty] private string _title;

        [ObservableProperty] private string _imageUrl;

        [ObservableProperty]
        private bool _isBusy;

        private IEnumerable<GoogleAutoCompleteModel> _autoCompletePlaces;
        private readonly IAuthenticationService _authenticationService;
        private readonly IWarehouseService _warehouseService;
        private readonly IGoogleApiService _googleApiService;

        private Random _random;
        public WarehouseUpsertViewModel(IAuthenticationService authenticationService, IWarehouseService warehouseService, IGoogleApiService googleApiService) : base(authenticationService)
        {
            _authenticationService = authenticationService;
            _warehouseService = warehouseService;
            _googleApiService = googleApiService;
            Warehouse = new WarehouseDetailModel();
            ImageUrl = Constants.NoImageUri;
            Title = "Add Warehouse";

            WeakReferenceMessenger.Default.Register<SendWarehouseDetailMessage>(this, async (r, m) =>
            {
                Warehouse = m.Warehouse;
                Title = "Edit Warehouse";

                var hasLocation = Warehouse.GoogleInfo != null &&
                                  !string.IsNullOrWhiteSpace(Warehouse.GoogleInfo?.GoogleAddress) &&
                                  !string.IsNullOrWhiteSpace(Warehouse.GoogleInfo?.GoogleAddressId);
                if (hasLocation)
                {
                    _autoCompletePlaces = new List<GoogleAutoCompleteModel>
                    {
                        new GoogleAutoCompleteModel
                        {
                            Name = Warehouse.GoogleInfo.GoogleAddress,
                            PlaceId = Warehouse.GoogleInfo.GoogleAddressId
                        }
                    };

                    AutoCompleteText = new ObservableCollection<string>
                    {
                        Warehouse.GoogleInfo.GoogleAddress
                    };

                    SelectedAutoCompleteText = Warehouse.GoogleInfo.GoogleAddress;

                    if (Warehouse.GoogleInfo.GooglePhotoUris.Any())
                    {
                        _random = new Random();
                        var photoCount = Warehouse.GoogleInfo.GooglePhotoUris.Count();
                        var randomImageIndex = _random.Next(0, photoCount);

                        var images = Warehouse.GoogleInfo.GooglePhotoUris.ToList();
                        ImageUrl = images[randomImageIndex].PhotoUri;
                    }
                }
            });
        }

        [RelayCommand]
        public async Task PopulateSuggestionListAsync(string text)
        {
            var location = await _googleApiService.GetAutoCompletePlacesAsync(text);
            if (location.IsSuccess)
            {
                _autoCompletePlaces = new List<GoogleAutoCompleteModel>(
                    location.Items.Select(s => new GoogleAutoCompleteModel
                    {
                        PlaceId = s.PlaceId,
                        Name = s.Name
                    }).ToList()
                );

                AutoCompleteText = new ObservableCollection<string>(
                    _autoCompletePlaces.Select(s => s.Name).ToList()
                );
            }
            else
            {
                _autoCompletePlaces = new List<GoogleAutoCompleteModel>();
                AutoCompleteText = new ObservableCollection<string>();
            }
        }

        [RelayCommand]
        public async Task UpsertWarehouseAsync()
        {
            IsBusy = true;
            var info  = _autoCompletePlaces.FirstOrDefault(x => x.Name == SelectedAutoCompleteText);
            if (info == null)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("Please select a valid address from the list.", typeof(WarehouseUpsertViewModel)));
                return;
            }

            var location = await _googleApiService.GetPlaceDetailsAsync(info.PlaceId);
            if (!location.IsSuccess)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("There was a issue finding location details.", typeof(WarehouseUpsertViewModel)));
                return;
            }

            var warehouseUpsertRequestModel = new WarehouseUpsertRequestModel
            {
                Name = Warehouse.Name,
                ShortName = Warehouse.ShortName,
                Phone = Warehouse.Phone,
                Location = new WarehouseLocation
                {
                    Address = location.Data.Address,
                    City = location.Data.City,
                    PostalCode = location.Data.PostalCode,
                    State = location.Data.State,
                    Country = location.Data.Country,
                    Latitude = location.Data.Latitude,
                    Longitude = location.Data.Longitude,
                    
                },
                GoogleInfo = new GooglePlaceInfo
                {
                    GoogleAddress = info.Name,
                    GoogleAddressId = info.PlaceId,
                    GooglePhotoUris = location.Data.PhotoUris
                }
            };

            if (Warehouse.Id != null && Warehouse.Id != Guid.Empty)
            {
                warehouseUpsertRequestModel.Id = Warehouse.Id.Value;
            }

            var result = await _warehouseService.TryUpsertWarehouseAsync(warehouseUpsertRequestModel);
            if (!result.IsSuccess)
            {
                var messageAction = warehouseUpsertRequestModel.Id != Guid.Empty ? "updating" : "adding";
                WeakReferenceMessenger.Default.Send(new SendToastrMessage($"There was a issue with {messageAction} the warehouse.", typeof(WarehouseUpsertViewModel)));
                IsBusy = false;
                return;
            }
            await Shell.Current.GoToAsync("..");
            WeakReferenceMessenger.Default.Send(new SendWarehousesChangedMessage());

            if (Warehouse.Id != null && Warehouse.Id != Guid.Empty)
            {
                WeakReferenceMessenger.Default.Send(new SendWarehouseIdentifierMessage(Warehouse.Id.Value));
            }
        }
    }
}
