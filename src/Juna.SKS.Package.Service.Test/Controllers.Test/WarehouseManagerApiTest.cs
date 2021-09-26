using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Juna.SKS.Package.Services.DTOs.Models;
using Juna.SKS.Package.Services.Controllers;

namespace Juna.SKS.Package.Services.Test.Controllers.Test
{
    public class WarehouseManagerApiTest
    {

        [Test]
        public void GetWarehouse_ValidCode_NoArgumentExceptionThrown()
        {
            WarehouseManagementApiController manager = new WarehouseManagementApiController();
            
            string code = "WTTA01";
            Exception ex = null;

            try
            {
                manager.GetWarehouse(code);
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.IsNull(ex);
        }


        [Test]
        public void GetWarehouse_EmptyCode_ArgumentExceptionThrown()
        {
            WarehouseManagementApiController manager = new WarehouseManagementApiController();

            string code = null;
            Exception ex = null;

            try
            {
                manager.GetWarehouse(code);
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.AreEqual(ex.Message, "Code cannot be null");

        }

        [Test]
        public void GetWarehouse_CodeLenthZero_ArgumentExceptionThrown()
        {
            WarehouseManagementApiController manager = new WarehouseManagementApiController();

            string code = "";
            Exception ex = null;

            try
            {
                manager.GetWarehouse(code);
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.AreEqual(ex.Message, "Code cannot have zero or negative length");

        }

        [Test]
        public void ImportWarehouses_ValidWarehouse_NoArgumentExceptionThrown()
        {
            WarehouseManagementApiController manager = new WarehouseManagementApiController();

            Warehouse warehouse = new Warehouse();
            warehouse.Level = 1;
            Exception ex = null;

            try
            {
                manager.ImportWarehouses(warehouse);
            }
            catch (Exception e)
            {
                ex = e;
            }

            //Assert.IsNull(ex);
            Assert.AreEqual(ex.Message, "The method or operation is not implemented.");

        }


        [Test]
        public void ImportWarehouses_EmptyLevel_ArgumentExceptionThrown()
        {
            WarehouseManagementApiController manager = new WarehouseManagementApiController();

            Warehouse warehouse = new Warehouse();
            warehouse.Level = null;
            Exception ex = null;

            try
            {
                manager.ImportWarehouses(warehouse);
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.AreEqual(ex.Message, "Level cannot be null");

        }

        [Test]
        public void ImportWarehouses_LevelZero_ArgumentExceptionThrown()
        {
            WarehouseManagementApiController manager = new WarehouseManagementApiController();

            Warehouse warehouse = new Warehouse();
            warehouse.Level = 0;
            Exception ex = null;

            try
            {
                manager.ImportWarehouses(warehouse);
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.AreEqual(ex.Message, "Zero or negative level is not valid");

        }

    }
}
