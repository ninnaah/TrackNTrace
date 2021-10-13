using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic.Interfaces
{
    public interface IStaffLogic
    {
        bool ReportParcelDelivery(string trackingId);

        bool ReportParcelHop(string trackingId, string code);
    }
}
