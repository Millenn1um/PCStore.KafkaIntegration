using PCStore.Data;
using PCStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PCStore.Repositories
{
    public interface IGPURepository
    {
        Task<IEnumerable<GPU>> GetAllGPUsAsync();
        Task<GPU?> GetGPUByIdAsync(int id);
        Task AddGPUAsync(GPU gpu);
        Task UpdateGPUAsync(GPU gpu);
        Task DeleteGPUAsync(int id);
    }

    public class GPURepository : IGPURepository
    {
        private readonly PcShopContext _context;

        public GPURepository(PcShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GPU>> GetAllGPUsAsync() =>
            await _context.GPUs.ToListAsync();

        public async Task<GPU?> GetGPUByIdAsync(int id) =>
            await _context.GPUs.FirstOrDefaultAsync(g => g.Id == id);

        public async Task AddGPUAsync(GPU gpu)
        {
            await _context.GPUs.AddAsync(gpu);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateGPUAsync(GPU gpu)
        {
            _context.GPUs.Update(gpu);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGPUAsync(int id)
        {
            var gpu = await GetGPUByIdAsync(id);
            if (gpu != null)
            {
                _context.GPUs.Remove(gpu);
                await _context.SaveChangesAsync();
            }
        }
    }
}