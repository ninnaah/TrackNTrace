using AutoMapper;
using FluentValidation;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic.Entities.Validators;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using Juna.SKS.Package.BusinessLogic.Interfaces.Exceptions;
using Juna.SKS.Package.DataAccess.Interfaces;
using Juna.SKS.Package.DataAccess.Interfaces.Exceptions;
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
                _logger.LogError($"Code is invalid - {result}");
                throw new ValidatorException(nameof(code), nameof(GetWarehouse), string.Join(" ", result.Errors.Select(err => err.ErrorMessage)));
            }

            try
            {
                DataAccess.Entities.Warehouse DAwarehouse = _repository.GetSingleWarehouseByCode(code);
                Warehouse warehouse = this._mapper.Map<BusinessLogic.Entities.Warehouse>(DAwarehouse);

                _logger.LogInformation("Got the warehouse");
                return warehouse;
            }
            catch (DataNotFoundException ex)
            {
                string errorMessage = $"Warehouse with code {code} cannot be found";
                _logger.LogError(errorMessage, ex);
                throw new LogicDataNotFoundException(nameof(WarehouseManagementLogic), nameof(GetWarehouse), errorMessage);
            }
            catch (DataException ex)
            {
                string errorMessage = $"An error occurred while getting a warehouse with code {code}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(WarehouseManagementLogic), nameof(GetWarehouse), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred while getting a warehouse with code {code}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(WarehouseManagementLogic), nameof(GetWarehouse), errorMessage, ex);
            }

        }

        public bool ImportWarehouse(Warehouse warehouse)
        {
            _logger.LogInformation("Trying to import a warehouse");

            IValidator<Warehouse> validator = new WarehouseValidator();
            var result = validator.Validate(warehouse);

            if (result.IsValid == false)
            {
                _logger.LogError($"Warehouse is invalid - {result}");
                throw new ValidatorException(nameof(warehouse), nameof(ImportWarehouse), string.Join(" ", result.Errors.Select(err => err.ErrorMessage)));
            }

            DataAccess.Entities.Warehouse DAwarehouse = this._mapper.Map<DataAccess.Entities.Warehouse>(warehouse);
            try
            {
                _repository.Create(DAwarehouse);
            }
            catch (DataException ex)
            {
                string errorMessage = $"An error occurred importing a warehouse with code {warehouse.Code}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(WarehouseManagementLogic), nameof(ImportWarehouse), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred importing a warehouse with code {warehouse.Code}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(WarehouseManagementLogic), nameof(ImportWarehouse), errorMessage, ex);
            }

            _logger.LogInformation("Imported the warehouse");
            return true;
        }
    }
}
