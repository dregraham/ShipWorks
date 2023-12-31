﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Actions;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.Actions
{
    public class ComputerActionPolicyTest
    {
        private ComputerActionPolicy testObject;

        [Fact]
        public void ToCsv_WithSingleComputer()
        {
            List<ComputerEntity> computers = new List<ComputerEntity>
            {
                new ComputerEntity(1001)
            };

            testObject = new ComputerActionPolicy("1001");

            string csv = testObject.ToCsv();
            Assert.Equal("1001", csv);
        }

        [Fact]
        public void ToCsv_WithMultipleComputers()
        {
            string computerIDs = "1001, 2001, 3001,4001";

            testObject = new ComputerActionPolicy(computerIDs);

            string csv = testObject.ToCsv();
            Assert.Equal("1001, 2001, 3001, 4001", csv);
        }

        [Fact]
        public void IsComputerAllowed_ReturnsFalse()
        {
            string computerIDs = "1001, 2001, 3001,4001";

            testObject = new ComputerActionPolicy(computerIDs);

            Assert.False(testObject.IsComputerAllowed(new ComputerEntity(5001)));
        }

        [Fact]
        public void IsComputerAllowed_ReturnsTrue()
        {
            string computerIDs = "1001, 2001, 3001,4001";

            testObject = new ComputerActionPolicy(computerIDs);

            Assert.True(testObject.IsComputerAllowed(new ComputerEntity(3001)));
        }

        [Fact]
        public void Constructor_LoadsComputersFromCsv_WithSingleComputerId()
        {
            string computerIDs = "1001";

            testObject = new ComputerActionPolicy(computerIDs);

            Assert.True(testObject.IsComputerAllowed(new ComputerEntity(1001)));
        }

        [Fact]
        public void Constructor_LoadsComputersFromCsv_WithMultipleComputerIds()
        {
            string computerIDs = "1001, 2001, 3001,4001";

            testObject = new ComputerActionPolicy(computerIDs);

            Assert.True(testObject.IsComputerAllowed(new ComputerEntity(2001)));
        }

        [Fact]
        public void Constructor_LoadsComputersFromActionEntityInternalComputerLimitedList_WithSingleComputerId()
        {
            string computerIDs = "1001, 2001, 3001,4001";

            testObject = new ComputerActionPolicy(computerIDs);

            Assert.True(testObject.IsComputerAllowed(new ComputerEntity(1001)));
        }

        [Fact]
        public void Constructor_LoadsComputersFromActionEntityInternalComputerLimitedList_WithMutlipleComputerIds()
        {
            string computerIDs = "1001, 2001, 3001,4001,5001";

            testObject = new ComputerActionPolicy(computerIDs);

            for (int i = 0; i < 5; i++)
            {
                long computerId = 1001 + 1000 * i;
                Assert.True(testObject.IsComputerAllowed(new ComputerEntity(computerId)));
            }
        }
    }
}
