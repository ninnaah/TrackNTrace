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
    public class WebhookResponseProfile : Profile
    {
        public WebhookResponseProfile()
        {
            CreateMap<BusinessLogic.Entities.WebhookResponse, DTOs.Models.WebhookResponse>().ReverseMap();
            CreateMap<BusinessLogic.Entities.WebhookResponse, DataAccess.Entities.WebhookResponse>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ReverseMap();
        }
    }
}
