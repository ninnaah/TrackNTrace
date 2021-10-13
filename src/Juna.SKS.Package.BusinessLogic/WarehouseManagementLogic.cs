using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic
{
    public class WarehouseManagementLogic : IWarehouseManagementLogic
    {
        public WarehouseManagementLogic()
        {

        }
        public Warehouse ExportWarehouses()
        {
            List <Hop> nextHops = new List<Hop>
            {
                new Hop("truck", "123", "aHop", 23, "Vienna", 2.3, 3.1, 45),
                new Hop("truck", "321", "anotherHop", 13, "Vienna", 1.3, 3.4, 75)
            };

            Warehouse warehouse = new Warehouse(0, nextHops, "warehouse", "111", "aWarehouse", 11, "Vienna", 1.1, 1.1, 111);
            return warehouse;
        }

        public Warehouse GetWarehouse(string code)
        {
            List<Hop> nextHops = new List<Hop>
            {
                new Hop("truck", "123", "aHop", 23, "Vienna", 2.3, 3.1, 45),
                new Hop("truck", "321", "anotherHop", 13, "Vienna", 1.3, 3.4, 75)
            };

            Warehouse warehouse = new Warehouse(0, nextHops, "warehouse", "111", "aWarehouse", 11, "Vienna", 1.1, 1.1, 111);
            return warehouse;
        }

        public Warehouse ImportWarehouses()
        {
            List<Hop> nextHops = new List<Hop>
            {
                new Hop("truck", "123", "aHop", 23, "Vienna", 2.3, 3.1, 45),
                new Hop("truck", "321", "anotherHop", 13, "Vienna", 1.3, 3.4, 75)
            };

            Warehouse warehouse = new Warehouse(0, nextHops, "warehouse", "111", "aWarehouse", 11, "Vienna", 1.1, 1.1, 111);
            return warehouse;
        }
    }
}
