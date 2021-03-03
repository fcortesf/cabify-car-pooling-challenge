using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using car_pooling_api.Domain.Repositories;

namespace car_pooling_api.Services {
    public class PoolService : IPoolService {
        private readonly IGroupRepository groupRepository;
        private readonly ICarRepository carRepository;
        public PoolService(IGroupRepository groupRepo, ICarRepository carRepo)
        {
            groupRepository = groupRepo;
            carRepository = carRepo;
        }

        public async Task PoolProcessAsync() {
            
            var groups = await groupRepository.GetUnsagginedsAsync();
            foreach (var group in groups)
            {
                var car = await carRepository.GetCarWithEmptySeatsAsync(group.People);
                if (car != null) {
                    await groupRepository.SaveJourneyAsync(group, car);
                }
            }
          
        }

        public async Task<bool> DropoffAsync(int groupId) {
            var group = await groupRepository.GetAsync(groupId);
            if (group == null) return false;
            if (group.CarId != null) {
                var car = await carRepository.GetAsync(group.CarId.Value);
                car.GroupsAssigned.Remove(car.GroupsAssigned.FirstOrDefault(g => g.Id == groupId));
                await carRepository.UpdateCarAsync(car);
            }
            await groupRepository.DeleteAsync(groupId);
            return true;
        }
    }
}