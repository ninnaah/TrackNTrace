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

            _logger.LogInformation("Trying to export a warehouse");
            Warehouse warehouse = new();
            try
            {
                DataAccess.Entities.Warehouse DAwarehouse = _repository.GetWarehouseHierarchy();
                warehouse = this._mapper.Map<Warehouse>(DAwarehouse);
            }
            catch (DataNotFoundException ex)
            {
                string errorMessage = $"Warehouse hierarchy cannot be found";
                _logger.LogError(errorMessage, ex);
                throw new LogicDataNotFoundException(nameof(WarehouseManagementLogic), nameof(ExportWarehouse), errorMessage);
            }
            catch (DataException ex)
            {
                string errorMessage = $"An error occurred exporting the warehouse hierachy";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(WarehouseManagementLogic), nameof(ExportWarehouse), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred exporting the warehouse hierachy";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(WarehouseManagementLogic), nameof(ExportWarehouse), errorMessage, ex);
            }

            _logger.LogInformation("Exported warehouse");
            return warehouse;
        }

        public Hop GetHop(string code)
        {
            _logger.LogInformation("Trying to get a warehouse");

            HopArrival wrapHopArrival = new HopArrival(code, "Hauptlager 27-12", DateTime.Now);
            
            IValidator<HopArrival> validator = new HopArrivalValidator();
            var result = validator.Validate(wrapHopArrival);

            if (result.IsValid == false)
            {
                _logger.LogError($"Code is invalid - {result}");
                throw new ValidatorException(nameof(code), nameof(GetHop), string.Join(" ", result.Errors.Select(err => err.ErrorMessage)));
            }

            try
            {
                DataAccess.Entities.Hop DAhop = _repository.GetSingleHopByCode(code);
                Hop hop = this._mapper.Map<BusinessLogic.Entities.Hop>(DAhop);

                _logger.LogInformation("Got the warehouse");
                return hop;
            }
            catch (DataNotFoundException ex)
            {
                string errorMessage = $"Warehouse with code {code} cannot be found";
                _logger.LogError(errorMessage, ex);
                throw new LogicDataNotFoundException(nameof(WarehouseManagementLogic), nameof(GetHop), errorMessage);
            }
            catch (DataException ex)
            {
                string errorMessage = $"An error occurred while getting a warehouse with code {code}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(WarehouseManagementLogic), nameof(GetHop), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred while getting a warehouse with code {code}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(WarehouseManagementLogic), nameof(GetHop), errorMessage, ex);
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

            try
            {
                _repository.DropDatabase();
                DataAccess.Entities.Warehouse DAwarehouse = this._mapper.Map<DataAccess.Entities.Warehouse>(warehouse);
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
