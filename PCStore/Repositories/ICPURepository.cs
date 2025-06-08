using PCStore.Data;
using PCStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PCStore.Repositories
{
    public interface ICPURepository
    {
        Task<IEnumerable<CPU>> GetAllCPUsAsync();
        Task<CPU?> GetCPUByIdAsync(int id);
        Task AddCPUAsync(CPU cpu);
        Task UpdateCPUAsync(CPU cpu);
        Task DeleteCPUAsync(int id);
    }

    public class CPURepository : ICPURepository
    {
        private readonly PcShopContext _context;

        public CPURepository(PcShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CPU>> GetAllCPUsAsync() =>
            await _context.CPUs.ToListAsync();

        public async Task<CPU?> GetCPUByIdAsync(int id) =>
            await _context.CPUs.FirstOrDefaultAsync(c => c.Id == id);

        public async Task AddCPUAsync(CPU cpu)
        {
            await _context.CPUs.AddAsync(cpu);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCPUAsync(CPU cpu)
        {
            _context.CPUs.Update(cpu);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCPUAsync(int id)
        {
            var cpu = await GetCPUByIdAsync(id);
            if (cpu != null)
            {
                _context.CPUs.Remove(cpu);
                await _context.SaveChangesAsync();
            }
        }
    }
}