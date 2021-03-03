using System.Collections.Generic;
using car_pooling_api.Domain;

namespace car_pooling.UnitTest.Repositories
{
    public static class CarsMother {
        internal static List<Car> GetCars() {
            return new List<Car> {
                new Car {
                    Id = 1,
                    Seats = 4
                },
                new Car {
                    Id = 2,
                    Seats = 5
                },
                new Car {
                    Id = 3,
                    Seats = 5
                },
                new Car {
                    Id = 4,
                    Seats = 6
                }
            };
        }
    }
}