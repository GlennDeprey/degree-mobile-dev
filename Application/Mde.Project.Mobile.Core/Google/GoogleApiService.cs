using Mde.Project.Mobile.Core.Google.Interface;
using System.Net.Http.Json;
using System.Text.Json;
using Mde.Project.Mobile.Core.Google.RequestModels;
using Mde.Project.Mobile.Core.Models.Google;
using Mde.Project.Mobile.Core.ResultModels;

namespace Mde.Project.Mobile.Core.Google
{
    public class GoogleApiService : IGoogleApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public GoogleApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CollectionResultModel<GooglePlaceSuggestion>> GetAutoCompletePlacesAsync(string input)
        {
            var client = _httpClientFactory.CreateClient(Constants.GooglePlaceClientName);
            client.BaseAddress = new Uri("https://places.googleapis.com/v1/places:autocomplete");

            var requestModel = new GoogleSuggestionRequestModel
            {
                Input = input,
                IncludedRegionCodes = new List<string> { "be", "nl", "fr" }
            };

            var filter = "suggestions.placePrediction.placeId%2Csuggestions.placePrediction.text.text";
            var requestUri = $"?fields={filter}&key={Constants.GoogleApiKey}";

            var response = await client.PostAsJsonAsync(requestUri, requestModel);

            var googleSuggestionsResultModel = new CollectionResultModel<GooglePlaceSuggestion>();
            if (!response.IsSuccessStatusCode)
            {
                googleSuggestionsResultModel.Message = "Failed to retrieve suggestions.";
                return googleSuggestionsResultModel;
            }

            var content = await response.Content.ReadAsStringAsync();
            using (var document = JsonDocument.Parse(content))
            {
                var root = document.RootElement;
                if (root.TryGetProperty("suggestions", out var suggestionsElement) && suggestionsElement.ValueKind == JsonValueKind.Array && suggestionsElement.GetArrayLength() > 0)
                {
                    var suggestions = suggestionsElement.EnumerateArray();
                    googleSuggestionsResultModel.Items = suggestions.Select(s => new GooglePlaceSuggestion
                    {
                        PlaceId = s.GetProperty("placePrediction").GetProperty("placeId").GetString(),
                        Name = s.GetProperty("placePrediction").GetProperty("text").GetProperty("text").GetString()
                    }).ToList();
                }
                else
                {
                    googleSuggestionsResultModel.Message = "No suggestions found.";
                }
            }

            return googleSuggestionsResultModel;
        }

        public async Task<ResultModel<GooglePlaceDetails>> GetPlaceDetailsAsync(string placeId)
        {
            var client = _httpClientFactory.CreateClient(Constants.GooglePlaceClientName);

            var fields = "addressComponents,location,photos.name";
            var response = await client.GetAsync($"places/{placeId}?fields={fields}&key={Constants.GoogleApiKey}");
            var googlePlaceLocationResultModel = new ResultModel<GooglePlaceDetails>();
            if (!response.IsSuccessStatusCode)
            {
                googlePlaceLocationResultModel.Message = "Failed to retrieve place details.";
                return googlePlaceLocationResultModel;
            }


            var content = await response.Content.ReadAsStringAsync();
            using (var document = JsonDocument.Parse(content))
            {
                var root = document.RootElement;

                var street = string.Empty;
                var streetNumber = string.Empty;
                var city = string.Empty;
                var state = string.Empty;
                var state2 = string.Empty;
                var country = string.Empty;
                var postalCode = string.Empty;
                var placePhotos = new List<GooglePlacePhoto>();

                if (root.TryGetProperty("photos", out var photosElement) && photosElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var photo in photosElement.EnumerateArray())
                    {
                        var photoName = photo.GetProperty("name").GetString();
                        var photolink = await GetPlacePhotoAsync(photoName);
                        if (photolink.IsSuccess)
                        {
                            placePhotos.Add(new GooglePlacePhoto
                            {
                                PhotoUri = photolink.Data
                            });
                        }
                    }
                }

                // Extract address components
                var addressComponents = root.GetProperty("addressComponents").EnumerateArray();
                foreach (var component in addressComponents)
                {
                    var longText = component.GetProperty("longText").GetString();
                    var types = component.GetProperty("types").EnumerateArray().Select(t => t.GetString()).ToList();

                    if (types.Contains("route"))
                    {
                        street = longText;
                    }
                    else if (types.Contains("street_number"))
                    {
                        streetNumber = longText;
                    }
                    else if (types.Contains("sublocality_level_1") || types.Contains("sublocality"))
                    {
                        city = longText;
                    }
                    else if (types.Contains("administrative_area_level_2"))
                    {
                        state = longText;
                    }
                    else if (types.Contains("administrative_area_level_1"))
                    {
                        state2 = longText;
                    }
                    else if (types.Contains("country"))
                    {
                        country = longText;
                    }
                    else if (types.Contains("postal_code"))
                    {
                        postalCode = longText;
                    }
                }

                googlePlaceLocationResultModel = new ResultModel<GooglePlaceDetails>
                {
                    Data = new GooglePlaceDetails
                    {
                        Address = $"{street} {streetNumber}".Trim(),
                        City = city,
                        State = $"{state}{(string.IsNullOrWhiteSpace(state2) ? "" : $" - {state2}")}".Trim(),
                        Country = country,
                        PostalCode = postalCode,
                        Latitude = root.GetProperty("location").GetProperty("latitude").GetDouble(),
                        Longitude = root.GetProperty("location").GetProperty("longitude").GetDouble(),
                        PhotoUris = placePhotos
                    }
                };
            }

            return googlePlaceLocationResultModel;
        }

        private async Task<ResultModel<string>> GetPlacePhotoAsync(string placePhotoUri, int maxHeight = 120, int maxWidth = 120)
        {
            var client = _httpClientFactory.CreateClient(Constants.GooglePlaceClientName);

            var response = await client.GetAsync($"{placePhotoUri}/media?maxHeightPx={maxHeight}&maxWidthPx={maxWidth}&skipHttpRedirect=true&key={Constants.GoogleApiKey}");
            var googlePlacePhotoUriResultModel = new ResultModel<string>();
            if (!response.IsSuccessStatusCode)
            {
                googlePlacePhotoUriResultModel.Message = "Failed to retrieve place picture url.";
                return googlePlacePhotoUriResultModel;
            }

            var content = await response.Content.ReadAsStringAsync();
            using (var document = JsonDocument.Parse(content))
            {
                var root = document.RootElement;
                var imageUri = root.GetProperty("photoUri");
                googlePlacePhotoUriResultModel.Data = imageUri.GetString();
            }

            return googlePlacePhotoUriResultModel;
        }
    }
}
