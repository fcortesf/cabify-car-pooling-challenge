using System.Collections.Generic;
using car.pooling.ComponentTest.ApiClient;

namespace car_pooling.ComponentTest.Mothers {

    public static class CarsMother {
        internal static List<CarDto> GetOneCarList(int seats) {
            return new List<CarDto> {
                new CarDto {
                    Id = 1,
                    Seats = seats
                }
            };
        }
    }
    
}