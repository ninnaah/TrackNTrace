using Juna.SKS.Package.DataAccess.Entities;
using Juna.SKS.Package.DataAccess.Interfaces.Exceptions;
using Juna.SKS.Package.ServiceAgents.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Nominatim.API.Geocoders;
using Nominatim.API.Models;
using NUnit.Framework;

namespace Juna.SKS.Package.ServiceAgents.Tests
{
    /*public class OpenStreetMapEncodingAgentTest
    {
        Mock<ILogger<OpenStreetMapEncodingAgent>> mockLogger;
        Mock<ForwardGeocoder> mockCoder;

        [SetUp]
        public void Setup()
        {
            mockCoder = new Mock<ForwardGeocoder>();
            mockLogger = new Mock<ILogger<OpenStreetMapEncodingAgent>>();
        }

        [Test]
        public void EncodeAddress_ValidInput_ReturnGeoCoordinate()
        {
            var response = new GeocodeResponse[2]
            {
                new GeocodeResponse(),
                new GeocodeResponse()
            };

            response[0].Latitude = 1.2;
            response[0].Longitude = 2.1;

            var param = new ForwardGeocodeRequest()
            {
                StreetAddress = "Höchstädtplatz",
                City = "1200",
                Country = "Wien",
                PostalCode = "Austria"
            };

            mockCoder.Setup(g => g.Geocode(It.IsAny<ForwardGeocodeRequest>())).ReturnsAsync(response);

            IGeoEncodingAgent osm = new OpenStreetMapEncodingAgent(mockLogger.Object);
            GeoCoordinate geoCoordinate = osm.EncodeAddress("Höchstädtplatz", "1200", "Wien", "Austria");

            Assert.IsNotNull(geoCoordinate);
            Assert.IsInstanceOf<double>(geoCoordinate.Lat);
            Assert.IsInstanceOf<double>(geoCoordinate.Lon);
        }


        [Test]
        public void EncodeAddress_ValidInputStreetIsNull_ReturnGeoCoordinate()
        {
            IGeoEncodingAgent osm = new OpenStreetMapEncodingAgent(mockLogger.Object);
            GeoCoordinate geoCoordinate = osm.EncodeAddress(null, "1200", "Wien", "Austria");

            Assert.IsNotNull(geoCoordinate);
            Assert.IsInstanceOf<double>(geoCoordinate.Lat);
            Assert.IsInstanceOf<double>(geoCoordinate.Lon);
        }
        [Test]
        public void EncodeAddress_ValidInputCityIsNull_ReturnGeoCoordinate()
        {
            IGeoEncodingAgent osm = new OpenStreetMapEncodingAgent(mockLogger.Object);
            GeoCoordinate geoCoordinate = osm.EncodeAddress("Höchstädtplatz", "1200", null, "Austria");

            Assert.IsNotNull(geoCoordinate);
            Assert.IsInstanceOf<double>(geoCoordinate.Lat);
            Assert.IsInstanceOf<double>(geoCoordinate.Lon);
        }
        [Test]
        public void EncodeAddress_ValidInputCountryIsNull_ReturnGeoCoordinate()
        {
            IGeoEncodingAgent osm = new OpenStreetMapEncodingAgent(mockLogger.Object);
            GeoCoordinate geoCoordinate = osm.EncodeAddress("Höchstädtplatz", "1200", "Wien", null);

            Assert.IsNotNull(geoCoordinate);
            Assert.IsInstanceOf<double>(geoCoordinate.Lat);
            Assert.IsInstanceOf<double>(geoCoordinate.Lon);
        }
        [Test]
        public void EncodeAddress_InValidInputIsNull_ReturnNull()
        {
            IGeoEncodingAgent osm = new OpenStreetMapEncodingAgent(mockLogger.Object);

            try
            {
                GeoCoordinate geoCoordinate = osm.EncodeAddress(null, null, null, null);
                Assert.Fail();
            }
            catch (DataNotFoundException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void EncodeAddress_InValidInpu_ReturnNull()
        {
            IGeoEncodingAgent osm = new OpenStreetMapEncodingAgent(mockLogger.Object);

            try
            {
                GeoCoordinate geoCoordinate = osm.EncodeAddress("hfgbvdfgv", "3456754", "dhcvb<gh", "ycbycsgrf");
                Assert.Fail();
            }
            catch (DataNotFoundException)
            {
                Assert.Pass();
            }
        }
    }*/
}