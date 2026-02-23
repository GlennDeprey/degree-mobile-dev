using Mde.Project.Api.Core.Data;
using Mde.Project.Api.Core.Entities.Products;
using Mde.Project.Api.Core.Services.Base;
using Mde.Project.Api.Core.Services.Interfaces.Products;

namespace Mde.Project.Api.Core.Services.Products
{
    public class ProductTaxService : CrudService<ProductTax>, IProductTaxService
    {
        private readonly ApplicationDbContext _context;
        public ProductTaxService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
