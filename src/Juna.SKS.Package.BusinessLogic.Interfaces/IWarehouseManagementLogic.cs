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
        Warehouse ExportWarehouses();

        Warehouse GetWarehouse(string code);

        bool ImportWarehouses(Warehouse warehouse);
    }
}
