using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.Services.AutoMapper
{
    public class WarehouseNextHopsProfile : Profile
    {
        public WarehouseNextHopsProfile()
        {
            CreateMap<BusinessLogic.Entities.WarehouseNextHops, DTOs.Models.WarehouseNextHops>().ReverseMap();
        }
    }
}
