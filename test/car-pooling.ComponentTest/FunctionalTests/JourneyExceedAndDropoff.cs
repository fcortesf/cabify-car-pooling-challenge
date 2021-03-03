using System;
using System.Threading.Tasks;
using car.pooling.ComponentTest.ApiClient;
using car_pooling.ComponentTest.Mothers;

namespace car_pooling.ComponentTest.FunctionalTests
{
    public class JourneyExceedAndDropoff : IFunctionalTest
    {
        private CarPoolingApiClient client;

        public async Task<bool> ExecTest(CarPoolingApiClient apiClient)
        {
            client = apiClient;
            GroupMother.ResetId();
            await Health();
            await InitDatabase();
            await Journey();
            await Journey();
            await Journey();
            await LocateEmpty();
            await Dropoff();
            await Locate();
            return true;
        }

        private async Task Health()
        {
            await client.StatusAsync();            
        }

        private async Task InitDatabase(){
            var cars = CarsMother.GetOneCarList(4);

            await client.CarsAsync(cars);
        }

        private async Task Journey(){
            var group = GroupMother.GetGroup(2);
            await client.JourneyAsync(group);
        }

        private async Task Locate() {
           var car =  await client.LocateAsync(3);
           if (car == null | car.Id != 1) {
               throw new Exception("The group should be placed on car with id == 1.");
           }
        }

        private async Task Dropoff() {
            await client.DropoffAsync(1);            
        }

        private async Task LocateEmpty() {
            try {
                await client.LocateAsync(3);
                throw new Exception("Locate should throw an exception with 204 code.");
            }
            catch(ApiException notFound) {
                if (notFound.StatusCode != 204) {
                    throw new Exception("Locate should throw an exception with 204 code.");
                }
            }
        }

    }
}
