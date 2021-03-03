using car_pooling_api.Domain;
using IO.Swagger.Models;

namespace car_pooling_api.Mappers {
    public static class GroupMappingExtensions {
        public static GroupDto ToDto(this Group group) {
            return new GroupDto {
                Id = group.Id,
                People = group.People
            };
        }

        public static Group ToDomain(this GroupDto group) {
            return new Group {
                Id = group.Id,
                People = group.People
            };
        }
    }
}