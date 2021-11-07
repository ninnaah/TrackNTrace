using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Juna.SKS.Package.Services.DTOs.Models;
using Juna.SKS.Package.BusinessLogic.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Juna.SKS.Package.Services.AutoMapper
{
    [ExcludeFromCodeCoverage]
    public class HopArrivalProfile : Profile
    {
        public HopArrivalProfile()
        {
            CreateMap<BusinessLogic.Entities.HopArrival, DTOs.Models.HopArrival>().ReverseMap();
            CreateMap<BusinessLogic.Entities.HopArrival, DataAccess.Entities.HopArrival>().ReverseMap();
        }
    }
}
