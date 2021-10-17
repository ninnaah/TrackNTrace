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
    public class HopProfile : Profile
    {
        public HopProfile()
        {
            CreateMap<BusinessLogic.Entities.Hop, DTOs.Models.Hop>()
               .Include<BusinessLogic.Entities.Warehouse, DTOs.Models.Warehouse>()
               .Include<BusinessLogic.Entities.Truck, DTOs.Models.Truck>()
               .Include<BusinessLogic.Entities.Transferwarehouse, DTOs.Models.Transferwarehouse>()
               .ReverseMap();
        }
    }
}