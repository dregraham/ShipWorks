using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    [TestClass]
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
        }

        [TestInitialize]
        public void Initialize()
        {
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

        [TestMethod]
        public void DefaultConstructor_AddsEligibilityRule_Test()
        {
            // Testing the default constructor so just create a new Policy object for testing
            Policy policy = new Policy();
            Assert.IsNotNull(policy.Rules.Any(r => r.GetType() == typeof(EligibilityRule)));
        }

        [TestMethod]
        public void DefaultConstructor_AddsSelectedShippingMethodRule_Test()
        {
            // Testing the default constructor so just create a new Policy object for testing
            Policy policy = new Policy();
            Assert.IsNotNull(policy.Rules.Any(r => r.GetType() == typeof(SelectedShippingMethodRule)));
        }

        [TestMethod]
        public void DefaultConstructor_AddsTwoRules_Test()
        {
            // Testing the default constructor so just create a new Policy object for testing
            Policy policy = new Policy();
            Assert.AreEqual(2, policy.Rules.Count());
        }

        [TestMethod]
        public void EligibleForGlobalShippingProgram_DelegatesToEachRule_WhenAllRulesPass_Test()
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

        [TestMethod]
        public void EligibleForGlobalShippingProgram_ReturnsTrue_WhenAllRulesPass_Test()
        {
            testObject = new Policy(mockedAllPassingRules);
                        
            Assert.IsTrue(testObject.IsEligibleForGlobalShippingProgram(new EbayOrderEntity()));
        }

        [TestMethod]
        public void EligibleForGlobalShippingProgram_ReturnsFalse_WhenSomeRulesFail_Test()
        {
            // configure the test object to have a rule set where some rules pass and some fail
            testObject = new Policy(mockedMixtureOfRules);

            Assert.IsFalse(testObject.IsEligibleForGlobalShippingProgram(new EbayOrderEntity()));
        }

        [TestMethod]
        public void EligibleForGlobalShippingProgram_ReturnsFalse_WhenAllRulesFail_Test()
        {
            // configure the test object to have a rule set where all rules fail
            testObject = new Policy(mockedAllFailingRules);

            Assert.IsFalse(testObject.IsEligibleForGlobalShippingProgram(new EbayOrderEntity()));
        }

        [TestMethod]
        public void EligibleForGlobalShippingProgram_ReturnsFalse_WhenEbayOrderIsNull_Test()
        {
            Assert.IsFalse(testObject.IsEligibleForGlobalShippingProgram(null));
        }

        
        [TestMethod]
        public void ConfigureShipmentForGlobalShippingProgram_ShipCompanyIsReferenceID_Test()
        {
            // Nothing specific about the reference ID based on the carrier, so just use any carrier
            testObject.ConfigureShipmentForGlobalShippingProgram(endiciaShipment, shipmentOrder);

            Assert.AreEqual(shipmentOrder.GspReferenceID, endiciaShipment.ShipCompany);
        }

        [TestMethod]
        public void ConfigureShipmentForGlobalShippingProgram_ShippingAddressIsSwapped_Test()
        {
            // Nothing specific about the address based on the carrier, so just use any carrier
            testObject.ConfigureShipmentForGlobalShippingProgram(endiciaShipment, shipmentOrder);

            Assert.AreEqual(shipmentOrder.GspStreet1, endiciaShipment.ShipStreet1);
            Assert.AreEqual(shipmentOrder.GspStreet2, endiciaShipment.ShipStreet2);
            Assert.AreEqual(string.Empty, endiciaShipment.ShipStreet3);

            Assert.AreEqual(shipmentOrder.GspCity, endiciaShipment.ShipCity);
            Assert.AreEqual(shipmentOrder.GspStateProvince, endiciaShipment.ShipStateProvCode);
            Assert.AreEqual(shipmentOrder.GspPostalCode, endiciaShipment.ShipPostalCode);
            Assert.AreEqual(shipmentOrder.GspCountryCode, endiciaShipment.ShipCountryCode);
        }

        [TestMethod]
        public void ConfigureShipmentForGlobalShippingProgram_ContactInfoIsWiped_Test()
        {
            // Nothing specific about the phone/email based on the carrier, so just use any carrier
            testObject.ConfigureShipmentForGlobalShippingProgram(endiciaShipment, shipmentOrder);

            Assert.AreEqual(string.Empty, endiciaShipment.ShipPhone);
            Assert.AreEqual(string.Empty, endiciaShipment.ShipEmail);
        }

        [TestMethod]
        public void ConfigureShipmentForGlobalShippingProgram_RecipientNameIsWiped_WhenShippingWithEndicia_Test()
        {
            testObject.ConfigureShipmentForGlobalShippingProgram(endiciaShipment, shipmentOrder);

            Assert.AreEqual(string.Empty, endiciaShipment.ShipFirstName);
            Assert.AreEqual(string.Empty, endiciaShipment.ShipMiddleName);
            Assert.AreEqual(string.Empty, endiciaShipment.ShipLastName);
        }

        [TestMethod]
        public void ConfigureShipmentForGlobalShippingProgram_RecipientNameIsWiped_WhenShippingWithExpress1_Test()
        {
            testObject.ConfigureShipmentForGlobalShippingProgram(express1Shipment, shipmentOrder);

            Assert.AreEqual(string.Empty, express1Shipment.ShipFirstName);
            Assert.AreEqual(string.Empty, express1Shipment.ShipMiddleName);
            Assert.AreEqual(string.Empty, express1Shipment.ShipLastName);
        }

        [TestMethod]
        public void ConfigureShipmentForGlobalShippingProgram_RecipientNameIsReplacedWithGspId_WhenShippingWithUpsSurePost_Test()
        {
            foreach (UpsServiceType service in UpsUtility.SurePostShipmentTypes)
            {
                upsShipment.Ups.Service = (int)service;

                testObject.ConfigureShipmentForGlobalShippingProgram(upsShipment, shipmentOrder);

                string testingDescription = string.Format("Testing {0}", EnumHelper.GetDescription(service));
                Assert.AreEqual(string.Empty, upsShipment.ShipFirstName, testingDescription);
                Assert.AreEqual(string.Empty, upsShipment.ShipMiddleName, testingDescription);
                Assert.AreEqual(shipmentOrder.GspReferenceID, upsShipment.ShipLastName, testingDescription);
            }
        }

        [TestMethod]
        public void ConfigureShipmentForGlobalShippingProgram_RecipientNameIsGSPName_WhenShippingWithCarrierOtherThanExpress1Endicia_Test()
        {
            testObject.ConfigureShipmentForGlobalShippingProgram(uspsShipment, shipmentOrder);

            Assert.AreEqual(shipmentOrder.GspFirstName, uspsShipment.ShipFirstName);
            Assert.AreEqual(string.Empty, uspsShipment.ShipMiddleName);
            Assert.AreEqual(shipmentOrder.GspLastName, uspsShipment.ShipLastName);
        }

        [TestMethod]
        public void ConfigureShipmentForGlobalShippingProgram_AddressVerificationIsNotRequired_WhenShippingWithUsps_Test()
        {
            testObject.ConfigureShipmentForGlobalShippingProgram(uspsShipment, shipmentOrder);

            Assert.IsFalse(uspsShipment.Postal.Usps.RequireFullAddressValidation);
        }

        [TestMethod]
        public void ConfigureShipmentForGlobalShippingProgram_PhoneNumberIsNotWiped_WhenShippingWithFedEx_Test()
        {
            testObject.ConfigureShipmentForGlobalShippingProgram(fedexShipment, shipmentOrder);

            Assert.AreEqual(PhoneNumber, fedexShipment.ShipPhone);
        }

        [TestMethod]
        public void ConfigureShipmentForGlobalShippingProgram_PhoneNumberIsWiped_WhenNotShippingWithFedEx_Test()
        {
            testObject.ConfigureShipmentForGlobalShippingProgram(uspsShipment, shipmentOrder);

            Assert.AreEqual(string.Empty, uspsShipment.ShipPhone);
        }

        [TestMethod]
        public void ConfigureShipmentForGlobalShippingProgram_EmailIsWiped_Test()
        {
            // This is not carrier specific, so any carrier will do
            testObject.ConfigureShipmentForGlobalShippingProgram(uspsShipment, shipmentOrder);

            Assert.AreEqual(string.Empty, uspsShipment.ShipEmail);
        }

        [TestMethod]
        public void ConfigureShipmentForGlobalShippingProgram_ReturnsCorrectModifiedFieldCount_WhenNotFedExShipment_Test()
        {
            List<ShipmentFieldIndex> modifiedFields = testObject.ConfigureShipmentForGlobalShippingProgram(uspsShipment, shipmentOrder);
            Assert.AreEqual(13, modifiedFields.Count);
        }

        [TestMethod]
        public void ConfigureShipmentForGlobalShippingProgram_ReturnsCorrectModifiedFieldCount_WhenFedExShipment_Test()
        {
            List<ShipmentFieldIndex> modifiedFields = testObject.ConfigureShipmentForGlobalShippingProgram(fedexShipment, shipmentOrder);
            Assert.AreEqual(12, modifiedFields.Count);
        }

        [TestMethod]
        public void ConfigureShipmentForGlobalShippingProgram_ShipPostalCodeIsFiveDigits_WhenGspPostalCodeValueExceedsFiveDigits_Test()
        {
            shipmentOrder.GspPostalCode = "41018-3190";

            // This is not carrier specific, so any carrier will do
            testObject.ConfigureShipmentForGlobalShippingProgram(uspsShipment, shipmentOrder);

            Assert.AreEqual("41018", uspsShipment.ShipPostalCode);
        }

        [TestMethod]
        public void ConfigureShipmentForGlobalShippingProgram_ShipPostalCodeIsFiveDigits_WhenGspPostalCodeValueExceedsFiveDigits_AndIsInvalidPostalCode_Test()
        {
            // This was an actual postal code that eBay sent down in responses, so test it out here
            shipmentOrder.GspPostalCode = "41018-319";

            // This is not carrier specific, so any carrier will do
            testObject.ConfigureShipmentForGlobalShippingProgram(uspsShipment, shipmentOrder);

            Assert.AreEqual("41018", uspsShipment.ShipPostalCode);
        }
    }
}
