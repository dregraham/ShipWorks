using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Interapptive.Shared.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Api;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Registration;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.Postal.Stamps
{
    [TestClass]
    public class StampsWebClientTest
    {
        private readonly StampsWebClient testObject;

        private readonly Mock<ICarrierAccountRepository<StampsAccountEntity>> accountRepository;
        private readonly Mock<ILogEntryFactory> logEntryFactory;

        private readonly StampsAccountEntity account;

        public StampsWebClientTest()
        {
            // This will initialize all of the various static classes
            new StampsPrototypeFixture();
            
            accountRepository = new Mock<ICarrierAccountRepository<StampsAccountEntity>>();
            accountRepository.Setup(r => r.Accounts).Returns(new List<StampsAccountEntity>());

            account = new StampsAccountEntity()
            {
                Username = "interapptive",
                Password = "AYSaiZOMP3UcalGuDB+4aA==",
                FirstName = "Interapptive",
                LastName = "ShipWorks",
                Street1 = "1 Memorial Drive",
                Street2 = "Suite 2000",
                City = "St. Louis",
                StateProvCode = "MO",
                PostalCode = "63102"
            };

            logEntryFactory = new Mock<ILogEntryFactory>();
            logEntryFactory.Setup(f => f.GetLogEntry(It.IsAny<ApiLogSource>(), It.IsAny<string>(), It.IsAny<LogActionType>())).Returns(new Mock<IApiLogEntry>().Object);

            testObject = new StampsWebClient(accountRepository.Object, logEntryFactory.Object, new TrustingCertificateInspector(), StampsResellerType.StampsExpedited);
            StampsWebClient.UseTestServer = true;
        }

        [TestCategory("Stamps")]
        [TestCategory("ContinuousIntegration")]
        [TestMethod]
        public void GetAccountInfo_ReturnsAccountInformation_Test()
        {           
            AccountInfo info = testObject.GetAccountInfo(account) as AccountInfo;

            // Basically just a connectivity test to confirm that the web client is not broken
            Assert.IsNotNull(info);
        }

        [TestCategory("Stamps")]
        [TestCategory("ContinuousIntegration")]
        [TestMethod]
        public void CreateScanForm_Connectivity_Test()
        {
            try
            {
                // Checking for basic connectivity with the API to make sure there haven't been 
                // any breaking changes between the web client and API for scan forms
                List<StampsShipmentEntity> shipments = new List<StampsShipmentEntity>
                {
                    new StampsShipmentEntity { StampsTransactionID = Guid.NewGuid() },
                    new StampsShipmentEntity { StampsTransactionID = Guid.NewGuid() }
                };

                testObject.CreateScanForm(shipments, account);
            }
            catch (StampsApiException exception)
            {
                // Since we just dummied some data up to create a scan form with we're going
                // to get an exception. Make sure the error code is that of an invalid transaction ID
                Assert.AreEqual(4523265, exception.Code);
            }
        }

        [TestCategory("Stamps")]
        [TestCategory("ContinuousIntegration")]
        [TestMethod]
        public void RegisterAccount_IsNotSuccessful_WhenUsernameExists_Test()
        {
            StampsRegistration registration = CreateRegistrationWithoutUsername();
            registration.UserName = "interapptive";

            StampsRegistrationResult registrationResult = testObject.RegisterAccount(registration);

            Assert.IsFalse(registrationResult.IsSuccessful);
        }

        [TestCategory("Stamps")]
        [TestCategory("ContinuousIntegration")]
        [TestMethod]
        public void RegisterAccount_IsSuccessful_Test()
        {
            StampsRegistration registration = CreateRegistrationWithoutUsername();
            registration.UserName = DateTime.UtcNow.Ticks.ToString();

            StampsRegistrationResult registrationResult = testObject.RegisterAccount(registration);

            Assert.IsTrue(registrationResult.IsSuccessful);
        }

        /// <summary>
        /// Helper method to create a Stamps.com registration without a username.
        /// </summary>
        /// <returns>StampsRegistration.</returns>
        private StampsRegistration CreateRegistrationWithoutUsername()
        {
            Mock<IStampsRegistrationValidator> validator = new Mock<IStampsRegistrationValidator>();
            validator.Setup(v => v.Validate(It.IsAny<StampsRegistration>())).Returns(new List<RegistrationValidationError>());

            Mock<IStampsRegistrationGateway> gateway = new Mock<IStampsRegistrationGateway>();

            Mock<IRegistrationPromotion> promotion = new Mock<IRegistrationPromotion>();
            promotion.Setup(p => p.GetPromoCode(It.IsAny<PostalAccountRegistrationType>())).Returns(string.Empty);

            StampsRegistration registration = new StampsRegistration(validator.Object, gateway.Object, promotion.Object);            
            registration.UsageType = AccountType.OfficeBasedBusiness;
            registration.CreditCard = new CreditCard()
            {
                AccountNumber = "4111111111111111",
                BillingAddress = new Address() { Address1 = "1 Memorial Drive", City = "St. Louis", State = "MO", PostalCode = "63102", FirstName = "ShipWorks", LastName = "Interapptive", PhoneNumber = "5555555555", ZIPCode = "63102" },
                CreditCardType = CreditCardType.Visa,
                ExpirationDate = DateTime.Now.AddYears(3)
            };
            registration.Email = "some.one@shipworks.com";
            registration.MachineInfo = new MachineInfo() { IPAddress = "127.0.0.1" };
            registration.MailingAddress = registration.CreditCard.BillingAddress;
            registration.Password = "abc123456xyz";
            registration.RegistrationType = PostalAccountRegistrationType.Expedited;
            registration.PhysicalAddress = registration.CreditCard.BillingAddress;

            return registration;
        }

        [TestCategory("Stamps")]
        [TestCategory("ContinuousIntegration")]
        [TestMethod]
        public void GetContract_Connectivity_Test()
        {
            StampsAccountContractType contractType = testObject.GetContractType(account);

            Assert.AreEqual(StampsAccountContractType.Reseller, contractType);
        }

        [TestCategory("Stamps")]
        [TestCategory("ContinuousIntegration")]
        [TestMethod]
        public void ChangeToExpeditedPlan_Connectivity_Test()
        {
            // We just need to make sure we can connect and submit the request without
            // an exception. This will throw an exception since we're trying to convert
            // an existing reseller account.
            try
            {
                testObject.ChangeToExpeditedPlan(account, "ShipWorks3");
            }
            catch (StampsApiException exception)
            {
                Assert.AreEqual(0x005f0302, exception.Code);
            }
        }

    }
}
