using System.Collections.Generic;
using car.pooling.ComponentTest.ApiClient;

namespace car_pooling.ComponentTest.Mothers {

    public static class GroupMother {
        private static int id = 1;
        internal static GroupDto GetGroup(int people) {
            return new GroupDto {
                Id = id++,
                People = people
            };
            
        }

        internal static void ResetId() {
            id = 1;
        }


    }
    
}