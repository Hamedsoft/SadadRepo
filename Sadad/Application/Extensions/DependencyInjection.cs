using Microsoft.Extensions.DependencyInjection;
using Contracts.Repositories;
using Contracts.Services;

namespace Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Application Services
            services.AddScoped<IOrderService, OrderService>();
            return services;
        }
    }
}
