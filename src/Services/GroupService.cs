using System.Threading.Tasks;
using car_pooling_api.Domain.Repositories;
using car_pooling_api.Mappers;
using IO.Swagger.Models;

namespace car_pooling_api.Services {
    public class GroupService {
        private readonly IGroupRepository groupRepository;
        private readonly IPoolService poolService;
        public GroupService(IGroupRepository repository, IPoolService carPoolService)
        {
            groupRepository = repository;
            poolService = carPoolService;
        }

        public async Task<bool> Journey(GroupDto group) {
            var dbGroup = await groupRepository.GetAsync(group.Id);
            if (dbGroup != null) return false;
            await groupRepository.InsertAsync(group.ToDomain());
            groupRepository.unitOfWork.Commit();   
            await poolService.PoolProcessAsync();
            groupRepository.unitOfWork.Commit(); 
            return true;           
        }

        public async Task<bool> Dropoff(int groupId) {
            var result = await poolService.DropoffAsync(groupId);
            if (result) {                
                groupRepository.unitOfWork.Commit();   
                await poolService.PoolProcessAsync();
                groupRepository.unitOfWork.Commit();
            }
            return result;
        }
        
        public async Task<CarDto> Locate(int groupId) {
            var group = await groupRepository.GetAsync(groupId);
            if (group != null) {
                return group.JourneyCar?.ToDto();
            }
            else {
                return new CarDto {
                    Id = -1,
                    Seats = -1
                };
            }
        }
    }
}