using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api.Response;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api.Response.Manipulators;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;
using ShipWorks.Stores.Platforms.Amazon.WebServices.Associates;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.OpenAccount.Api.Response
{
    [TestClass]
    public class UpsOpenAccountCreateUpsAccountEntityManipulatorTest
    {
        private UpsOpenAccountCreateUpsAccountEntityManipulator testObject;
        private UpsOpenAccountResponse upsOpenAccountResponse;
        private Mock<CarrierRequest> carrierRequest;
        private CodeOnlyType pickupCode;
        private OpenAccountResponse openAccountResponse;
        private CodeDescriptionType responseCode;
        private UpsAccountEntity upsAccount;

        [TestInitialize]
        public void Initialize()
        {
            testObject = new UpsOpenAccountCreateUpsAccountEntityManipulator();

            responseCode = new CodeDescriptionType() { Code = EnumHelper.GetApiValue(UpsOpenAccountResponseStatusCode.Success) };

            openAccountResponse = new OpenAccountResponse()
            {
                Response = new ResponseType() { ResponseStatus = responseCode , Alert = new CodeDescriptionType[0]}
            };

            pickupCode = new CodeOnlyType() { Code = EnumHelper.GetApiValue(UpsPickupOption.DailyOnRoute) };

            OpenAccountRequest nativeRequest = new OpenAccountRequest() { PickupInformation = new PickupInformationType() { PickupOption = pickupCode } };
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);
           
            upsAccount = new UpsAccountEntity()
            {
                Street1 = "test street",
                AccountNumber = "42",
                PostalCode = "90210"
            };

            carrierRequest
                .Setup(c => c.CarrierAccountEntity)
                .Returns(upsAccount);

            upsOpenAccountResponse = new UpsOpenAccountResponse(openAccountResponse, carrierRequest.Object, new List<ICarrierResponseManipulator>());
            
        }

        [TestMethod]
        [ExpectedException(typeof(UpsOpenAccountBusinessAddressException))]
        public void UpsOpenAccountCreateUpsAccountEntityManipulator_ThrowsUpsOpenAccountBusinessAddressException_ResponseContainsBillingCandidate_Test()
        {
            openAccountResponse.BillingAddressCandidate = new AddressKeyCandidateType();

            testObject.Manipulate(upsOpenAccountResponse);
        }

        [TestMethod]
        [ExpectedException(typeof(UpsOpenAccountPickupAddressException))]
        public void UpsOpenAccountCreateUpsAccountEntityManipulator_ThrowsUpsOpenAccountPickupAddressException_ResponseContainsPickupAddressCandidates_Test()
        {
            openAccountResponse.PickupAddressCandidate = new AddressKeyCandidateType();

            testObject.Manipulate(upsOpenAccountResponse);
        }

        [TestMethod]
        [ExpectedException(typeof(UpsOpenAccountException))]
        public void UpsOpenAccountCreateUpsAccountEntityManipulator_ThrowsUpsOpenAccountException_ResponseContainsFailedCode_Test()
        {
            responseCode.Code = EnumHelper.GetApiValue(UpsOpenAccountResponseStatusCode.Failed);

            testObject.Manipulate(upsOpenAccountResponse);
        }

        [TestMethod]
        public void UpsOpenAccountCreateUpsAccountEntityManipulator_SetsAccountNumber_Test()
        {
            testObject.Manipulate(upsOpenAccountResponse);

            Assert.AreEqual("42", upsAccount.AccountNumber);
        }

        [TestMethod]
        public void UpsOpenAccountCreateUpsAccountEntityManipulator_SetsRateTypeToOccasional_PickupOptionIsSmart_Test()
        {
            pickupCode.Code = EnumHelper.GetApiValue(UpsPickupOption.SmartPickup);

            testObject.Manipulate(upsOpenAccountResponse);

            Assert.AreEqual((int) UpsRateType.Occasional, upsAccount.RateType);
        }

        [TestMethod]
        public void UpsOpenAccountCreateUpsAccountEntityManipulator_SetsRateTypeToDaily_PickupOptionIsDaily_Test()
        {
            pickupCode.Code = EnumHelper.GetApiValue(UpsPickupOption.RegularDailyPickup);

            testObject.Manipulate(upsOpenAccountResponse);

            Assert.AreEqual((int) UpsRateType.DailyPickup, upsAccount.RateType);
        }
    }
}
