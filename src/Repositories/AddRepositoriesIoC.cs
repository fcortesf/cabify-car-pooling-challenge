using car_pooling_api.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace car_pooling_api.Repositories { 
    public static class AddRepositoriesIoC {
        public static void AddEntityFrameworkRepositories(this IServiceCollection services) {
            services.AddTransient<ICarRepository, CarRepository>();
            services.AddTransient<IGroupRepository, GroupRepository>();
        }
    }
}
