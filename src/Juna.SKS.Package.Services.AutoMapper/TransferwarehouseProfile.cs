using AutoMapper;
using NetTopologySuite.Algorithm;
using NetTopologySuite.Features;
using NetTopologySuite.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.Services.AutoMapper
{
    [ExcludeFromCodeCoverage]
    public class TransferwarehouseProfile : Profile
    {
        public TransferwarehouseProfile()
        {
            CreateMap<BusinessLogic.Entities.TransferWarehouse, DTOs.Models.TransferWarehouse>().ReverseMap();

            CreateMap<BusinessLogic.Entities.TransferWarehouse, DataAccess.Entities.TransferWarehouse>().BeforeMap((s, d) =>
            {
                var reader = new GeoJsonReader();
                Feature g = reader.Read<Feature>(s.RegionGeoJson);
                if (!Orientation.IsCCW(g.Geometry.Coordinates))
                    g.Geometry = g.Geometry.Reverse();
                d.Region = g.Geometry;
            });
            CreateMap<DataAccess.Entities.TransferWarehouse, BusinessLogic.Entities.TransferWarehouse>().BeforeMap((s, d) =>
            {
                var writer = new GeoJsonWriter();
                d.RegionGeoJson = writer.Write(new Feature()
                {
                    Geometry = s.Region
                });
            });


        }
    }
}
