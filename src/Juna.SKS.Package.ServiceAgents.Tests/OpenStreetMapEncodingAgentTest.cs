using Juna.SKS.Package.DataAccess.Entities;
using Juna.SKS.Package.ServiceAgents.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Juna.SKS.Package.ServiceAgents.Tests
{
    public class OpenStreetMapEncodingAgentTest
    {
        Mock<ILogger<OpenStreetMapEncodingAgent>> mockLogger;
        [SetUp]
        public void Setup()
        {
            mockLogger = new Mock<ILogger<OpenStreetMapEncodingAgent>>();
        }

        /*[Test]
        public void EncodeAddress_ValidInput_ReturnGeoCoordinate()
        {
            IGeoEncodingAgent osm = new OpenStreetMapEncodingAgent(mockLogger.Object);
            GeoCoordinate geoCoordintae = osm.EncodeAddress("Höchstädtplatz", "1200", "Wien", "Austria");

            Assert.IsNotNull(geoCoordintae);
            Assert.IsInstanceOf<double>(geoCoordintae.Lat);
            Assert.IsInstanceOf<double>(geoCoordintae.Lon);
        }*/


        [Test]
        public void EncodeAddress_InvalidInputStreetIsNull_ReturnNull()
        {
            IGeoEncodingAgent osm = new OpenStreetMapEncodingAgent(mockLogger.Object);
            GeoCoordinate geoCoordintae = osm.EncodeAddress(null, "1200", "Wien", "Austria");

            Assert.IsNull(geoCoordintae);
        }
        [Test]
        public void EncodeAddress_InvalidInputCityIsNull_ReturnNull()
        {
            IGeoEncodingAgent osm = new OpenStreetMapEncodingAgent(mockLogger.Object);
            GeoCoordinate geoCoordintae = osm.EncodeAddress("Höchstädtplatz", "1200", null, "Austria");

            Assert.IsNull(geoCoordintae);
        }
        [Test]
        public void EncodeAddress_InvalidInputCountryIsNull_ReturnNull()
        {
            IGeoEncodingAgent osm = new OpenStreetMapEncodingAgent(mockLogger.Object);
            GeoCoordinate geoCoordintae = osm.EncodeAddress("Höchstädtplatz", "1200", "Wien", null);

            Assert.IsNull(geoCoordintae);
        }
    }
}