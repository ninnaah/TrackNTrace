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
    public class WarehouseProfile : Profile
    {
        public WarehouseProfile()
        {
            CreateMap<BusinessLogic.Entities.Warehouse, DTOs.Models.Warehouse>().ReverseMap();
        }
    }
}
