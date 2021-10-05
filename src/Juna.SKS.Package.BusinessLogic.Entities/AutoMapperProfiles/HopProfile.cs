using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Juna.SKS.Package.Services.DTOs.Models;


namespace Juna.SKS.Package.BusinessLogic.Entities.AutoMapperProfiles
{
    public class HopProfile : Profile
    {
        public HopProfile()
        {
            CreateMap<Entities.Hop, Services.DTOs.Models.Hop>();
        }
    }
}
