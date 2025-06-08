using PCStore.Models;
using PCStore.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PCStore.Services
{
    public class ProductService : IProductService
    {
        private readonly ICPURepository _cpuRepository;
        private readonly IGPURepository _gpuRepository;

        public ProductService(ICPURepository cpuRepository, IGPURepository gpuRepository)
        {
            _cpuRepository = cpuRepository;
            _gpuRepository = gpuRepository;
        }

        public async Task<IEnumerable<CPU>> GetCPUsAsync() =>
            await _cpuRepository.GetAllCPUsAsync();

        public async Task<IEnumerable<GPU>> GetGPUsAsync() =>
            await _gpuRepository.GetAllGPUsAsync();

        public async Task<object?> GetProductByIdAsync(int id)
        {
            var cpu = await _cpuRepository.GetCPUByIdAsync(id);
            if (cpu != null) return cpu;

            var gpu = await _gpuRepository.GetGPUByIdAsync(id);
            return gpu;
        }

        public async Task AddCPUAsync(CPU cpu) =>
            await _cpuRepository.AddCPUAsync(cpu);

        public async Task AddGPUAsync(GPU gpu) =>
            await _gpuRepository.AddGPUAsync(gpu);

        public async Task<bool> DeleteProductAsync(int id)
        {
            var cpu = await _cpuRepository.GetCPUByIdAsync(id);
            if (cpu != null)
            {
                await _cpuRepository.DeleteCPUAsync(id);
                return true;
            }

            var gpu = await _gpuRepository.GetGPUByIdAsync(id);
            if (gpu != null)
            {
                await _gpuRepository.DeleteGPUAsync(id);
                return true;
            }

            return false;
        }
    }
}
