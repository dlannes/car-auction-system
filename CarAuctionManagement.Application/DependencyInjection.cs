using CarAuctionManagement.Application.Interfaces;
using CarAuctionManagement.Application.Mappings;
using CarAuctionManagement.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CarAuctionManagement.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<VehicleMapper>();
            services.AddTransient<IAuctionService, AuctionService>();

            return services;
        }
    }
}
