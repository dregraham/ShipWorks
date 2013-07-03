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
                new ComputerEntity(2001),
                new ComputerEntity(3001),
                new ComputerEntity(4001)
            };

            testObject = new ComputerActionPolicy(computers);

            string csv = testObject.ToCsv();
            Assert.AreEqual("1001, 2001, 3001, 4001", csv);
        }

        [TestMethod]
        public void IsComputerAllowed_ReturnsFalse_Test()
        {
            List<ComputerEntity> computers = new List<ComputerEntity>
            {
                new ComputerEntity(1001),
                new ComputerEntity(2001),
                new ComputerEntity(3001),
                new ComputerEntity(4001)
            };

            testObject = new ComputerActionPolicy(computers);

            Assert.IsFalse(testObject.IsComputerAllowed(new ComputerEntity(5001)));
        }

        [TestMethod]
        public void IsComputerAllowed_ReturnsTrue_Test()
        {
            List<ComputerEntity> computers = new List<ComputerEntity>
            {
                new ComputerEntity(1001),
                new ComputerEntity(2001),
                new ComputerEntity(3001),
                new ComputerEntity(4001)
            };

            testObject = new ComputerActionPolicy(computers);

            Assert.IsTrue(testObject.IsComputerAllowed(new ComputerEntity(3001)));
        }

        [TestMethod]
        public void Constructor_LoadsComputersFromCsv_WithSingleComputerId_Test()
        {
            testObject = new ComputerActionPolicy("1001");

            Assert.IsTrue(testObject.IsComputerAllowed(new ComputerEntity(1001)));
        }

        [TestMethod]
        public void Constructor_LoadsComputersFromCsv_WithMultipleComputerIds_Test()
        {
            testObject = new ComputerActionPolicy("1001, 2001, 3001");

            Assert.IsFalse(testObject.IsComputerAllowed(new ComputerEntity(2001)));
        }


    }
}
