using Juna.SKS.Package.DataAccess.Entities;
using Juna.SKS.Package.DataAccess.Interfaces.Exceptions;
using Juna.SKS.Package.ServiceAgents.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Nominatim.API.Geocoders;
using Nominatim.API.Models;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;

namespace Juna.SKS.Package.ServiceAgents
{
    [ExcludeFromCodeCoverage]
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


    }
}
