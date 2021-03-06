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
using Juna.SKS.Package.Services.DTOs.Models;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using Juna.SKS.Package.BusinessLogic;
using AutoMapper;
using Juna.SKS.Package.Services.AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Juna.SKS.Package.DataAccess.Sql;
using Microsoft.Extensions.Logging;
using Juna.SKS.Package.BusinessLogic.Interfaces.Exceptions;

namespace Juna.SKS.Package.Services.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class StaffApiController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IStaffLogic _staffLogic;
        private readonly ILogger<StaffApiController> _logger;

        public StaffApiController(IStaffLogic staffLogic, IMapper mapper, ILogger<StaffApiController> logger)
        {
            this._staffLogic = staffLogic;
            this._mapper = mapper;
            _logger = logger;
        }
        /// <summary>
        /// Report that a Parcel has been delivered at it&#x27;s final destination address. 
        /// </summary>
        /// <param name="trackingId">The tracking ID of the parcel. E.g. PYJRB4HZ6 </param>
        /// <response code="200">Successfully reported hop.</response>
        /// <response code="400">The operation failed due to an error.</response>
        /// <response code="404">Parcel does not exist with this tracking ID. </response>
        [HttpPost]
        [Route("/parcel/{trackingId}/reportDelivery/")]
        [ValidateModelState]
        [SwaggerOperation("ReportParcelDelivery")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "The operation failed due to an error.")]
        public virtual IActionResult ReportParcelDelivery([FromRoute][Required][RegularExpression("^[A-Z0-9]{9}$")]string trackingId)
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200);

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400, default(Error));

            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404);

            try
            {
                this._staffLogic.ReportParcelDelivery(trackingId);

                _logger.LogInformation("Respond 200 - Reported parcel delivery");
                return StatusCode(200);
            }
            catch (LogicDataNotFoundException ex)
            {
                _logger.LogError("Respond 404 - Parcel not found", ex);
                return StatusCode(404);
            }
            catch (ValidatorException ex)
            {
                _logger.LogError("Respond 400 - Trackingid is invalid", ex);
                return StatusCode(400, new Error($"Trackingid is invalid - {ex.Message}")); ;
            }
            catch (LogicException ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(400, new Error(ex.Message)); ;
            }


        }

        /// <summary>
        /// Report that a Parcel has arrived at a certain hop either Warehouse or Truck. 
        /// </summary>
        /// <param name="trackingId">The tracking ID of the parcel. E.g. PYJRB4HZ6 </param>
        /// <param name="code">The Code of the hop (Warehouse or Truck).</param>
        /// <response code="200">Successfully reported hop.</response>
        /// <response code="400">The operation failed due to an error.</response>
        /// <response code="404">Parcel does not exist with this tracking ID or hop with code not found. </response>
        [HttpPost]
        [Route("/parcel/{trackingId}/reportHop/{code}")]
        [ValidateModelState]
        [SwaggerOperation("ReportParcelHop")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "The operation failed due to an error.")]
        public virtual IActionResult ReportParcelHop([FromRoute][Required][RegularExpression("^[A-Z0-9]{9}$")]string trackingId, [FromRoute][Required][RegularExpression(@"^[A-Z]{4}\d{1,4}$")]string code)
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200);

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400, default(Error));

            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404);
            try
            {
                this._staffLogic.ReportParcelHop(trackingId, code);
                _logger.LogInformation("Respond 200 - Reported parcel hop");
                return StatusCode(200);
            }
            catch (LogicDataNotFoundException ex)
            {
                _logger.LogError("Respond 404 - Parcel or hop not found", ex);
                return StatusCode(404);
            }
            catch (ValidatorException ex)
            {
                _logger.LogError("Respond 400 - Trackingid or code is invalid", ex);
                return StatusCode(400, new Error($"Trackingid or code is invalid - {ex.Message}")); ;
            }
            catch (LogicException ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(400, new Error(ex.Message)); ;
            }

        }
    }
}
