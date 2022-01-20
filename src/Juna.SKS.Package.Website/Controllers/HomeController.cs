using Juna.SKS.Package.Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private HttpClient _client;
        private Uri _baseAdress = new Uri("https://juna-trackntrace.azurewebsites.net/");

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            _client = new HttpClient();
            _client.BaseAddress = _baseAdress;
        }

        public IActionResult Index()
        {   
            return View();
        }



        [HttpGet]
        public ActionResult Submit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Submit(ParcelInput parcel)
        {
            NewParcelInfo parcelInfo = new();

            Parcel sendParcel = new();
            sendParcel.Recipient = new(parcel.RecipientName, parcel.RecipientStreet, parcel.RecipientPostalCode, parcel.RecipientCity, parcel.RecipientCountry);
            sendParcel.Sender = new(parcel.SenderName, parcel.SenderStreet, parcel.SenderPostalCode, parcel.SenderCity, parcel.SenderCountry);
            sendParcel.Weight = parcel.Weight;

            string data = JsonConvert.SerializeObject(sendParcel);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = _client.PostAsync($"{_client.BaseAddress}parcel", content).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                parcelInfo = JsonConvert.DeserializeObject<NewParcelInfo>(result);
            }
            return View("DisplaySubmit", parcelInfo);
        }
        [HttpGet]
        public ActionResult DisplaySubmit(NewParcelInfo parcelInfo)
        {
            return View(parcelInfo);
        }




        [HttpGet]
        public ActionResult Track()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Track(NewParcelInfo parcelInfo)
        {
            //read id from user

            TrackingInformation trackingInfo = new TrackingInformation();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}parcel/{parcelInfo.TrackingId}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                trackingInfo = JsonConvert.DeserializeObject<TrackingInformation>(data);
            }
            return View("DisplayTrack", trackingInfo);
        }
        [HttpGet]
        public ActionResult DisplayTrack(TrackingInformation trackingInfo)
        {
            return View(trackingInfo);
        }



        [HttpGet]
        public ActionResult ReportHop()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReportHop(ReportHopInput input)
        {
            NewParcelInfo message = new();
            string data = JsonConvert.SerializeObject(input);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = _client.PostAsync($"{_client.BaseAddress}parcel/{input.TrackingId}/reportHop/{input.Code}", content).Result;
            if (response.IsSuccessStatusCode)
            {
                message.TrackingId = $"Reported Parcel with trackingId {input.TrackingId} and Hop with Code {input.Code}";
            }
            else
            {
                message.TrackingId = response.StatusCode.ToString();
            }
            return View("DisplayReportHop", message);
        }
        [HttpGet]
        public ActionResult DisplayReportHop(NewParcelInfo message)
        {
            return View(message);
        }




        [HttpGet]
        public ActionResult ReportDelivery()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReportDelivery(NewParcelInfo parcelInfo)
        {
            NewParcelInfo message = new();
            string data = JsonConvert.SerializeObject(parcelInfo);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = _client.PostAsync($"{_client.BaseAddress}parcel/{parcelInfo.TrackingId}/reportDelivery", content).Result;
            if (response.IsSuccessStatusCode)
            {
                message.TrackingId = $"Reported Parcel delivery with trackingId {parcelInfo.TrackingId}";
            }
            else
            {
                message.TrackingId = response.StatusCode.ToString();
            }
            return View("DisplayReportDelivery", message);
        }
        [HttpGet]
        public ActionResult DisplayReportDelivery(NewParcelInfo message)
        {
            return View(message);
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
