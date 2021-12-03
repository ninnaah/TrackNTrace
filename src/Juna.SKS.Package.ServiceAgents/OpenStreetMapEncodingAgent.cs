using Juna.SKS.Package.DataAccess.Entities;
using Juna.SKS.Package.DataAccess.Interfaces.Exceptions;
using Juna.SKS.Package.ServiceAgents.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Nominatim.API.Geocoders;
using Nominatim.API.Models;
using System;
using System.IO;
using System.Net;

namespace Juna.SKS.Package.ServiceAgents
{
    public class OpenStreetMapEncodingAgent : IGeoEncodingAgent
    {
        private readonly ILogger<OpenStreetMapEncodingAgent> _logger;
        private readonly ForwardGeocoder _coder;

        public OpenStreetMapEncodingAgent(ILogger<OpenStreetMapEncodingAgent> logger)
        {
            _coder = new ForwardGeocoder();
            _logger = logger;
        }

        public GeoCoordinate EncodeAddress(string street, string postalCode, string city, string country)
        {
            _logger.LogInformation($"Trying to encode an adress - street: {street}, postalCode: {postalCode}, city: {city} and country: {country}");
            var response = _coder.Geocode(new ForwardGeocodeRequest()
            {
                StreetAddress = street,
                City = city,
                Country = country,
                PostalCode = postalCode
            }).Result;

            if (response.Length == 0)
            {
                _logger.LogError($"No response received for street: {street}, postalCode: {postalCode}, city: {city} and country: {country}");
                throw new DataNotFoundException(nameof(OpenStreetMapEncodingAgent), nameof(EncodeAddress));
            }

            GeoCoordinate coordinate = new();
            coordinate.Lat = response[0].Latitude;
            coordinate.Lon = response[0].Longitude;

            _logger.LogInformation($"Encoded adress - street: {street}, postalCode: {postalCode}, city: {city} and country: {country}");
            return coordinate;
        }















        /*public GeoCoordinate EncodeAddress(string street, string postalCode, string city, string country)
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
        }*/
    }
}
