using Mde.Project.Api.Core.Data;
using Mde.Project.Api.Core.Entities.Products;
using Mde.Project.Api.Core.Services.Base;
using Mde.Project.Api.Core.Services.Interfaces;
using Mde.Project.Api.Core.Services.Interfaces.Products;
using Mde.Project.Api.Core.Services.Products;
using System.Text;

namespace Mde.Project.Api.Core.Services
{
    public class ProductService : CrudService<Product>, IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductBrandService _brands;
        private readonly IProductCategoryService _categories;
        private readonly IProductTaxService _taxes;
        public IProductBrandService Brands => _brands ?? new ProductBrandService(_context);
        public IProductCategoryService Categories => _categories ?? new ProductCategoryService(_context);
        public IProductTaxService Taxes => _taxes ?? new ProductTaxService(_context);
        public ProductService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<string> GenerateBarcodeAsync()
        {
            var random = new Random();
            while (true)
            {
                var codeBuilder = new StringBuilder();
                for (int i = 0; i < 12; i++)
                {
                    codeBuilder.Append(random.Next(0, 10));
                }

                var code = codeBuilder.ToString();
                var checkDigit = CalculateCheckDigit(code);
                var barcode = code + checkDigit;

                var barcodeExists = await ExistsAsync(p => p.Barcode == barcode);
                if (!barcodeExists.Data)
                {
                    return barcode;
                }
            }
        }
        private int CalculateCheckDigit(string code)
        {
            int sum = 0;
            for (int i = 0; i < code.Length; i++)
            {
                int digit = int.Parse(code[i].ToString());
                if (i % 2 == 0)
                {
                    sum += digit;
                }
                else
                {
                    sum += digit * 3;
                }
            }
            int remainder = sum % 10;
            return remainder == 0 ? 0 : 10 - remainder;
        }
    }
}
