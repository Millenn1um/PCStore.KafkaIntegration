using Microsoft.AspNetCore.Mvc;
using PCStore.Services;
using System.Threading.Tasks;

namespace PCStore.Controllers
{
    [Route("api/business")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessService _businessService;

        public BusinessController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        [HttpGet("total-inventory-value")]
        public async Task<IActionResult> GetTotalInventoryValue()
        {
            var totalInventoryValue = await _businessService.GetTotalInventoryValueAsync();
            return Ok(new { TotalInventoryValue = totalInventoryValue });
        }

        [HttpGet("total-products-count")]
        public async Task<IActionResult> GetTotalProductsCount()
        {
            var totalProductsCount = await _businessService.GetTotalProductsCountAsync();
            return Ok(new { TotalProductsCount = totalProductsCount });
        }
    }
}