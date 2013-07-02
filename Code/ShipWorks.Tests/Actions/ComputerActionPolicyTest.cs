using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Actions;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.Actions
{
    [TestClass]
    public class ComputerActionPolicyTest
    {
        private ComputerActionPolicy testObject;

        [TestMethod]
        public void ToCsv_WithSingleComputer_Test()
        {
            List<ComputerEntity> computers = new List<ComputerEntity>
            {
                new ComputerEntity(1001)
            };

            testObject = new ComputerActionPolicy(computers);

            string csv = testObject.ToCsv();
            Assert.AreEqual("1001", csv);
        }

        [TestMethod]
        public void ToCsv_WithMultipleComputers_Test()
        {
            List<ComputerEntity> computers = new List<ComputerEntity>
            {
                new ComputerEntity(1001),
                new ComputerEntity(2001)
            };

            testObject = new ComputerActionPolicy(computers);

            string csv = testObject.ToCsv();
            Assert.AreEqual("1001, 2001", csv);
        }
    }
}
