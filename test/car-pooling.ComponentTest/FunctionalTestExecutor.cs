using car.pooling.ComponentTest.ApiClient;
using Xunit;

namespace car_pooling.ComponentTest.FunctionalTests
{
    public class FunctionalTestExecutor {
        private readonly CarPoolingApiClient client;
        public FunctionalTestExecutor()
        {
            client = new CarPoolingApiClient();
            client.BaseUrl = BaseUrl.Url;
        }
        [Fact]
        public async void ExecuteFunctionalTest() {
            var tests = FunctionalTestCollection.GetAllFunctionalTest();
            foreach (var test in tests)
            {
                var result = await test.ExecTest(client);
                Assert.True(result);
            }
        }
    }
}