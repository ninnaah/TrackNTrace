using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Juna.SKS.Package.IntegrationTests
{
    /*public class IntegrationTest
    {
        private string _baseURL;
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            _baseURL = "https://juna-trackntrace.azurewebsites.net/";

            _httpClient = new HttpClient()
            {
                BaseAddress = new System.Uri(_baseURL)
            };
        }

        [Test, Order(1)]
        public async Task ImportWarehouse()
        {
            
        }

        [Test, Order(1)]
        public async Task ExportWarehouse()
        {
            var response = await _httpClient.GetAsync("/Warehouse");
            var json = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json", response.Content.Headers.ContentType.MediaType);
            Assert.IsNotEmpty(json);

            Assert.That(json, Contains.Substring("Warehouse Level 1 - Wien"));
        }

        [Test, Order(1)]
        public async Task GetWarehouse()
        {

        }

        [Test, Order(2)]
        public async Task SubmitParcel()
        {
        }

        [Test, Order(3)]
        public async Task TrackParcel_1()
        {
        }

        [Test, Order(4)]
        public async Task ReportParcelHop_1()
        {
        }

        [Test, Order(5)]
        public async Task TrackParcel_2()
        {
        }

        [Test, Order(6)]
        public async Task ReportParcelHop_2()
        {
        }

        [Test, Order(7)]
        public async Task ReportParcelDelivery()
        {
        }

        [Test, Order(8)]
        public async Task TrackParcel_3()
        {
        }





        [Test, Order(9)]
        public async Task TransitionParcel()
        {
        }






        [Test, Order(10)]
        public async Task SubscribeParcelWebhook_1()
        {
        }

        [Test, Order(11)]
        public async Task SubscribeParcelWebhook_2()
        {
        }

        [Test, Order(12)]
        public async Task ListParcelWebhooks_1()
        {
        }

        [Test, Order(13)]
        public async Task UnsubscribeParcelWebhook_1()
        {
        }

        [Test, Order(14)]
        public async Task ListParcelWebhooks_2()
        {
        }
    }*/
}