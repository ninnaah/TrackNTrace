using Juna.SKS.Package.DataAccess.Entities;
using Juna.SKS.Package.ServiceAgents.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Juna.SKS.Package.ServiceAgents.Tests
{
    /*public class OpenStreetMapEncodingAgentTest
    {
        Mock<ILogger<OpenStreetMapEncodingAgent>> mockLogger;
        [SetUp]
        public void Setup()
        {
            mockLogger = new Mock<ILogger<OpenStreetMapEncodingAgent>>();
        }

        [Test]
        public void EncodeAddress_ValidInput_ReturnGeoCoordinate()
        {
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
            GeoCoordinate geoCoordinate = osm.EncodeAddress(null, null, null, null);

            Assert.IsNull(geoCoordinate);
            Assert.IsInstanceOf<double>(geoCoordinate.Lat);
            Assert.IsInstanceOf<double>(geoCoordinate.Lon);
        }
    }*/
}