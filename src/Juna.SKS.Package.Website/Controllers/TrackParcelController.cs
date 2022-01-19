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

        
    }
}