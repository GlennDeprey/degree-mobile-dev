using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using Mde.Project.Mobile.Core.Warehouses.Interface;
using Mde.Project.Mobile.Messages;
using Mde.Project.Mobile.Messages.Maps;
using Mde.Project.Mobile.Messages.Products;
using Mde.Project.Mobile.Messages.Warehouses;
using Mde.Project.Mobile.Models.Warehouse;
using Mde.Project.Mobile.ViewModels.Base;
using System;
using Mde.Project.Mobile.Core;
using CommunityToolkit.Maui.Storage;
using Mde.Project.Mobile.Core.Items.Interfaces;

namespace Mde.Project.Mobile.ViewModels.App.Warehouses
{
    public partial class WarehouseDetailViewModel : UserViewModel
    {
        [ObservableProperty]
        private WarehouseDetailModel _warehouse;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty] 
        private bool _hasLocation;

        [ObservableProperty] private string _imageUrl;

        private readonly IAuthenticationService _authenticationService;
        private readonly IWarehouseService _warehouseService;
        private readonly IWarehouseItemsService _warehouseItemsService;
        private Random _random;
        public WarehouseDetailViewModel(IAuthenticationService authenticationService, IWarehouseService warehouseService, IWarehouseItemsService warehouseItemsService) : base(authenticationService)
        {
            _authenticationService = authenticationService;
            _warehouseService = warehouseService;
            _warehouseItemsService = warehouseItemsService;
            Warehouse = new WarehouseDetailModel();
            
            WeakReferenceMessenger.Default.Register<SendWarehouseIdentifierMessage>(this, async (r, m) =>
            {
                _random = new Random();
                await InitializeWarehouseCommand.ExecuteAsync(m.Id);
            });
        }

        [RelayCommand]
        public async Task WarehouseEditNavigationAsync()
        {
            await Shell.Current.GoToAsync(MauiRoutes.WarehouseUpsert);
            WeakReferenceMessenger.Default.Send(new SendWarehouseDetailMessage(Warehouse));
        }

        [RelayCommand]
        public async Task WarehouseDeleteNavigationAsync()
        {
            await Shell.Current.GoToAsync(MauiRoutes.WarehouseDelete);
            WeakReferenceMessenger.Default.Send(new SendWarehouseDeleteMessage(Warehouse.Id.Value, Warehouse.Name));
        }

        [RelayCommand]
        public async Task WarehouseMapsNavigationAsync()
        {
#if ANDROID
            await Shell.Current.GoToAsync(MauiRoutes.GoogleMaps);

#elif WINDOWS
            await Shell.Current.GoToAsync(MauiRoutes.GoogleMapsFrame);
#endif

            WeakReferenceMessenger.Default.Send(new SendLocationMessage(Warehouse.Name, Warehouse.GoogleInfo.GoogleAddress, Warehouse.Location.Latitude, Warehouse.Location.Longitude, Warehouse.GoogleInfo.GoogleAddressId));
        }

        [RelayCommand]
        public async Task InitializeWarehouseAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                await Shell.Current.GoToAsync("..");
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("There was no correct warehouse id.", typeof(WarehouseDetailViewModel)));
                return;
            }

            IsBusy = true;
            var warehouse = await _warehouseService.TryGetWarehouseByIdAsync(id);
            if (!warehouse.IsSuccess)
            {
                await Shell.Current.GoToAsync("..");
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("The given warehouse was not found.", typeof(WarehouseDetailViewModel)));
                return;
            }

            Warehouse = new WarehouseDetailModel()
            {
                Id = warehouse.Data.Id,
                Name = warehouse.Data.Name,
                ShortName = warehouse.Data.ShortName,
                Phone = warehouse.Data.Phone,
                Location = new WarehouseLocationModel
                {
                    Id = warehouse.Data.Location.Id,
                    Address = warehouse.Data.Location.Address,
                    City = warehouse.Data.Location.City,
                    State = warehouse.Data.Location.State,
                    Country = warehouse.Data.Location.Country,
                    PostalCode = warehouse.Data.Location.PostalCode,
                    Longitude = warehouse.Data.Location.Longitude,
                    Latitude = warehouse.Data.Location.Latitude,
                    
                },
                GoogleInfo = new WarehouseGoogleModel
                {
                    GoogleAddress = warehouse.Data.GoogleInfo.GoogleAddress,
                    GoogleAddressId = warehouse.Data.GoogleInfo.GoogleAddressId,
                    GooglePhotoUris = warehouse.Data.GoogleInfo.GooglePhotoUris.Select(photo => new WarehousePhotoModel
                    {
                        PhotoUri = photo.PhotoUri,
                    }).ToList()
                },
                Stock = new WarehouseStockModel
                {
                    TotalItems = warehouse.Data.Stock.TotalItems,
                    LowestItemPrice = warehouse.Data.Stock.LowestItemPrice,
                    HighestItemPrice = warehouse.Data.Stock.HighestItemPrice,
                    AverageItemPrice = warehouse.Data.Stock.AverageItemPrice
                },
                Earnings = warehouse.Data.Earnings
            };

            HasLocation = !string.IsNullOrWhiteSpace(Warehouse.GoogleInfo.GoogleAddressId);
            if (!Warehouse.GoogleInfo.GooglePhotoUris.Any())
            {
                ImageUrl = Constants.NoImageUri;
                IsBusy = false;
                return;
            }

            var photoCount = Warehouse.GoogleInfo.GooglePhotoUris.Count();
            var randomImageIndex = _random.Next(0, photoCount);

            var images = Warehouse.GoogleInfo.GooglePhotoUris.ToList();
            ImageUrl = images[randomImageIndex].PhotoUri;

            IsBusy = false;
        }

        [RelayCommand]
        public async Task WarehouseProductsPdfDownloadAsync()
        {
            if (!Warehouse.Id.HasValue)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("There is no correct Warehouse id.", typeof(WarehouseDetailViewModel)));
                return;
            }

            var result = await _warehouseItemsService.TryGetWarehouseProductsPdf(Warehouse.Id.Value);
            if (!result.IsSuccess)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(result.Message, typeof(WarehouseDetailViewModel)));
                return;
            }

            var fileSaver = FileSaver.Default;
            string safeName = string.Join("_", Warehouse.Name.Split(Path.GetInvalidFileNameChars()));
            var fileName = $"{safeName}.pdf";

            var stream = new MemoryStream(result.Data);
            var response = await fileSaver.SaveAsync(fileName, stream, new CancellationToken());

            if (response.IsSuccessful)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("Warehouse Products barcode pdf successfully saved.", typeof(WarehouseDetailViewModel)));

                // This does not work on android due to permissions, so i had to set it to windows only.
#if WINDOWS
                await Launcher.Default.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(response.FilePath)
                });
#endif
            }
            else
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("Could not save PDF", typeof(WarehouseDetailViewModel)));
            }
        }
    }
}
