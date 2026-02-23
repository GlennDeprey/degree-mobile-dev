using Mde.Project.Api.Dtos.Products;
using Mde.Project.Api.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Mde.Project.Api.Controllers
{
    [Route("api/taxes")]
    [ApiController]
    public class TaxesController : ControllerBase
    {
        private readonly IProductService _productService;
        public TaxesController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<TaxesListDto>> GetTaxesAsync()
        {
            var taxRatesList = new TaxesListDto();

            var taxes = await _productService.Taxes.GetAllAsync();
            if (taxes.Success)
            {
                taxRatesList.Items = taxes.Data.Select(p => new TaxDto()
                {
                    Id = p.Id,
                    Name = p.Name,
                    TaxRate = p.TaxRate,
                });

                return Ok(taxRatesList);
            }
            return NotFound(taxes.Errors);

        }
    }
}
