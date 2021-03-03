using System.Threading.Tasks;
using car.pooling.ComponentTest.ApiClient;

namespace car_pooling.ComponentTest.FunctionalTests
{
    public interface IFunctionalTest {
        Task<bool> ExecTest(CarPoolingApiClient apiClient);
    }
}