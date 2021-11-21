using Juna.SKS.Package.DataAccess.Entities;
using Juna.SKS.Package.ServiceAgents.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;

namespace Juna.SKS.Package.ServiceAgents
{
    public class OpenStreetMapEncodingAgent : IGeoEncodingAgent
    {
        private readonly ILogger<OpenStreetMapEncodingAgent> _logger;

        public OpenStreetMapEncodingAgent(ILogger<OpenStreetMapEncodingAgent> logger)
        {
            _logger = logger;
        }
        public GeoCoordinate EncodeAddress(string street, string postalCode, string city, string country)
        {
            try
            {
                string urlString = "https://nominatim.openstreetmap.org/search?street="+ street +"&city="+ city +"&country="+country+"&postalcode="+postalCode+"&format=geojson&limit=1";
                _logger.LogInformation($"Encoding Adress with OpenStreetMap with url {urlString}");

                var request = WebRequest.Create(urlString);
                request.ContentType = "application/json";
                request.Method = "GET";
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                        try
                        {
                            GeoCoordinate geoCoordinate = new ();
                            if (response.StatusCode != HttpStatusCode.OK)
                            {
                                _logger.LogError($"Error fetching data. OSM Server returned status code: {response.StatusCode}");
                                return null;
                            }
                            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                            {
                                var json = reader.ReadToEnd();
                                _logger.LogInformation($"Response OSM: {json}");

                                if (string.IsNullOrWhiteSpace(json))
                                {
                                    _logger.LogError("Response contained emty body");
                                    return null;
                                }
                                else
                                {
                                    var location = JObject.Parse(json)["geometry"];
                                    var coordinates = location?["coordinates"];

                                    geoCoordinate.Lat = coordinates[0]?.Value<double>();
                                    geoCoordinate.Lon = coordinates[1]?.Value<double>();

                                }
                            }
                            return geoCoordinate;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Error while loading the data:: {ex.Message}");
                            return null;
                        }
                }
            }
            catch (WebException ex)
            {
                _logger.LogError($"Error while loading the data:: {ex.Message}");
                return null;
            }
        }
    }
}
