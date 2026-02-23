using System.Text.Json.Serialization;

namespace Mde.Project.Mobile.Core.Google.RequestModels
{
    public class GoogleSuggestionRequestModel
    {
        [JsonPropertyName("input")]
        public string Input { get; set; }

        [JsonPropertyName("includedRegionCodes")]
        public List<string> IncludedRegionCodes { get; set; }
    }
}
