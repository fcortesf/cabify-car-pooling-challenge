using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using car_pooling_api.Domain;
using car_pooling_api.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace car_pooling_api.Repositories {
    public class GroupRepository : IGroupRepository
    {
        private readonly CarPoolingDbContext dbContext;

        public GroupRepository(CarPoolingDbContext context)
        {
            dbContext = context;
        }
        public IUnitOfWork unitOfWork {
            get {
                return dbContext;
            }
        }

        public async Task DeleteAsync(int groupId)
        {
            var groupToRemove = await dbContext.Group.FirstOrDefaultAsync(g => g.Id == groupId);
            dbContext.Group.Remove(groupToRemove);
        }

        public async Task<Group> GetAsync(int id)
        {
            var group = await dbContext.Group.FirstOrDefaultAsync(g => g.Id == id);
            if (group != null && (group.CarId != null || group.CarId > 0)) {
                group.JourneyCar = dbContext.Car.FirstOrDefault(c => c.Id == group.CarId);
            }
            return group;
        }

        public async Task<IEnumerable<Group>> GetUnsagginedsAsync()
        {
            return await dbContext.Group.Where(g => g.CarId == null || g.CarId <= 0).OrderBy(g => g.CreationDate).ToListAsync();
        }

        public async Task<Group> InsertAsync(Group groupToSave)
        {
            groupToSave.CreationDate = DateTime.Now;
            await dbContext.Group.AddAsync(groupToSave);
            return groupToSave;
        }

        public Task<Group> SaveJourneyAsync(Group group, Car car)
        {
            return Task.Run(() => {
                group.CarId = car.Id;
                group.JourneyCar = car;
                return group;
            });
        }
    }
}