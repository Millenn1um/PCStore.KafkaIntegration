using Microsoft.AspNetCore.Mvc;
using PCStore.Models;
using PCStore.Services;
using System.Threading.Tasks;

namespace PCStore.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("cpus")]
        public async Task<IActionResult> GetAllCPUs()
        {
            var cpus = await _productService.GetCPUsAsync();
            return Ok(cpus);
        }

        [HttpGet("gpus")]
        public async Task<IActionResult> GetAllGPUs()
        {
            var gpus = await _productService.GetGPUsAsync();
            return Ok(gpus);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost("cpus")]
        public async Task<IActionResult> AddCPU([FromBody] CPU cpu)
        {
            if (cpu == null)
                return BadRequest();
            await _productService.AddCPUAsync(cpu);
            return CreatedAtAction(nameof(GetProductById), new { id = cpu.Id }, cpu);
        }

        [HttpPost("gpus")]
        public async Task<IActionResult> AddGPU([FromBody] GPU gpu)
        {
            if (gpu == null)
                return BadRequest();
            await _productService.AddGPUAsync(gpu);
            return CreatedAtAction(nameof(GetProductById), new { id = gpu.Id }, gpu);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleted = await _productService.DeleteProductAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}