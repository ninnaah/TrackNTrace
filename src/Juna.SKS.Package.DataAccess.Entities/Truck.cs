using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class Truck : Hop
    {
        public Truck()
        {

        }

        public Truck(Geometry region, string numberPlate)
        {
            Region = region;
            NumberPlate = numberPlate;
        }

        public Truck(int id, Geometry region, string numberPlate, string hopType, string code, string description, int processingDelayMins, string locationName, GeoCoordinate locationCoordinates)
        {
            Id = id;
            Region = region;
            NumberPlate = numberPlate;
            HopType = hopType;
            Code = code;
            Description = description;
            ProcessingDelayMins = processingDelayMins;
            LocationName = locationName;
            LocationCoordinates = locationCoordinates;
        }

        public Truck(int id, Geometry region, string numberPlate, string hopType, string code, string description, int processingDelayMins, string locationName, GeoCoordinate locationCoordinates, WarehouseNextHops parent)
        {
            Id = id;
            Region = region;
            NumberPlate = numberPlate;
            HopType = hopType;
            Code = code;
            Description = description;
            ProcessingDelayMins = processingDelayMins;
            LocationName = locationName;
            LocationCoordinates = locationCoordinates;
            Parent = parent;
        }
        public virtual Geometry Region { get; set; }

        public string NumberPlate { get; set; }
    }
}
