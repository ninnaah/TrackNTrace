using Juna.SKS.Package.Services.DTOs.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.IntegrationTests
{
    public class IntegrationTest
    {
        private string _baseURL;
        private HttpClient _httpClient;

        NewParcelInfo parcelInfo;

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

            var sampleDataset = File.ReadAllText(@"../../../../Juna.SKS.Package.IntegrationTests/SampleDataset.json");
            Debug.WriteLine(sampleDataset);

            var data = new StringContent(sampleDataset, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/warehouse", data);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test, Order(1)]
        public async Task ExportWarehouse()
        {
            var response = await _httpClient.GetAsync("/warehouse");
            var json = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json", response.Content.Headers.ContentType.MediaType);
            Assert.IsNotEmpty(json);
            Assert.That(json, Contains.Substring("Warehouse Level 1 - Wien"));
        }

        [Test, Order(1)]
        public async Task GetWarehouse()
        {
            var response = await _httpClient.GetAsync("/warehouse/WTTA010");
            var json = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json", response.Content.Headers.ContentType.MediaType);
            Assert.IsNotEmpty(json);
            Assert.That(json, Contains.Substring("Truck in Atzgersdorf"));
        }

        /*[Test, Order(2)]
        public async Task SubmitParcel()
        {
            Recipient recipient = new ()
            {
                Name = "Anna",
                Street = "Höchstädtplatz 3",
                PostalCode = "1200",
                City = "Wien",
                Country ="Österreich"
            };

            Recipient sender = new()
            {
                Name = "Berta",
                Street = "Dresdnerstraße 20",
                PostalCode = "1200",
                City = "Wien",
                Country = "Österreich"
            };

            Parcel parcel = new ()
            {
                Weight = 3,
                Recipient = recipient,
                Sender = sender
            };

            var dataJson = JsonConvert.SerializeObject(parcel);
            var data = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/parcel", data);

            var json = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual("application/json", response.Content.Headers.ContentType.MediaType);
            Assert.IsNotEmpty(json);
            Assert.That(json, Contains.Substring("trackingId"));

            parcelInfo = JsonConvert.DeserializeObject<NewParcelInfo>(json);

        }*/

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
    }
}