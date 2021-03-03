using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using car_pooling_api.Domain;
using car_pooling_api.Domain.Repositories;
using car_pooling_api.Services;
using Moq;
using Xunit;

namespace car_pooling.UnitTest.Services
{
    public class CarPoolingServiceTest
    {
        [Theory]
        [InlineData(500)]
        [InlineData(10)]
        [InlineData(1)]
        public async void DropoffGroupNotFound(int value)
        {
            // Arrange
            var groupRepositoryMock = new Mock<IGroupRepository>();
            groupRepositoryMock.Setup(c => c.GetAsync(value)).Returns(Task.FromResult((Group)null));
            var poolService = new PoolService(groupRepositoryMock.Object, null);

            // Act
            var result = await poolService.DropoffAsync(value);

            // Assert
            Assert.False(result, $"{value} id group should not be founded");
            groupRepositoryMock.Verify(c => c.GetAsync(value), Times.Exactly(1));
        }

        [Theory]
        [InlineData(500)]
        [InlineData(10)]
        [InlineData(1)]
        public async void DropoffGroupWithoutCar(int value)
        {
            // Arrange
            var groupRepositoryMock = new Mock<IGroupRepository>();
            groupRepositoryMock.Setup(c => c.GetAsync(value)).Returns(Task.FromResult(new Group {
                Id = value,
                People = 3
            }));
            groupRepositoryMock.Setup(c => c.DeleteAsync(value)).Returns(Task.FromResult(1));
            // Car repository should not be called
            var poolService = new PoolService(groupRepositoryMock.Object, null);

            // Act
            var result = await poolService.DropoffAsync(value);

            // Assert
            Assert.True(result, $"{value} id group should be founded and deleted");
            groupRepositoryMock.Verify(c => c.GetAsync(value), Times.Exactly(1));
            groupRepositoryMock.Verify(c => c.DeleteAsync(value), Times.Exactly(1));
        }

        [Theory]
        [InlineData(500)]
        [InlineData(10)]
        [InlineData(1)]
        public async void DropoffGroupWithCar(int value)
        {
            var group = new Group {
                Id = value,
                People = 3,
                CarId = 1
            };
            var carToDropOff = new Car {
                Id = 1,
                Seats = 6,
                GroupsAssigned = new List<Group>()
            };
            carToDropOff.GroupsAssigned.Add(group);
            group.JourneyCar = carToDropOff;
            // Arrange
            var groupRepositoryMock = new Mock<IGroupRepository>();
            groupRepositoryMock.Setup(c => c.GetAsync(value)).Returns(Task.FromResult(group));
            groupRepositoryMock.Setup(c => c.DeleteAsync(value)).Returns(Task.FromResult(1));
            var carRepositoryMock = new Mock<ICarRepository>();
            carRepositoryMock.Setup(c => c.GetAsync(1)).Returns(Task.FromResult(carToDropOff));
            carRepositoryMock.Setup(c => c.UpdateCarAsync(carToDropOff)).Returns(Task.FromResult(carToDropOff));

            // Car repository should not be called
            var poolService = new PoolService(groupRepositoryMock.Object, carRepositoryMock.Object);

            // Act
            var result = await poolService.DropoffAsync(value);

            // Assert
            Assert.True(result, $"{value} id group should be founded and deleted");
            Assert.Empty(carToDropOff.GroupsAssigned);
            groupRepositoryMock.Verify(c => c.GetAsync(value), Times.Exactly(1));
            groupRepositoryMock.Verify(c => c.DeleteAsync(value), Times.Exactly(1));
            carRepositoryMock.Verify(c => c.GetAsync(1), Times.Exactly(1));
            carRepositoryMock.Verify(c => c.UpdateCarAsync(carToDropOff), Times.Exactly(1));            

        }

    }
}
