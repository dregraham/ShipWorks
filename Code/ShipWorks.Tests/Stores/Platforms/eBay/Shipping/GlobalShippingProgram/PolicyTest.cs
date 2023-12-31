﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using Xunit;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Platforms.Ebay.Shipping.GlobalShippingProgram;
using ShipWorks.Stores.Platforms.Ebay.Shipping.GlobalShippingProgram.Rules;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Ebay.Enums;
using ShipWorks.Shipping;
using Moq;
using ShipWorks.Data.Model;

namespace ShipWorks.Tests.Stores.eBay.GlobalShippingProgram
{
    public class PolicyTest
    {
        private const string PhoneNumber = "555-555-5555";
        private const string Email = "someone@somewhere.com";

        private Policy testObject;

        private ShipmentEntity endiciaShipment;
        private ShipmentEntity express1Shipment;
        private ShipmentEntity uspsShipment;
        private ShipmentEntity fedexShipment;
        private ShipmentEntity upsShipment;

        private EbayOrderEntity shipmentOrder;

        private List<IGlobalShippingProgramRule> mockedAllPassingRules;
        private List<IGlobalShippingProgramRule> mockedMixtureOfRules;
        private List<IGlobalShippingProgramRule> mockedAllFailingRules;

        public PolicyTest()
        {
            // Create a rule set for testing a policy with all rules that pass
            mockedAllPassingRules = new List<IGlobalShippingProgramRule>();
            for (int i = 0; i < 5; i++)
            {
                // Setup a mock rule so it always evaluates to true and add the mocked passing rule 
                // five times - this will be used to make sure all the rules get executed
                Mock<IGlobalShippingProgramRule> mockedPassingRule = new Mock<IGlobalShippingProgramRule>();
                mockedPassingRule.Setup(r => r.Evaluate(It.IsAny<EbayOrderEntity>())).Returns(true);

                mockedAllPassingRules.Add(mockedPassingRule.Object);
            }

            // Create a rule set for testing a policy with all rules that fail
            mockedAllFailingRules = new List<IGlobalShippingProgramRule>();
            for (int i = 0; i < 5; i++)
            {
                // Setup a mock rule so it always evaluates to false and add the mocked passing rule 
                // five times - this will be used to make sure all the rules get executed
                Mock<IGlobalShippingProgramRule> mockedFailingRule = new Mock<IGlobalShippingProgramRule>();
                mockedFailingRule.Setup(r => r.Evaluate(It.IsAny<EbayOrderEntity>())).Returns(false);

                mockedAllFailingRules.Add(mockedFailingRule.Object);
            }


            // Create a rule set for testing a policy with all some rules that pass and some that fail
            mockedMixtureOfRules = new List<IGlobalShippingProgramRule>();
            Mock<IGlobalShippingProgramRule> passingRule = new Mock<IGlobalShippingProgramRule>();
            passingRule.Setup(r=>r.Evaluate(It.IsAny<EbayOrderEntity>())).Returns(true);

            Mock<IGlobalShippingProgramRule> failingRule = new Mock<IGlobalShippingProgramRule>();
            failingRule.Setup(r => r.Evaluate(It.IsAny<EbayOrderEntity>())).Returns(false);

            mockedMixtureOfRules.Add(passingRule.Object);
            mockedMixtureOfRules.Add(failingRule.Object);

            shipmentOrder = new EbayOrderEntity()
            {
                // Just setup the GSP data of the order
                GspEligible = true,

                GspReferenceID = "Reference #123",
                GspFirstName = "GlobalFirstName",
                GspLastName = "GlobalLastName",
                GspStreet1 = "1850 Airport Exchange Blvd",
                GspStreet2 = "Suite 400",
                GspCity = "Erlanger",
                GspStateProvince = "KY",
                GspPostalCode = "41018",
                GspCountryCode = "US"
            };

            // Setup our test object will all passing rules since this will be the setup
            // required by most of our tests
            testObject = new Policy(mockedAllPassingRules);

            // Our shipments could change from test to test so make sure they're initialized before each test
            endiciaShipment = CreateShipment((int)ShipmentTypeCode.Endicia);

            express1Shipment = CreateShipment((int) ShipmentTypeCode.Express1Endicia);

            uspsShipment = CreateShipment((int) ShipmentTypeCode.Usps);
            uspsShipment.Postal = new PostalShipmentEntity()
            {
                Usps = new UspsShipmentEntity()
            };

            fedexShipment = CreateShipment((int) ShipmentTypeCode.FedEx);

            upsShipment = CreateShipment((int) ShipmentTypeCode.UpsOnLineTools);
            upsShipment.Ups = new UpsShipmentEntity();
        }

        private ShipmentEntity CreateShipment(int shipmentType)
        {
            return new ShipmentEntity
            {
                Order = shipmentOrder,
                ShipmentType = shipmentType,

                ShipFirstName = "John",
                ShipMiddleName = "Paul",
                ShipLastName = "Jones",

                ShipStreet1 = "123 Main Street",
                ShipStreet2 = "Suite 900",
                ShipStreet3 = "Sound Room 2",
                ShipCity = "Sidcup",
                ShipStateProvCode = "Kent",
                ShipCountryCode = "UK",
                ShipPostalCode = "DA14 5BU",

                ShipPhone = PhoneNumber,
                ShipEmail = Email
            };
        }

        [Fact]
        public void DefaultConstructor_AddsEligibilityRule()
        {
            // Testing the default constructor so just create a new Policy object for testing
            Policy policy = new Policy();
            Assert.NotNull(policy.Rules.Any(r => r.GetType() == typeof(EligibilityRule)));
        }

        [Fact]
        public void DefaultConstructor_AddsSelectedShippingMethodRule()
        {
            // Testing the default constructor so just create a new Policy object for testing
            Policy policy = new Policy();
            Assert.NotNull(policy.Rules.Any(r => r.GetType() == typeof(SelectedShippingMethodRule)));
        }

        [Fact]
        public void DefaultConstructor_AddsTwoRules()
        {
            // Testing the default constructor so just create a new Policy object for testing
            Policy policy = new Policy();
            Assert.Equal(2, policy.Rules.Count());
        }

        [Fact]
        public void EligibleForGlobalShippingProgram_DelegatesToEachRule_WhenAllRulesPass()
        {
            // test object is already configured in the Initialize method with all passing rules
            testObject = new Policy(mockedAllPassingRules);
            EbayOrderEntity ebayOrder = new EbayOrderEntity();

            testObject.IsEligibleForGlobalShippingProgram(ebayOrder);

            // Check that each rule in the list we provided was evaluated exactly once
            foreach (IGlobalShippingProgramRule rule in mockedAllPassingRules)
            {
                Mock<IGlobalShippingProgramRule> mockedRule = Moq.Mock.Get(rule);
                mockedRule.Verify(r => r.Evaluate(ebayOrder), Times.Once());
            }            
        }

        [Fact]
        public void EligibleForGlobalShippingProgram_ReturnsTrue_WhenAllRulesPass()
        {
            testObject = new Policy(mockedAllPassingRules);
                        
            Assert.True(testObject.IsEligibleForGlobalShippingProgram(new EbayOrderEntity()));
        }

        [Fact]
        public void EligibleForGlobalShippingProgram_ReturnsFalse_WhenSomeRulesFail()
        {
            // configure the test object to have a rule set where some rules pass and some fail
            testObject = new Policy(mockedMixtureOfRules);

            Assert.False(testObject.IsEligibleForGlobalShippingProgram(new EbayOrderEntity()));
        }

        [Fact]
        public void EligibleForGlobalShippingProgram_ReturnsFalse_WhenAllRulesFail()
        {
            // configure the test object to have a rule set where all rules fail
            testObject = new Policy(mockedAllFailingRules);

            Assert.False(testObject.IsEligibleForGlobalShippingProgram(new EbayOrderEntity()));
        }

        [Fact]
        public void EligibleForGlobalShippingProgram_ReturnsFalse_WhenEbayOrderIsNull()
        {
            Assert.False(testObject.IsEligibleForGlobalShippingProgram(null));
        }

        
        [Fact]
        public void ConfigureShipmentForGlobalShippingProgram_ShipCompanyIsReferenceID()
        {
            // Nothing specific about the reference ID based on the carrier, so just use any carrier
            testObject.ConfigureShipmentForGlobalShippingProgram(endiciaShipment, shipmentOrder);

            Assert.Equal(shipmentOrder.GspReferenceID, endiciaShipment.ShipCompany);
        }

        [Fact]
        public void ConfigureShipmentForGlobalShippingProgram_ShippingAddressIsSwapped()
        {
            // Nothing specific about the address based on the carrier, so just use any carrier
            testObject.ConfigureShipmentForGlobalShippingProgram(endiciaShipment, shipmentOrder);

            Assert.Equal(shipmentOrder.GspStreet1, endiciaShipment.ShipStreet1);
            Assert.Equal(shipmentOrder.GspStreet2, endiciaShipment.ShipStreet2);
            Assert.Equal(string.Empty, endiciaShipment.ShipStreet3);

            Assert.Equal(shipmentOrder.GspCity, endiciaShipment.ShipCity);
            Assert.Equal(shipmentOrder.GspStateProvince, endiciaShipment.ShipStateProvCode);
            Assert.Equal(shipmentOrder.GspPostalCode, endiciaShipment.ShipPostalCode);
            Assert.Equal(shipmentOrder.GspCountryCode, endiciaShipment.ShipCountryCode);
        }

        [Fact]
        public void ConfigureShipmentForGlobalShippingProgram_ContactInfoIsWiped()
        {
            // Nothing specific about the phone/email based on the carrier, so just use any carrier
            testObject.ConfigureShipmentForGlobalShippingProgram(endiciaShipment, shipmentOrder);

            Assert.Equal(string.Empty, endiciaShipment.ShipPhone);
            Assert.Equal(string.Empty, endiciaShipment.ShipEmail);
        }

        [Fact]
        public void ConfigureShipmentForGlobalShippingProgram_RecipientNameIsWiped_WhenShippingWithEndicia()
        {
            testObject.ConfigureShipmentForGlobalShippingProgram(endiciaShipment, shipmentOrder);

            Assert.Equal(string.Empty, endiciaShipment.ShipFirstName);
            Assert.Equal(string.Empty, endiciaShipment.ShipMiddleName);
            Assert.Equal(string.Empty, endiciaShipment.ShipLastName);
        }

        [Fact]
        public void ConfigureShipmentForGlobalShippingProgram_RecipientNameIsWiped_WhenShippingWithExpress1()
        {
            testObject.ConfigureShipmentForGlobalShippingProgram(express1Shipment, shipmentOrder);

            Assert.Equal(string.Empty, express1Shipment.ShipFirstName);
            Assert.Equal(string.Empty, express1Shipment.ShipMiddleName);
            Assert.Equal(string.Empty, express1Shipment.ShipLastName);
        }

        [Fact]
        public void ConfigureShipmentForGlobalShippingProgram_RecipientNameIsReplacedWithGspId_WhenShippingWithUpsSurePost()
        {
            foreach (UpsServiceType service in UpsUtility.SurePostShipmentTypes)
            {
                upsShipment.Ups.Service = (int)service;

                testObject.ConfigureShipmentForGlobalShippingProgram(upsShipment, shipmentOrder);

                string testingDescription = string.Format("Testing {0}", EnumHelper.GetDescription(service));
                Assert.Equal(string.Empty, upsShipment.ShipFirstName);
                Assert.Equal(string.Empty, upsShipment.ShipMiddleName);
                Assert.Equal(shipmentOrder.GspReferenceID, upsShipment.ShipLastName);
            }
        }

        [Fact]
        public void ConfigureShipmentForGlobalShippingProgram_RecipientNameIsGSPName_WhenShippingWithCarrierOtherThanExpress1Endicia()
        {
            testObject.ConfigureShipmentForGlobalShippingProgram(uspsShipment, shipmentOrder);

            Assert.Equal(shipmentOrder.GspFirstName, uspsShipment.ShipFirstName);
            Assert.Equal(string.Empty, uspsShipment.ShipMiddleName);
            Assert.Equal(shipmentOrder.GspLastName, uspsShipment.ShipLastName);
        }

        [Fact]
        public void ConfigureShipmentForGlobalShippingProgram_AddressVerificationIsNotRequired_WhenShippingWithUsps()
        {
            testObject.ConfigureShipmentForGlobalShippingProgram(uspsShipment, shipmentOrder);

            Assert.False(uspsShipment.Postal.Usps.RequireFullAddressValidation);
        }

        [Fact]
        public void ConfigureShipmentForGlobalShippingProgram_PhoneNumberIsNotWiped_WhenShippingWithFedEx()
        {
            testObject.ConfigureShipmentForGlobalShippingProgram(fedexShipment, shipmentOrder);

            Assert.Equal(PhoneNumber, fedexShipment.ShipPhone);
        }

        [Fact]
        public void ConfigureShipmentForGlobalShippingProgram_PhoneNumberIsWiped_WhenNotShippingWithFedEx()
        {
            testObject.ConfigureShipmentForGlobalShippingProgram(uspsShipment, shipmentOrder);

            Assert.Equal(string.Empty, uspsShipment.ShipPhone);
        }

        [Fact]
        public void ConfigureShipmentForGlobalShippingProgram_EmailIsWiped()
        {
            // This is not carrier specific, so any carrier will do
            testObject.ConfigureShipmentForGlobalShippingProgram(uspsShipment, shipmentOrder);

            Assert.Equal(string.Empty, uspsShipment.ShipEmail);
        }

        [Fact]
        public void ConfigureShipmentForGlobalShippingProgram_ReturnsCorrectModifiedFieldCount_WhenNotFedExShipment()
        {
            List<ShipmentFieldIndex> modifiedFields = testObject.ConfigureShipmentForGlobalShippingProgram(uspsShipment, shipmentOrder);
            Assert.Equal(13, modifiedFields.Count);
        }

        [Fact]
        public void ConfigureShipmentForGlobalShippingProgram_ReturnsCorrectModifiedFieldCount_WhenFedExShipment()
        {
            List<ShipmentFieldIndex> modifiedFields = testObject.ConfigureShipmentForGlobalShippingProgram(fedexShipment, shipmentOrder);
            Assert.Equal(12, modifiedFields.Count);
        }

        [Fact]
        public void ConfigureShipmentForGlobalShippingProgram_ShipPostalCodeIsFiveDigits_WhenGspPostalCodeValueExceedsFiveDigits()
        {
            shipmentOrder.GspPostalCode = "41018-3190";

            // This is not carrier specific, so any carrier will do
            testObject.ConfigureShipmentForGlobalShippingProgram(uspsShipment, shipmentOrder);

            Assert.Equal("41018", uspsShipment.ShipPostalCode);
        }

        [Fact]
        public void ConfigureShipmentForGlobalShippingProgram_ShipPostalCodeIsFiveDigits_WhenGspPostalCodeValueExceedsFiveDigits_AndIsInvalidPostalCode()
        {
            // This was an actual postal code that eBay sent down in responses, so test it out here
            shipmentOrder.GspPostalCode = "41018-319";

            // This is not carrier specific, so any carrier will do
            testObject.ConfigureShipmentForGlobalShippingProgram(uspsShipment, shipmentOrder);

            Assert.Equal("41018", uspsShipment.ShipPostalCode);
        }
    }
}
