using System.Collections.Generic;
using car_pooling_api.Domain;
using car_pooling_api.Domain.Repositories;
using car_pooling_api.Services;
using IO.Swagger.Models;
using Moq;
using Xunit;

namespace car_pooling.UnitTest.Services
{
    public class CarServiceTest {

        [Fact]
        public async void InitCarDatabaseTest(){
            // Arrange
            var carRepositoryMock = new Mock<ICarRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();            
            carRepositoryMock.Setup(c => c.unitOfWork).Returns(unitOfWorkMock.Object);
            var service = new CarService(carRepositoryMock.Object);

            // Act
            await service.InitCarDatabase(new List<CarDto>());

            // Assert
            unitOfWorkMock.Verify(c => c.DeleteAll(), Times.Exactly(1));
            unitOfWorkMock.Verify(c => c.Commit(), Times.Exactly(1));
            carRepositoryMock.Verify(c => c.InsertRangeAsync(new List<Car>()), Times.Exactly(1));             
        }
    }
}
