using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Restaurant_Management_System.HealthChecks
{
    public class MemoryCacheHealthCheck:IHealthCheck
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheHealthCheck(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var testKey = "health_check_test";
                _cache.Set(testKey, DateTime.UtcNow, TimeSpan.FromSeconds(1));
                var value = _cache.Get(testKey);

                if (value != null)
                {
                    return Task.FromResult(HealthCheckResult.Healthy("Memory cache is working correctly."));
                }

                return Task.FromResult(HealthCheckResult.Unhealthy("Memory cache is not working correctly."));
            }
            catch (Exception ex)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("Memory cache health check failed.", ex));
            }
        }
    }
}
