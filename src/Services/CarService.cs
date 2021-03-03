using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using car_pooling_api.Domain.Repositories;
using car_pooling_api.Mappers;
using IO.Swagger.Models;

namespace car_pooling_api.Services {
    public class CarService {
        private readonly ICarRepository carRepository;
        public CarService(ICarRepository repository)
        {
            carRepository = repository;
        }

        public async Task InitCarDatabase(List<CarDto> cars) {
            carRepository.unitOfWork.DeleteAll();
            var carsToInsert = cars.Select(c => c.ToDomain());
            await carRepository.InsertRangeAsync(carsToInsert);
            carRepository.unitOfWork.Commit();
        }
    }
}