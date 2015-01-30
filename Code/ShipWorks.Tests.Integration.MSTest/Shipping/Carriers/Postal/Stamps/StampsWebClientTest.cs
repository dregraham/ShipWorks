using System;
using System.Collections.Generic;
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

        public StampsWebClientTest()
        {
            // This will initialize all of the various static classes
            new StampsPrototypeFixture();
            
            accountRepository = new Mock<ICarrierAccountRepository<StampsAccountEntity>>();
            accountRepository.Setup(r => r.Accounts).Returns(new List<StampsAccountEntity>());

            logEntryFactory = new Mock<ILogEntryFactory>();
            logEntryFactory.Setup(f => f.GetLogEntry(It.IsAny<ApiLogSource>(), It.IsAny<string>(), It.IsAny<LogActionType>())).Returns(new Mock<IApiLogEntry>().Object);

            testObject = new StampsWebClient(accountRepository.Object, logEntryFactory.Object, new TrustingCertificateInspector(), StampsResellerType.StampsExpedited);
            StampsWebClient.UseTestServer = true;
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

    }
}
