using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.Services.AutoMapper
{
    public class TruckProfile : Profile
    {
        public TruckProfile()
        {
            CreateMap<BusinessLogic.Entities.Truck, DTOs.Models.Truck>().ReverseMap();
        }
    }
}
