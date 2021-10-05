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
        string ExportWarehouses();

        string GetWarehouse(string code);

        string ImportWarehouses(Warehouse warehouse);
    }
}
