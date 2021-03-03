using System.Threading.Tasks;

namespace car_pooling_api.Services
{
    public interface IPoolService
    {
        Task<bool> DropoffAsync(int groupId);
        Task PoolProcessAsync();
    }
}