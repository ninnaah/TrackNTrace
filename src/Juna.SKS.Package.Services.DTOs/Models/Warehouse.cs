/*
 * Parcel Logistics Service
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
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

namespace Juna.SKS.Package.Services.DTOs.Models
{ 
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class Warehouse : Hop
    { 
        /// <summary>
        /// Gets or Sets Level
        /// </summary>
        [Required]

        [DataMember(Name="level")]
        public int? Level { get; set; }

        /// <summary>
        /// Next hops after this warehouse (warehouses or trucks).
        /// </summary>
        /// <value>Next hops after this warehouse (warehouses or trucks).</value>
        [Required]

        [DataMember(Name="nextHops")]
        public List<WarehouseNextHops> NextHops { get; set; }

    }
}