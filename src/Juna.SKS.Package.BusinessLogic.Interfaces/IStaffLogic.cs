using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic.Interfaces
{
    public interface IStaffLogic
    {
        string ReportParcelDelivery(string trackingId);

        string ReportParcelHop(string trackingId, string code);
    }
}
