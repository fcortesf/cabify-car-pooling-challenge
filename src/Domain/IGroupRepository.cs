using System.Collections.Generic;
using System.Threading.Tasks;

namespace car_pooling_api.Domain.Repositories {
    public interface IGroupRepository {
        IUnitOfWork unitOfWork { get; }
        Task<Group> InsertAsync(Group groupToSave);
        Task<Group> GetAsync(int id);
        Task<IEnumerable<Group>> GetUnsagginedsAsync();
        Task<Group> SaveJourneyAsync(Group group,Car car);
        Task DeleteAsync(int groupId);
    }
}