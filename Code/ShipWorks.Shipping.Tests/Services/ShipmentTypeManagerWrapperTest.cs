using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services
{
    public class ShipmentTypeManagerWrapperTest
    {
        IShipmentTypeManager testObject;

        public ShipmentTypeManagerWrapperTest()
        {
            testObject = new ShipmentTypeManagerWrapper();
        }

        [Fact]
        public void ShipmentTypesSupportingAccounts_AreCorrect_Test()
        {
            Assert.False(testObject.ShipmentTypesSupportingAccounts.Contains(ShipmentTypeCode.BestRate));
            Assert.False(testObject.ShipmentTypesSupportingAccounts.Contains(ShipmentTypeCode.Other));
            Assert.False(testObject.ShipmentTypesSupportingAccounts.Contains(ShipmentTypeCode.PostalWebTools));
            Assert.False(testObject.ShipmentTypesSupportingAccounts.Contains(ShipmentTypeCode.None));

            Assert.True(testObject.ShipmentTypesSupportingAccounts.Contains(ShipmentTypeCode.OnTrac));
            Assert.True(testObject.ShipmentTypesSupportingAccounts.Contains(ShipmentTypeCode.Endicia));
            Assert.True(testObject.ShipmentTypesSupportingAccounts.Contains(ShipmentTypeCode.Express1Endicia));
            Assert.True(testObject.ShipmentTypesSupportingAccounts.Contains(ShipmentTypeCode.Express1Usps));
            Assert.True(testObject.ShipmentTypesSupportingAccounts.Contains(ShipmentTypeCode.FedEx));
            Assert.True(testObject.ShipmentTypesSupportingAccounts.Contains(ShipmentTypeCode.UpsOnLineTools));
            Assert.True(testObject.ShipmentTypesSupportingAccounts.Contains(ShipmentTypeCode.UpsWorldShip));
            Assert.True(testObject.ShipmentTypesSupportingAccounts.Contains(ShipmentTypeCode.Usps));
            Assert.True(testObject.ShipmentTypesSupportingAccounts.Contains(ShipmentTypeCode.iParcel));
        }
    }
}
