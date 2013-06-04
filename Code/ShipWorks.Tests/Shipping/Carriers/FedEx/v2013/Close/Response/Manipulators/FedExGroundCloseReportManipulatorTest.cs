﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Close.Response.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013;
using Moq;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Close.Response;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Close;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Close.Response.Manipulators
{
    [TestClass]
    public class FedExGroundCloseReportManipulatorTest
    {
        private FedExGroundCloseReportManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private Mock<ICarrierResponse> carrierResponse;
        private Mock<IFedExEndOfDayCloseRepository> closeRepository;

        private FedExAccountEntity account;
        private FedExEndOfDayCloseEntity closeEntity;

        [TestInitialize]
        public void Initialize()
        {
            account = new FedExAccountEntity {AccountNumber = "12345", FedExAccountID = 1001};
            closeEntity = new FedExEndOfDayCloseEntity();

            // Setup the carrier request to return the preconfigured account since this will be 
            // used in the manipulator to obtain the account number and account ID
            carrierRequest = new Mock<CarrierRequest>(null, null);
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns(account);

            carrierResponse = new Mock<ICarrierResponse>();
            carrierResponse.Setup(r => r.Request).Returns(carrierRequest.Object);

            closeRepository = new Mock<IFedExEndOfDayCloseRepository>();
            closeRepository.Setup(r => r.Save(It.IsAny<FedExEndOfDayCloseEntity>(), It.IsAny<GroundCloseReply>()));

            testObject = new FedExGroundCloseReportManipulator(closeRepository.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Manipulate_ThrowsArgumentNullException_WhenCloseEntityIsNull_Test()
        {
            closeEntity = null;

            testObject.Manipulate(carrierResponse.Object, closeEntity);

            Assert.IsNotNull(closeEntity);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenCarrierAccountIsNotFedEx_Test()
        {
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns(new UpsAccountEntity());

            testObject.Manipulate(carrierResponse.Object, closeEntity);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenCarrierAccountIsNull_Test()
        {
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns<IEntity2>(null);

            testObject.Manipulate(carrierResponse.Object, closeEntity);
        }

        [TestMethod]
        public void Manipulate_SetsFedExAccountId_Test()
        {
            testObject.Manipulate(carrierResponse.Object, closeEntity);

            Assert.AreEqual(account.FedExAccountID, closeEntity.FedExAccountID);
        }

        [TestMethod]
        public void Manipulate_SetsAccountNumber_Test()
        {
            testObject.Manipulate(carrierResponse.Object, closeEntity);

            Assert.AreEqual(account.AccountNumber, closeEntity.AccountNumber);
        }
        
        [TestMethod]
        public void Manipulate_SetsCloseDate_Test()
        {
            DateTime utcStart = DateTime.UtcNow;

            testObject.Manipulate(carrierResponse.Object, closeEntity);

            // Verify that the date assigned to the close date falls between the time the test started
            // and the after manipulate was completed
            Assert.IsTrue(utcStart.Ticks <= closeEntity.CloseDate.Ticks);
            Assert.IsTrue(closeEntity.CloseDate.Ticks <= DateTime.UtcNow.Ticks);
        }

        [TestMethod]
        public void Manipulate_IsSmartPostIsFalse_Test()
        {
            testObject.Manipulate(carrierResponse.Object, closeEntity);

            Assert.IsFalse(closeEntity.IsSmartPost);
        }

        [TestMethod]
        public void Manipulate_DelegatesToRepositoryWhenToSave_Test()
        {
            testObject.Manipulate(carrierResponse.Object, closeEntity);

            closeRepository.Verify(r => r.Save(closeEntity, It.IsAny<GroundCloseReply>()), Times.Once());
        }
    }
}
