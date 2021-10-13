using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.Services.AutoMapper
{
    public class AutoMapperProvider
    {
        public static IMapper GetMapper()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ParcelProfile());
                mc.AddProfile(new HopArrivalProfile());
                mc.AddProfile(new HopProfile());
                mc.AddProfile(new RecipientProfile());
                mc.AddProfile(new TransferwarehouseProfile());
                mc.AddProfile(new TruckProfile());
                mc.AddProfile(new WarehouseProfile());
            });

            Mapper mapper = new Mapper(mapperConfig);

            return mapper;
        }
    }
}
