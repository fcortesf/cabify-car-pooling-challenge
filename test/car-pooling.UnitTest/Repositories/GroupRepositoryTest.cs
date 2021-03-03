using System;
using System.Linq;
using car_pooling_api.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace car_pooling.UnitTest.Repositories
{
    public class GroupRepositoryTest
    {
         private CarPoolingDbContext getContext() {
            var options = new DbContextOptionsBuilder<CarPoolingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            return new CarPoolingDbContext(options);
        }

        private CarPoolingDbContext loadData() {
            var context = getContext();
            context.Group.AddRange(GroupsMother.GetGroups(null));
            context.SaveChanges();
            return context;
        }

        private GroupRepository getRepository() {
            var contextData = loadData();
            return new GroupRepository(contextData);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(1)]
        [InlineData(4)]
        public async void GetExist(int value) {
            // Arrange
            var repository = getRepository();

            // Act
            var group = await repository.GetAsync(value);

            // Assert
            Assert.NotNull(group);
            Assert.Equal(value, group.Id);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(40)]
        public async void GetNotExist(int value) {
            // Arrange
            var repository = getRepository();

            // Act
            var group = await repository.GetAsync(value);

            // Assert
            Assert.Null(group);
        }

        [Fact]
        public async void GetUnsaggineds() {
             // Arrange
            var repository = getRepository();

            // Act
            var groups = await repository.GetUnsagginedsAsync();

            // Assert
            Assert.Equal(GroupsMother.GetGroups(null).Count(), groups.Count());
        }

        [Fact]
        public async void GetEmptyUnsaggineds() {
            // Arrange
            var context = getContext();
            context.Group.AddRange(GroupsMother.GetGroups(1));
            context.SaveChanges();
            var repository = new GroupRepository(context);

            // Act
            var groups = await repository.GetUnsagginedsAsync();

            // Assert
            Assert.Empty(groups);
        }
    }
}