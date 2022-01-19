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
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace Juna.SKS.Package.Website.Models
{ 
    [ExcludeFromCodeCoverage]
    public class TrackingInformation
    { 
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum StateEnum
        {
            [EnumMember(Value = "Pickup")]
            PickupEnum = 0,

            [EnumMember(Value = "InTransport")]
            InTransportEnum = 1,

            [EnumMember(Value = "InTruckDelivery")]
            InTruckDeliveryEnum = 2,

            [EnumMember(Value = "Transferred")]
            TransferredEnum = 3,

            [EnumMember(Value = "Delivered")]
            DeliveredEnum = 4        
        }

        public StateEnum? State { get; set; }

        public List<HopArrival> VisitedHops { get; set; }

        public List<HopArrival> FutureHops { get; set; }

    }
}
