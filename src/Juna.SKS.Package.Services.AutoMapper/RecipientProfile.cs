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
    public class RecipientProfile : Profile
    {
        public RecipientProfile()
        {
            CreateMap<BusinessLogic.Entities.Recipient, DTOs.Models.Recipient>().ReverseMap();
            CreateMap<BusinessLogic.Entities.Recipient, DataAccess.Entities.Recipient>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ReverseMap();
        }
    }
}
