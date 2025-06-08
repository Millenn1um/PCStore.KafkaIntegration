using PCStore.Models;

namespace PCStore.Services
{
    public interface IProductService
    {
        Task<IEnumerable<CPU>> GetCPUsAsync();
        Task<IEnumerable<GPU>> GetGPUsAsync();
        Task<object?> GetProductByIdAsync(int id);
        Task AddCPUAsync(CPU cpu);
        Task AddGPUAsync(GPU gpu);
        Task<bool> DeleteProductAsync(int id);
    }
}