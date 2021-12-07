using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic.Interfaces
{
    public interface IStaffLogic
    {
        void ReportParcelDelivery(string trackingId);

        void ReportParcelHop(string trackingId, string code);
    }
}
