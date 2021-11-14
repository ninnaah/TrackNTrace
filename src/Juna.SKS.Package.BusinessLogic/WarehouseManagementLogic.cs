using AutoMapper;
using FluentValidation;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic.Entities.Validators;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using Juna.SKS.Package.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<WarehouseManagementLogic> _logger;
        public WarehouseManagementLogic(IHopRepository repo, IMapper mapper, ILogger<WarehouseManagementLogic> logger)
        {
            _repository = repo;
            _mapper = mapper;
            _logger = logger;
        }
        public Warehouse ExportWarehouse()
        {
            /*IEnumerable<DataAccess.Entities.Warehouse> DAwarehouses = _repository.GetAllWarehouses();

            IEnumerable<Warehouse> warehouses = this._mapper.Map<IEnumerable<BusinessLogic.Entities.Warehouse>>(DAwarehouses);

            return warehouses;*/

            _logger.LogInformation("Trying to export a warehouse");

            GeoCoordinate locationCoodinates = new GeoCoordinate(3.1, 4.5);
            Hop hop1 = new Hop("truck", "123", "aHop", 23, "Vienna", locationCoodinates);
            Hop hop2 = new Hop("truck", "321", "anotherHop", 13, "Vienna", locationCoodinates);
            List <WarehouseNextHops> nextHops = new List<WarehouseNextHops>
            {
                new WarehouseNextHops(123, hop1),
                new WarehouseNextHops(456, hop2)
            };

            Warehouse warehouse = new Warehouse(0, nextHops, "warehouse", "111", "Hauptlager 27-12", 11, "Vienna", locationCoodinates);
            _logger.LogInformation("Exported warehouse");
            return warehouse;
        }

        public Warehouse GetWarehouse(string code)
        {
            _logger.LogInformation("Trying to get a warehouse");

            HopArrival wrapHopArrival = new HopArrival(code, "Hauptlager 27-12", DateTime.Now);
            
            IValidator<HopArrival> validator = new HopArrivalValidator();
            var result = validator.Validate(wrapHopArrival);

            if (result.IsValid == false)
            {
                _logger.LogDebug($"Code is invalid - {result}");
                return null;
            }

            try
            {
                DataAccess.Entities.Warehouse DAwarehouse = _repository.GetSingleWarehouseByCode(code);
                Warehouse warehouse = this._mapper.Map<BusinessLogic.Entities.Warehouse>(DAwarehouse);

                _logger.LogInformation("Got the warehouse");
                return warehouse;
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Warehouse not found - {ex.Message}");
                throw;
            }
            
        }

        public bool ImportWarehouse(Warehouse warehouse)
        {
            _logger.LogInformation("Trying to import a warehouse");

            IValidator<Warehouse> validator = new WarehouseValidator();
            var result = validator.Validate(warehouse);

            if (result.IsValid == false)
            {
                _logger.LogDebug($"Warehouse is invalid - {result}");
                return false;
            }

            DataAccess.Entities.Warehouse DAwarehouse = this._mapper.Map<DataAccess.Entities.Warehouse>(warehouse);
            _repository.Create(DAwarehouse);

            _logger.LogInformation("Imported the warehouse");
            return true;
        }
    }
}
