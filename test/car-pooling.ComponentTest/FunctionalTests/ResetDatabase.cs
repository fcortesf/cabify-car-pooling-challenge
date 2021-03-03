using System;
using System.Threading.Tasks;
using car.pooling.ComponentTest.ApiClient;
using car_pooling.ComponentTest.Mothers;

namespace car_pooling.ComponentTest.FunctionalTests
{
    public class ResetDatabase : IFunctionalTest
    {
        private CarPoolingApiClient client;
        private async Task Health()
        {
            await client.StatusAsync();            
        }

        public async Task<bool> ExecTest(CarPoolingApiClient apiClient)
        {
            client = apiClient;
            GroupMother.ResetId();
            await Health();
            await InitDatabase();
            await Journey();
            await Locate();
            await InitDatabase();
            await LocateNotFound();
            return true;
        }

        private async Task InitDatabase(){
            var cars = CarsMother.GetOneCarList(4);

            await client.CarsAsync(cars);
        }

        private async Task Journey(){
            var group = GroupMother.GetGroup(4);
            await client.JourneyAsync(group);
        }

        private async Task LocateNotFound() {
            try {
                await client.LocateAsync(1);
                throw new Exception("Locate should throw an exception with 404 code.");
            }
            catch(ApiException notFound) {
                if (notFound.StatusCode != 404) {
                    throw new Exception("Locate should throw an exception with 404 code.");
                }
            }
        }

        private async Task Locate() {
           var car =  await client.LocateAsync(1);
           if (car == null | car.Id != 1) {
               throw new Exception("The group should be placed on car with id == 1.");
           }
        }
    }
}