﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration.Api.Request.Manipulators;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.InvoiceRegistration.Api.Request.Manipulators
{
    [TestClass]
    public class UpsInvoiceRegistrationNewProfileCredentialsManipulatorTest
    {
        UpsInvoiceRegistrationNewProfileCredentialsManipulator testObject;

        private CarrierRequest request;

        private RegisterRequest registerRequest;

        [TestInitialize]
        public void Initialize()
        {
            registerRequest=new RegisterRequest();

            Mock<CarrierRequest> mockRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(),null,registerRequest);

            request = mockRequest.Object;

            testObject = new UpsInvoiceRegistrationNewProfileCredentialsManipulator();           
        }

        [TestMethod]
        public void Manipulate_UsernameIsPopulated_Test()
        {
            testObject.Manipulate(request);

            Assert.IsFalse(string.IsNullOrWhiteSpace(registerRequest.Username));
        }

        [TestMethod]
        public void Manipulate_PasswordIsPopulated_Test()
        {
            testObject.Manipulate(request);

            Assert.IsFalse(string.IsNullOrWhiteSpace(registerRequest.Password));
        }

        [TestMethod]
        public void Manipulate_SuggestUserNameIndicatorCorrect_Test()
        {
            testObject.Manipulate(request);

            Assert.AreEqual("N", registerRequest.SuggestUsernameIndicator);
        }

        [TestMethod]
        public void Manipulate_NotificationCodeCorrect_Test()
        {
            testObject.Manipulate(request);

            Assert.AreEqual("00", registerRequest.NotificationCode);
        }

        [TestMethod]
        public void Manipulate_UsernameIsDifferentAfterSecondCall_Test()
        {
            testObject.Manipulate(request);

            string firstUsername = registerRequest.Username;

            testObject.Manipulate(request);

            Assert.AreNotEqual(firstUsername, registerRequest.Username);
        }
    }
}
