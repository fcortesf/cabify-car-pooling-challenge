using System.Collections.Generic;
using System.Threading.Tasks;

namespace car_pooling_api.Domain.Repositories {
    public interface ICarRepository {
        IUnitOfWork unitOfWork { get; }
        Task<Car> InsertAsync(Car carToSave);
        Task InsertRangeAsync(IEnumerable<Car> rangeCar);
        Task<Car> GetCarWithEmptySeatsAsync(int minimunEmpty);
        Task<Car> GetAsync(int id);
        Task<Car> UpdateCarAsync(Car car);
    }
}