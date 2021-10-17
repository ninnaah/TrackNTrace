using AutoMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.Services.AutoMapper
{
    [ExcludeFromCodeCoverage]
    public class GeoCoordinateProfile : Profile
    {
        public GeoCoordinateProfile()
        {
            CreateMap<BusinessLogic.Entities.GeoCoordinate, DTOs.Models.GeoCoordinate>().ReverseMap();
        }
    }
}
