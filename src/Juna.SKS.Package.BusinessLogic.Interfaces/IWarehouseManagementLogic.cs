using Juna.SKS.Package.BusinessLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic.Interfaces
{
    public interface IWarehouseManagementLogic
    {
        Warehouse ExportWarehouse();

        Hop GetHop(string code);

        bool ImportWarehouse(Warehouse warehouse);
    }
}
