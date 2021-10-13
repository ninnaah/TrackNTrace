using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Juna.SKS.Package.Services.DTOs.Models;
using Juna.SKS.Package.BusinessLogic.Entities;

namespace Juna.SKS.Package.Services.AutoMapper
{
    public class HopArrivalProfile : Profile
    {
        public HopArrivalProfile()
        {
            CreateMap<BusinessLogic.Entities.HopArrival, DTOs.Models.HopArrival>().ReverseMap();
        }
    }
}
