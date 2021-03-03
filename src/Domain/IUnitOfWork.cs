namespace car_pooling_api.Domain.Repositories { 
    public interface IUnitOfWork {

        void Commit();
        void DeleteAll();
    }
}