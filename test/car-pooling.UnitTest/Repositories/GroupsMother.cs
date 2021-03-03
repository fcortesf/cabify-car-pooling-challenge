using System.Collections.Generic;
using car_pooling_api.Domain;

namespace car_pooling.UnitTest.Repositories
{
    public static class GroupsMother {
        internal static List<Group> GetGroups(int? carId) {
            return new List<Group> {
                new Group {
                    Id = 1,
                    People = 1,
                    CarId = carId
                },
                new Group {
                    Id = 2,
                    People = 3,
                    CarId = carId
                },
                new Group {
                    Id = 3,
                    People = 5,
                    CarId = carId
                },
                new Group {
                    Id = 4,
                    People = 6,
                    CarId = carId
                }
            };
        }
    }
}