using System;
using car_pooling_api.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace car_pooling.UnitTest.Repositories
{
    public class CarRepositoryTest
    {
        private CarPoolingDbContext getContext() {
            var options = new DbContextOptionsBuilder<CarPoolingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            return new CarPoolingDbContext(options);
        }

        private CarPoolingDbContext loadData() {
            var context = getContext();
            context.Car.AddRange(CarsMother.GetCars());
            context.Group.AddRange(GroupsMother.GetGroups(1));
            context.SaveChanges();
            return context;
        }

        private CarRepository getRepository() {
            var contextData = loadData();
            return new CarRepository(contextData);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(1)]
        [InlineData(4)]
        public async void GetExist(int value) {
            // Arrange
            var repository = getRepository();

            // Act
            var car = await repository.GetAsync(value);

            // Assert
            Assert.NotNull(car);
            Assert.Equal(value, car.Id);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(40)]
        public async void GetNotExist(int value) {
            // Arrange
            var repository = getRepository();

            // Act
            var car = await repository.GetAsync(value);

            // Assert
            Assert.Null(car);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(1)]
        [InlineData(6)]
        public async void GetEmpty(int value) {
            // Arrange
            var repository = getRepository();

            // Act
            var car = await repository.GetCarWithEmptySeatsAsync(value);

            // Assert
            Assert.NotNull(car);
            Assert.True(car.Seats >= value);
        }

        [Theory]
        [InlineData(10)]
        public async void GetNotEmpty(int value) {
            // Arrange
            var repository = getRepository();

            // Act
            var car = await repository.GetCarWithEmptySeatsAsync(value);

            // Assert
            Assert.Null(car);
        }
    }
}