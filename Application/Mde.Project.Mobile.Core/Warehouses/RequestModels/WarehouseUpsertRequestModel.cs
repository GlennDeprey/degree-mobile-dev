using Mde.Project.Mobile.Core.Models.Google;
using Mde.Project.Mobile.Core.Models.Warehouses;

namespace Mde.Project.Mobile.Core.Warehouses.RequestModels
{
    public class WarehouseUpsertRequestModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Phone { get; set; }
        public WarehouseLocation Location { get; set; }
        public GooglePlaceInfo GoogleInfo { get; set; }
    }
}
