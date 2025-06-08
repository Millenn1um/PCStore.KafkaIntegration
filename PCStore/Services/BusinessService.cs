using System.Linq;
using System.Threading.Tasks;
using PCStore.Services;

namespace PCStore.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly IProductService _productService;

        public BusinessService(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<decimal> GetTotalInventoryValueAsync()
        {
            var cpus = await _productService.GetCPUsAsync();
            var gpus = await _productService.GetGPUsAsync();
            var cpuValue = cpus.Sum(cpu => cpu.Price);
            var gpuValue = gpus.Sum(gpu => gpu.Price);
            return cpuValue + gpuValue;
        }

        public async Task<int> GetTotalProductsCountAsync()
        {
            var cpus = await _productService.GetCPUsAsync();
            var gpus = await _productService.GetGPUsAsync();
            return cpus.Count() + gpus.Count();
        }
    }
}