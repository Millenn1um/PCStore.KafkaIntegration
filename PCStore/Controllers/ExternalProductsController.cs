using PCStore.Models;
using Microsoft.AspNetCore.Mvc;
using PCStore.Services;

namespace PCStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExternalProductsController : ControllerBase
    {
        private readonly ExternalProductService _externalProductService;

        public ExternalProductsController(ExternalProductService externalProductService)
        {
            _externalProductService = externalProductService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _externalProductService.GetProductsFromExternalServiceAsync();
            return Ok(products);
        }
    }
}