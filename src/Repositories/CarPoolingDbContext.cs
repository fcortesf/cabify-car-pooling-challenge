using System;
using car_pooling_api.Domain;
using car_pooling_api.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace car_pooling_api.Repositories {
    public class CarPoolingDbContext : DbContext, IUnitOfWork {

        public CarPoolingDbContext(DbContextOptions<CarPoolingDbContext> options) : base(options) { }
        public DbSet<Car> Car { get; set; }
        public DbSet<Group> Group { get; set; }

        public void Commit()
        {
            this.SaveChanges();
        }

        public void DeleteAll()
        {
           this.Database.EnsureDeleted();
        }
    }

    public class CarPoolingDbContextDesignFactory : IDesignTimeDbContextFactory<CarPoolingDbContext>
    {
        public CarPoolingDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CarPoolingDbContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            return new CarPoolingDbContext(builder.Options);
        }
    }
}