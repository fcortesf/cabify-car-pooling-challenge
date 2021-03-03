using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using car_pooling_api.Domain;
using car_pooling_api.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace car_pooling_api.Repositories {
    public class CarRepository : ICarRepository
    {
        private readonly CarPoolingDbContext dbContext;
        public CarRepository(CarPoolingDbContext context)
        {
            dbContext = context;
        }
        public IUnitOfWork unitOfWork { 
            get {
                return dbContext;
            }
        }

        private Car AddGroups(Car car){
            car.GroupsAssigned = dbContext.Group.Where(g => g.CarId == car.Id).ToList();
            return car;
        }

        public async Task<Car> GetAsync(int id)
        {
            var car = await dbContext.Car.FirstOrDefaultAsync(c => c.Id == id);
            if (car != null) {
                car = AddGroups(car);
            }
            return car;
        }

        public async Task<Car> GetCarWithEmptySeatsAsync(int minimunEmpty)
        {
            var cars = await dbContext.Car.ToListAsync();
            var carsWithGroups = cars.Select(c => AddGroups(c));
            return carsWithGroups.FirstOrDefault(c => c.EmptySeats >= minimunEmpty);
                     
        }

        public async Task<Car> InsertAsync(Car carToSave)
        {
            await dbContext.Car.AddAsync(carToSave);
            return carToSave;
        }

        public async Task InsertRangeAsync(IEnumerable<Car> rangeCar)
        {
            await dbContext.Car.AddRangeAsync(rangeCar);
        }

        public async Task<Car> UpdateCarAsync(Car car)
        {
            await dbContext.SaveChangesAsync();
            return car;
        }
    }
}