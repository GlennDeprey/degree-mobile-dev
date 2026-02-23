using Mde.Project.Api.Core.Entities.Products;
using Mde.Project.Api.Core.Entities.Warehouses;

namespace Mde.Project.Api.Core.Entities.Reports
{
    public class Report : BaseEntity
    {
        public Guid? WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        public Guid? ProductId { get; set; }
        public Product Product { get; set; }
        public int QuantityChange { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
    }
}
