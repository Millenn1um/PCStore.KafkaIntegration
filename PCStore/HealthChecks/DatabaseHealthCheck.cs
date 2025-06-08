using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using PCStore.Data;
using System.Threading;
using System.Threading.Tasks;

namespace PCStore.HealthChecks
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly PcShopContext _context;

        public DatabaseHealthCheck(PcShopContext context) => _context = context;

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return await _context.Database.CanConnectAsync(cancellationToken)
                ? HealthCheckResult.Healthy("Database is accessible.")
                : HealthCheckResult.Unhealthy("Database connection failed.");
        }
    }
}
