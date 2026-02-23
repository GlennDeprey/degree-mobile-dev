using Mde.Project.Api.Core.Entities.Warehouses;

namespace Mde.Project.Api.Dtos.Warehouses
{
    public class WarehouseGoogleDto
    {
        public string GoogleAddress { get; set; }
        public string GoogleAddressId { get; set; }
        public ICollection<WarehousePhoto> GooglePhotoUris { get; set; }
    }
}
