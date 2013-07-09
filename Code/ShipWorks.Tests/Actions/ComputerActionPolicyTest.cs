﻿using System;
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

            testObject = new ComputerActionPolicy(ComputerLimitationType.NamedList, "1001");

            string csv = testObject.ToCsv();
            Assert.AreEqual("1001", csv);
        }

        [TestMethod]
        public void ToCsv_WithMultipleComputers_Test()
        {
            string computerIDs = "1001, 2001, 3001,4001";

            testObject = new ComputerActionPolicy(ComputerLimitationType.NamedList, computerIDs);

            string csv = testObject.ToCsv();
            Assert.AreEqual("1001, 2001, 3001, 4001", csv);
        }

        [TestMethod]
        public void IsComputerAllowed_ReturnsFalse_Test()
        {
            string computerIDs = "1001, 2001, 3001,4001";

            testObject = new ComputerActionPolicy(ComputerLimitationType.NamedList, computerIDs);

            Assert.IsFalse(testObject.IsComputerAllowed(new ComputerEntity(5001)));
        }

        [TestMethod]
        public void IsComputerAllowed_ReturnsTrue_Test()
        {
            string computerIDs = "1001, 2001, 3001,4001";

            testObject = new ComputerActionPolicy(ComputerLimitationType.NamedList, computerIDs);

            Assert.IsTrue(testObject.IsComputerAllowed(new ComputerEntity(3001)));
        }

        [TestMethod]
        public void Constructor_LoadsComputersFromCsv_WithSingleComputerId_Test()
        {
            string computerIDs = "1001";

            testObject = new ComputerActionPolicy(ComputerLimitationType.NamedList, computerIDs);

            Assert.IsTrue(testObject.IsComputerAllowed(new ComputerEntity(1001)));
        }

        [TestMethod]
        public void Constructor_LoadsComputersFromCsv_WithMultipleComputerIds_Test()
        {
            string computerIDs = "1001, 2001, 3001,4001";

            testObject = new ComputerActionPolicy(ComputerLimitationType.NamedList, computerIDs);

            Assert.IsTrue(testObject.IsComputerAllowed(new ComputerEntity(2001)));
        }

        [TestMethod]
        public void Constructor_LoadsComputersFromActionEntityInternalComputerLimitedList_WithSingleComputerId_Test()
        {
            string computerIDs = "1001, 2001, 3001,4001";

            testObject = new ComputerActionPolicy(ComputerLimitationType.NamedList, computerIDs);

            Assert.IsTrue(testObject.IsComputerAllowed(new ComputerEntity(1001)));
        }

        [TestMethod]
        public void Constructor_LoadsComputersFromActionEntityInternalComputerLimitedList_WithMutlipleComputerIds_Test()
        {
            string computerIDs = "1001, 2001, 3001,4001,5001";

            testObject = new ComputerActionPolicy(ComputerLimitationType.NamedList, computerIDs);

            for (int i = 0; i < 5; i++)
            {
                long computerId = 1001 + 1000 * i;
                Assert.IsTrue(testObject.IsComputerAllowed(new ComputerEntity(computerId)));
            }
        }
    }
}
