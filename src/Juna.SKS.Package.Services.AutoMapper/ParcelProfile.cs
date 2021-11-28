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
    public class ParcelProfile : Profile
    {
        public ParcelProfile()
        {
            CreateMap<DTOs.Models.Parcel, BusinessLogic.Entities.Parcel>().ReverseMap();
            CreateMap<DTOs.Models.NewParcelInfo, BusinessLogic.Entities.Parcel>().ReverseMap();
            CreateMap<DTOs.Models.TrackingInformation, BusinessLogic.Entities.Parcel>().ReverseMap();

            CreateMap<BusinessLogic.Entities.Parcel, DataAccess.Entities.Parcel>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ReverseMap();
        }
    }
}
