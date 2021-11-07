using AutoMapper;
using FluentValidation;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic.Entities.Validators;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using Juna.SKS.Package.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic
{
    public class WarehouseManagementLogic : IWarehouseManagementLogic
    {
        private readonly IMapper _mapper;
        private IHopRepository _repository;
        public WarehouseManagementLogic(IHopRepository repo, IMapper mapper)
        {
            _repository = repo;
            _mapper = mapper;
        }
        public Warehouse ExportWarehouses()
        {
            GeoCoordinate locationCoodinates = new GeoCoordinate(3.1, 4.5);
            Hop hop1 = new Hop("truck", "123", "aHop", 23, "Vienna", locationCoodinates);
            Hop hop2 = new Hop("truck", "321", "anotherHop", 13, "Vienna", locationCoodinates);
            List <WarehouseNextHops> nextHops = new List<WarehouseNextHops>
            {
                new WarehouseNextHops(123, hop1),
                new WarehouseNextHops(456, hop2)
            };

            Warehouse warehouse = new Warehouse(0, nextHops, "warehouse", "111", "Hauptlager 27-12", 11, "Vienna", locationCoodinates);
            return warehouse;
        }

        public Warehouse GetWarehouse(string code)
        {
            HopArrival wrapHopArrival = new HopArrival(code, "Hauptlager 27-12", DateTime.Now);
            
            IValidator<HopArrival> validator = new HopArrivalValidator();
            var result = validator.Validate(wrapHopArrival);

            if (result.IsValid == false)
            {
                return null;
            }

            DataAccess.Entities.Warehouse DAwarehouse = _repository.GetSingleWarehouseByCode(code);
            Warehouse warehouse = this._mapper.Map<BusinessLogic.Entities.Warehouse>(DAwarehouse);

            return warehouse;
            /*
            GeoCoordinate locationCoodinates = new GeoCoordinate(3.1, 4.5);
            Hop hop1 = new Hop("truck", "123", "aHop", 23, "Vienna", locationCoodinates);
            Hop hop2 = new Hop("truck", "321", "anotherHop", 13, "Vienna", locationCoodinates);
            List<WarehouseNextHops> nextHops = new List<WarehouseNextHops>
            {
                new WarehouseNextHops(123, hop1),
                new WarehouseNextHops(456, hop2)
            };

            Warehouse warehouse = new Warehouse(0, nextHops, "warehouse", code, "Hauptlager 27-12", 11, "Vienna", locationCoodinates);
            return warehouse;*/
        }

        public bool ImportWarehouses(Warehouse warehouse)
        {
            IValidator<Warehouse> validator = new WarehouseValidator();
            var result = validator.Validate(warehouse);

            if (result.IsValid == false)
            {
                return false;
            }

            DataAccess.Entities.Warehouse DAwarehouse = this._mapper.Map<DataAccess.Entities.Warehouse>(warehouse);
            _repository.Create(DAwarehouse);

            return true;
        }
    }
}
