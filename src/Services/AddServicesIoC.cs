using Microsoft.Extensions.DependencyInjection;

namespace car_pooling_api.Services {
     public static class AddServicesIoC {
        public static void AddServices(this IServiceCollection services) {
            services.AddTransient<CarService>();
            services.AddTransient<GroupService>();
            services.AddTransient<IPoolService, PoolService>();
        }
    }
}