using Juna.SKS.Package.Website.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;


namespace Juna.SKS.Package.Website.Controllers
{
    public class SubmitParcelController : Controller
    {
        private HttpClient _client;
        private Uri _baseAdress = new Uri("https://localhost:5001/");

        public SubmitParcelController()
        {
            _client = new HttpClient();
            _client.BaseAddress = _baseAdress;
           
        }

        [HttpPost]
        public ActionResult Submit(Parcel parcel)
        {
            string trackingId = "";
            string data = JsonConvert.SerializeObject(parcel);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = _client.PostAsync($"{_client.BaseAddress}/parcel", content).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                trackingId = JsonConvert.DeserializeObject<string>(result);
            }
            return View(trackingId);
        }
    }
}