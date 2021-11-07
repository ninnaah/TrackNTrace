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
    public class WarehouseNextHopsProfile : Profile
    {
        public WarehouseNextHopsProfile()
        {
            CreateMap<BusinessLogic.Entities.WarehouseNextHops, DTOs.Models.WarehouseNextHops>().ReverseMap();
            CreateMap<BusinessLogic.Entities.WarehouseNextHops, DataAccess.Entities.WarehouseNextHops>().ReverseMap();
        }
    }
}
