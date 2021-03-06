using Juna.SKS.Package.DataAccess.Entities;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Juna.SKS.Package.ServiceAgents.Interfaces
{
    public interface IGeoEncodingAgent
    {
        GeoCoordinate EncodeAddress(string street, string postalCode, string city, string country);
    }
}
