using System.Collections.Generic;
using System.Threading.Tasks;
using car_pooling_api.Domain;
using car_pooling_api.Domain.Repositories;
using car_pooling_api.Mappers;
using car_pooling_api.Services;
using IO.Swagger.Models;
using Moq;
using Xunit;

namespace car_pooling.UnitTest.Services
{
    public class GroupServiceTest {

        [Fact]
        public async void JourneyTest(){
            // Arrange
            var groupRepositoryMock = new Mock<IGroupRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();  
            var poolServiceMock = new Mock<IPoolService>();
            groupRepositoryMock.Setup(r => r.unitOfWork).Returns(unitOfWorkMock.Object);
            var groupService = new GroupService(groupRepositoryMock.Object, poolServiceMock.Object);
            var groupToInsert = new GroupDto { Id = 1, People = 1};            

            // Act
            await groupService.Journey(groupToInsert);

            // Assert
            groupRepositoryMock.Verify(r => r.unitOfWork.Commit(), Times.Exactly(2));
            poolServiceMock.Verify(s => s.PoolProcessAsync(), Times.Exactly(1));
        }

        [Theory]
        [InlineData(1)]
        public async void DropoffExist(int value) {
             // Arrange
            var groupRepositoryMock = new Mock<IGroupRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();  
            var poolServiceMock = new Mock<IPoolService>();
            poolServiceMock.Setup(s => s.DropoffAsync(value)).Returns(Task.FromResult(true));
            groupRepositoryMock.Setup(r => r.unitOfWork).Returns(unitOfWorkMock.Object);
            var groupService = new GroupService(groupRepositoryMock.Object, poolServiceMock.Object);
            var groupToInsert = new GroupDto { Id = 1, People = 1};
            
            // Act
            await groupService.Dropoff(value);

            // Assert
            groupRepositoryMock.Verify(r => r.unitOfWork.Commit(), Times.Exactly(2));
            poolServiceMock.Verify(s => s.PoolProcessAsync(), Times.Exactly(1));
            poolServiceMock.Verify(s => s.DropoffAsync(value), Times.Exactly(1));
        }

        [Theory]
        [InlineData(1)]
        public async void DropoffNotExist(int value) {
             // Arrange
            var groupRepositoryMock = new Mock<IGroupRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();  
            var poolServiceMock = new Mock<IPoolService>();
            poolServiceMock.Setup(s => s.DropoffAsync(value)).Returns(Task.FromResult(false));
            groupRepositoryMock.Setup(r => r.unitOfWork).Returns(unitOfWorkMock.Object);
            var groupService = new GroupService(groupRepositoryMock.Object, poolServiceMock.Object);
            var groupToInsert = new GroupDto { Id = 1, People = 1};
            
            // Act
            await groupService.Dropoff(value);

            // Assert
            groupRepositoryMock.Verify(r => r.unitOfWork.Commit(), Times.Exactly(0));
            poolServiceMock.Verify(s => s.PoolProcessAsync(), Times.Exactly(0));
            poolServiceMock.Verify(s => s.DropoffAsync(value), Times.Exactly(1));
        }

        private GroupService GetServiceToLocateTests(int id, Group returnedGroup) {
            var groupRepositoryMock = new Mock<IGroupRepository>();            
            groupRepositoryMock.Setup(r => r.GetAsync(id)).Returns(Task.FromResult(returnedGroup));
            return new GroupService(groupRepositoryMock.Object, null);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public async void LocateExistsWithCar(int value){
            // Arrange
            Group returnedGroup = new Group { 
                Id = value, 
                People = 1, 
                JourneyCar = new Car {
                    Id = 1,
                    Seats = 4
                }
            };
            var service = GetServiceToLocateTests(value, returnedGroup);

            // Act
            var car = await service.Locate(value);

            // Assert
            Assert.NotNull(car);
            Assert.Equal(1, car.Id);
            
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public async void LocateExistsWithoutCar(int value){
            // Arrange
            Group returnedGroup = new Group { 
                Id = value, 
                People = 1,
            };
            var service = GetServiceToLocateTests(value, returnedGroup);

            // Act
            var car = await service.Locate(value);

            // Assert
            Assert.Null(car);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public async void LocateNotExists(int value){
            // Arrange
            var service = GetServiceToLocateTests(value, null);

            // Act
            var car = await service.Locate(value);

            // Assert
            Assert.NotNull(car);
            Assert.Equal(-1, car.Id);
        }

    }
}