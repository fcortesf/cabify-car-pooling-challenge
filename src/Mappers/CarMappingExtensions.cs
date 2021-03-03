using car_pooling_api.Domain;
using IO.Swagger.Models;

namespace car_pooling_api.Mappers {
    public static class CarMappingExtensions {
        public static CarDto ToDto(this Car car) {
            return new CarDto {
                Id = car.Id,
                Seats = car.Seats
            };
        }

        public static Car ToDomain(this CarDto car) {
            return new Car {
                Id = car.Id,
                Seats = car.Seats
            };
        }
    }
}