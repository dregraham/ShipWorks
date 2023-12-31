﻿using System;
using System.Collections.Generic;
using Interapptive.Shared.Net;
using Moq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using Xunit;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.Postal.Usps
{
    public class UspsWebClientTest
    {
        private readonly UspsWebClient testObject;

        private readonly Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>> accountRepository;
        private readonly Mock<ILogEntryFactory> logEntryFactory;

        private readonly UspsAccountEntity account;

        public UspsWebClientTest()
        {
            // This will initialize all of the various static classes
            new UspsPrototypeFixture();

            accountRepository = new Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>();
            accountRepository.Setup(r => r.Accounts).Returns(new List<UspsAccountEntity>());

            account = new UspsAccountEntity()
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
            IUspsWebServiceFactory webServiceFactory = new UspsWebServiceFactory(logEntryFactory.Object);
            logEntryFactory.Setup(f => f.GetLogEntry(It.IsAny<ApiLogSource>(), It.IsAny<string>(), It.IsAny<LogActionType>())).Returns(new Mock<IApiLogEntry>().Object);

            testObject = new UspsWebClient(accountRepository.Object, webServiceFactory, new TrustingCertificateInspector(), UspsResellerType.None);
            UspsWebClient.UseTestServer = true;
        }

        [Trait("Category", "USPS")]
        //[Trait("Category", "ContinuousIntegration")]
        [Fact]
        public void GetAccountInfo_ReturnsAccountInformation()
        {
            AccountInfoV65 info = testObject.GetAccountInfo(account) as AccountInfoV65;

            // Basically just a connectivity test to confirm that the web client is not broken
            Assert.NotNull(info);
        }

        [Trait("Category", "USPS")]
        //[Trait("Category", "ContinuousIntegration")]
        [Fact]
        public void CreateScanForm_Connectivity()
        {
            try
            {
                // Checking for basic connectivity with the API to make sure there haven't been
                // any breaking changes between the web client and API for scan forms
                List<UspsShipmentEntity> shipments = new List<UspsShipmentEntity>
                {
                    new UspsShipmentEntity
                    {
                        UspsTransactionID = Guid.NewGuid(),
                        PostalShipment = new PostalShipmentEntity
                        {
                            Service = (int) PostalServiceType.PriorityMail
                        }
                    },
                    new UspsShipmentEntity
                    {
                        UspsTransactionID = Guid.NewGuid(),
                        PostalShipment = new PostalShipmentEntity
                        {
                            Service = (int) PostalServiceType.PriorityMail
                        }
                    }
                };

                testObject.CreateScanForm(shipments, account);
            }
            catch (UspsApiException exception)
            {
                // Since we just dummied some data up to create a scan form with we're going
                // to get an exception. Make sure the error code is that of an invalid transaction ID
                Assert.Equal(4523265, exception.Code);
            }
        }

        [Trait("Category", "USPS")]
        //[Trait("Category", "ContinuousIntegration")]
        [Fact]
        public void RegisterAccount_IsNotSuccessful_WhenUsernameExists()
        {
            UspsRegistration registration = CreateRegistrationWithoutUsername();
            registration.UserName = "interapptive";

            UspsRegistrationResult registrationResult = testObject.RegisterAccount(registration);

            Assert.False(registrationResult.IsSuccessful);
        }

        [Trait("Category", "USPS")]
        //[Trait("Category", "ContinuousIntegration")]
        [Fact]
        public void RegisterAccount_IsSuccessful()
        {
            UspsRegistration registration = CreateRegistrationWithoutUsername();
            registration.UserName = DateTime.UtcNow.Ticks.ToString();

            UspsRegistrationResult registrationResult = testObject.RegisterAccount(registration);

            Assert.True(registrationResult.IsSuccessful);
        }

        /// <summary>
        /// Helper method to create a Stamps.com registration without a username.
        /// </summary>
        private UspsRegistration CreateRegistrationWithoutUsername()
        {
            Mock<IUspsRegistrationValidator> validator = new Mock<IUspsRegistrationValidator>();
            validator.Setup(v => v.Validate(It.IsAny<UspsRegistration>())).Returns(new List<RegistrationValidationError>());

            Mock<IUspsRegistrationGateway> gateway = new Mock<IUspsRegistrationGateway>();

            Mock<IRegistrationPromotion> promotion = new Mock<IRegistrationPromotion>();
            promotion.Setup(p => p.GetPromoCode()).Returns(string.Empty);

            UspsRegistration registration = new UspsRegistration(validator.Object, gateway.Object, promotion.Object);
            registration.UsageType = AccountType.OfficeBasedBusiness;
            registration.CreditCard = new CreditCard()
            {
                AccountNumber = "4111111111111111",
                BillingAddress = new Address() { Address1 = "1 Memorial Drive", City = "St. Louis", State = "MO", PostalCode = "63102", FirstName = "ShipWorks", LastName = "Interapptive", PhoneNumber = "5555555555", ZIPCode = "63102" },
                CreditCardType = CreditCardType.Visa,
                ExpirationDate = DateTime.Now.AddYears(3)
            };
            registration.FirstCodewordType = CodewordType.MothersMaidenName;
            registration.FirstCodewordValue = "mom";
            registration.SecondCodewordType = CodewordType.PetsName;
            registration.SecondCodewordValue = "Max";
            registration.Email = "some.one@shipworks.com";
            registration.MachineInfo = new MachineInfo() { IPAddress = "127.0.0.1" };
            registration.MailingAddress = registration.CreditCard.BillingAddress;
            registration.Password = "abc123456xyz";
            registration.RegistrationType = PostalAccountRegistrationType.Expedited;
            registration.PhysicalAddress = registration.CreditCard.BillingAddress;
            registration.FirstCodewordType = CodewordType.Last4DriversLicense;
            registration.FirstCodewordValue = "9933";
            registration.SecondCodewordType = CodewordType.FirstCarsMakeModel;
            registration.SecondCodewordValue = "Foo Bar";
            return registration;
        }

        [Trait("Category", "USPS")]
        //[Trait("Category", "ContinuousIntegration")]
        [Fact]
        public void GetContract_Connectivity()
        {
            UspsAccountContractType contractType = testObject.GetContractType(account);

            Assert.Equal(UspsAccountContractType.Reseller, contractType);
        }

        [Trait("Category", "USPS")]
        //[Trait("Category", "ContinuousIntegration")]
        [Fact]
        public void ChangeToExpeditedPlan_Connectivity()
        {
            // We just need to make sure we can connect and submit the request without
            // an exception. This will throw an exception since we're trying to convert
            // an existing reseller account.
            try
            {
                testObject.ChangeToExpeditedPlan(account, "ShipWorks3");
            }
            catch (UspsApiException exception)
            {
                Assert.Equal(0x005f0302, exception.Code);
            }
        }

    }
}
