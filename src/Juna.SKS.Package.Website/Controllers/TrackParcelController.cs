using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Juna.SKS.Package.Website.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace Juna.SKS.Package.Website.Controllers
{
    public class TrackParcelController : Controller
    {
        private HttpClient _client;
        private Uri _baseAdress = new Uri("https://localhost:5001/");

        public TrackParcelController()
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseAdress;

        }

        [HttpGet]
        public ActionResult Track(string trackingId)
        {
            //read id from user

            TrackingInformation parcelInfo = new TrackingInformation();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/parcel/{trackingId}").Result;
           
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                parcelInfo = JsonConvert.DeserializeObject<TrackingInformation>(data);
                
            }
            return View(parcelInfo);

        }
    }
}