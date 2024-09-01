using CarAuctionManagement.Application.Interfaces;
using CarAuctionManagement.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CarAuctionManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBidRepository, BidRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IAuctionRepository, AuctionRepository>();


            return services;
        }
    }
}