using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;


namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        private static INationalClient _client;
        [TestMethod]
        public void TestMethod1()
        {
            _client = new NationalClient(_options);

        }

        private readonly IOptions<WebApiOptions> _options = Options.Create(new WebApiOptions
        {
            ClientId = "961f5593-3384-47a6-a186-1dd526f020b7",
            Secret = "4q.55~0kl4lJb-xcIAQM_W~1UrOxqnBQ4p",
            ResourceUrl = "https://redbarnet.dk/961f5593-3384-47a6-a186-1dd526f020b7",
            TenantId = "b738c0f4-215a-438a-ad8b-3a8b60351c56"
        });


        public void RunClass()
        {
            var Endpoint = new NationalWebApiRequest<string>
            {
                Endpoint = "/common/lists"
            };

            var token = client.GetAccessToken();
            var list = await _client.Get<string, InitiativeApiResponse>(Endpoint);
        }
    }
}
