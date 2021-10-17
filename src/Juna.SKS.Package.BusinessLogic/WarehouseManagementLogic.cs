using FluentValidation;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic.Entities.Validators;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic
{
    public class WarehouseManagementLogic : IWarehouseManagementLogic
    {
        public WarehouseManagementLogic()
        {

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
            HopArrival wrapHopArrival = new HopArrival(code, "aHop", DateTime.Now);
            
            IValidator<HopArrival> validator = new HopArrivalValidator();
            var result = validator.Validate(wrapHopArrival);

            if (result.IsValid == false)
            {
                return null;
            }

            GeoCoordinate locationCoodinates = new GeoCoordinate(3.1, 4.5);
            Hop hop1 = new Hop("truck", "123", "aHop", 23, "Vienna", locationCoodinates);
            Hop hop2 = new Hop("truck", "321", "anotherHop", 13, "Vienna", locationCoodinates);
            List<WarehouseNextHops> nextHops = new List<WarehouseNextHops>
            {
                new WarehouseNextHops(123, hop1),
                new WarehouseNextHops(456, hop2)
            };

            Warehouse warehouse = new Warehouse(0, nextHops, "warehouse", code, "Hauptlager 27-12", 11, "Vienna", locationCoodinates);
            return warehouse;
        }

        public bool ImportWarehouses(Warehouse warehouse)
        {
            IValidator<Warehouse> validator = new WarehouseValidator();
            var result = validator.Validate(warehouse);

            if (result.IsValid == false)
            {
                return false;
            }

            return true;
        }
    }
}
