﻿using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api.Request;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api.Request.Manipulators;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.OpenAccount.Api
{
    [TestClass]
    public class UpsOpenAccountRequestFactoryTest
    {
        private UpsOpenAccountRequestFactory testObject;

        [TestInitialize]
        public void Initialize()
        {
            testObject = new UpsOpenAccountRequestFactory();
        }

        [TestMethod]
        public void CreateOpenAccountRequest_ReturnsUpsOpenAccountRequest_Test()
        {
            CarrierRequest request = testObject.CreateOpenAccountRequest(new OpenAccountRequest());

            Assert.IsInstanceOfType(request, typeof(UpsOpenAccountRequest));
        }

        [TestMethod]
        public void CreateOpenAccountRequest_PopulatesManipulators_Test()
        {
            CarrierRequest request = testObject.CreateOpenAccountRequest(new OpenAccountRequest()) as UpsOpenAccountRequest;

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.AreEqual(1, request.Manipulators.Count());
        }

        [TestMethod]
        public void CreateOpenAccountRequest_AddsUpsOpenAccountAddEndUserInformation_Test()
        {
            CarrierRequest request = testObject.CreateOpenAccountRequest(new OpenAccountRequest()) as UpsOpenAccountRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(UpsOpenAccountAddEndUserInformation)) == 1);
        }
    }
}