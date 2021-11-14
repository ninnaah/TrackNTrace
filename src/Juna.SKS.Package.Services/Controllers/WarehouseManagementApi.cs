/*
 * Parcel Logistics Service
 *
 * No description provided (generated by Swagger
 * Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.20.1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Juna.SKS.Package.Services.Attributes;

using Microsoft.AspNetCore.Authorization;
using Juna.SKS.Package.Services;
using Juna.SKS.Package.Services.DTOs.Models;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using Juna.SKS.Package.BusinessLogic;
using AutoMapper;
using Juna.SKS.Package.Services.AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Juna.SKS.Package.DataAccess.Sql;
using Microsoft.Extensions.Logging;

namespace Juna.SKS.Package.Services.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class WarehouseManagementApiController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWarehouseManagementLogic _warehouseManagementLogic;
        private readonly ILogger<WarehouseManagementApiController> _logger;

        public WarehouseManagementApiController(IWarehouseManagementLogic warehouseManagementLogic, IMapper mapper, ILogger<WarehouseManagementApiController> logger)
        {
            this._warehouseManagementLogic = warehouseManagementLogic;
            this._mapper = mapper;
            _logger = logger;
        }
        /// <summary>
        /// Exports the hierarchy of Warehouse and Truck objects. 
        /// </summary>
        /// <response code="200">Successful response</response>
        /// <response code="400">An error occurred loading.</response>
        /// <response code="404">No hierarchy loaded yet.</response>
        [HttpGet]
        [Route("/warehouse")]
        [ValidateModelState]
        [SwaggerOperation("ExportWarehouses")]
        [SwaggerResponse(statusCode: 200, type: typeof(Warehouse), description: "Successful response")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "An error occurred loading.")]
        public virtual IActionResult ExportWarehouses()
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(Warehouse));

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400, default(Error));

            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404);

            var response = this._warehouseManagementLogic.ExportWarehouse();

            if (response == null)
            {
                _logger.LogInformation("Respond 400 - No warehouse found");
                return StatusCode(404, new Error("No warehouse found"));
            }

            BusinessLogic.Entities.Warehouse BLwarehouse = response;
            DTOs.Models.Warehouse warehouse = this._mapper.Map<DTOs.Models.Warehouse>(BLwarehouse);

            _logger.LogInformation("Respond 200 - Exported warehouse");
            return StatusCode(200, warehouse);

            //400  missing
        }

        /// <summary>
        /// Get a certain warehouse or truck by code
        /// </summary>
        /// <param name="code"></param>
        /// <response code="200">Successful response</response>
        /// <response code="400">An error occurred loading.</response>
        /// <response code="404">Warehouse id not found</response>
        [HttpGet]
        [Route("/warehouse/{code}")]
        [ValidateModelState]
        [SwaggerOperation("GetWarehouse")]
        [SwaggerResponse(statusCode: 200, type: typeof(Warehouse), description: "Successful response")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "An error occurred loading.")]
        public virtual IActionResult GetWarehouse([FromRoute][Required]string code)
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(Warehouse));

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400, default(Error));

            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404);
            try
            {
                var response = this._warehouseManagementLogic.GetWarehouse(code);

                if (response == null)
                {
                    _logger.LogInformation("Respond 400 - Code is invalid");
                    return StatusCode(400, new Error("Code is invalid"));
                }

                BusinessLogic.Entities.Warehouse BLwarehouse = response;
                DTOs.Models.Warehouse warehouse = this._mapper.Map<DTOs.Models.Warehouse>(BLwarehouse);

                _logger.LogInformation("Respond 200 - Got warehouse");
                return StatusCode(200, warehouse);
            }
            catch (Exception)
            {
                _logger.LogInformation("Respond 404 - Warehouse not found");
                return StatusCode(404, new Error("Warehouse not found"));
            }
            

        }

        /// <summary>
        /// Imports a hierarchy of Warehouse and Truck objects. 
        /// </summary>
        /// <param name="body"></param>
        /// <response code="200">Successfully loaded.</response>
        /// <response code="400">The operation failed due to an error.</response>
        [HttpPost]
        [Route("/warehouse")]
        [ValidateModelState]
        [SwaggerOperation("ImportWarehouses")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "The operation failed due to an error.")]
        public virtual IActionResult ImportWarehouses([FromBody]Warehouse body)
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200);

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400, default(Error));

            BusinessLogic.Entities.Warehouse BLwarehouse = this._mapper.Map<BusinessLogic.Entities.Warehouse>(body);
            bool response = this._warehouseManagementLogic.ImportWarehouse(BLwarehouse);

            if (response == false)
            {
                _logger.LogInformation("Respond 400 - Warehouse is invalid");
                return StatusCode(400, new Error("Warehouse is invalid"));
            }
            _logger.LogInformation("Respond 200 - Imported warehouse");
            return StatusCode(200);
        }
    }
}
