using Mde.Project.Mobile.Core.Models.Google;
using Mde.Project.Mobile.Core.ResultModels;

namespace Mde.Project.Mobile.Core.Google.Interface;

public interface IGoogleApiService
{
    Task<CollectionResultModel<GooglePlaceSuggestion>> GetAutoCompletePlacesAsync(string input);
    Task<ResultModel<GooglePlaceDetails>> GetPlaceDetailsAsync(string placeId);
}