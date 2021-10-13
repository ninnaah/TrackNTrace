using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.Services.AutoMapper
{
    public class TransferwarehouseProfile : Profile
    {
        public TransferwarehouseProfile()
        {
            CreateMap<BusinessLogic.Entities.Transferwarehouse, DTOs.Models.Transferwarehouse>().ReverseMap();
        }
    }
}
