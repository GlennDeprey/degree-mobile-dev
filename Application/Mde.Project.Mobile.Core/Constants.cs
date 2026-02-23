using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Core
{
    public static class Constants
    {
        public const string ProjectApiUrl = "https://lg1svpzm-7041.euw.devtunnels.ms";
        public const string ProjectClientName = "apiClient";
        public const string GooglePlaceUrl = "https://places.googleapis.com/v1/";
        public const string GooglePlaceClientName = "googlePlaceClient";
        public const string TokenKey = "token";
        public const string AdminRole = "Admin";
        public const string CustomerRole = "Customer";

        public const string DefaultUploadRoot = $"{ProjectApiUrl}/uploads/";
        public const string NoImageUri = $"{DefaultUploadRoot}defaultimage.png";
        public const string DefaultImage = "defaultimage.png";

        // Should be a secret key
        public const string GoogleApiKey = "AIzaSyCEj8ZDoe3mkQVJrXs24zUhhfzBVd9qzFI";
    }
}
