﻿using System.Linq;
using System.Xml;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Editions;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Policies;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing
{
    public class LicenseCapabilitiesTest
    {
        [Fact]
        public void LicenseCapabilities_ThrowsLicenseException_WhenResponseContainsError()
        {
            string xmlWithError =
                "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><Error>Some random error</Error></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithError);

            ShipWorksLicenseException ex = Assert.Throws<ShipWorksLicenseException>(() => new LicenseCapabilities(xmlResponse));
            Assert.Equal("Some random error", ex.Message);
        }

        [Fact]
        public void LicenseCapabilities_RestrictsPurchasingPostage_WhenResponseContainsPurchasePostageRestriction()
        {
            string xmlWithEndiciaPurchasePostageRestrictions =
                "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithEndiciaPurchasePostageRestrictions);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.ShipmentTypeRestriction.ContainsKey(ShipmentTypeCode.Endicia));
            Assert.Equal(true, licenseCapabilities.ShipmentTypeRestriction[ShipmentTypeCode.Endicia].Contains(ShipmentTypeRestrictionType.Purchasing));
        }

        [Fact]
        public void LicenseCapabilities_RestrictsAccountRegistration_WhenResponseContainsAccountRegistrationRestriction()
        {
            string xmlWithEndiciaAccountRegistrationRestrictions =
                "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithEndiciaAccountRegistrationRestrictions);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.ShipmentTypeRestriction.ContainsKey(ShipmentTypeCode.Endicia));
            Assert.Equal(true, licenseCapabilities.ShipmentTypeRestriction[ShipmentTypeCode.Endicia].Contains(ShipmentTypeRestrictionType.AccountRegistration));
        }

        [Fact]
        public void LicenseCapabilities_DisablesShipmentType_WhenResponseContainsDisabledRestriction()
        {
            string xmlWithEndiciaDisabledRestrictions = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>Disabled</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";
            
            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithEndiciaDisabledRestrictions);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.ShipmentTypeRestriction.ContainsKey(ShipmentTypeCode.Endicia));
            Assert.Equal(true, licenseCapabilities.ShipmentTypeRestriction[ShipmentTypeCode.Endicia].Contains(ShipmentTypeRestrictionType.Disabled));
        }

        [Fact]
        public void LicenseCapabilities_DoesNotAddRestrictions_WhenResponseContainsInvalidRestriction()
        {
            string xmlWithInvalidShipmentTypeRestrictions = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><CustomerStatus><ShipmentTypeFunctionality><ShipmentType TypeCode='2'><Restriction>Something Wrong</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithInvalidShipmentTypeRestrictions);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.ShipmentTypeRestriction.ContainsKey(ShipmentTypeCode.Endicia));
            Assert.Equal(false, licenseCapabilities.ShipmentTypeRestriction[ShipmentTypeCode.Endicia].Contains(ShipmentTypeRestrictionType.Disabled));
        }

        [Fact]
        public void LicenseCapabilities_DoesNotAddFeature_WhenResponseContainsInvalidFeature()
        {
            string xmlWithInvalidShipmentTypeFeature = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><CustomerStatus><ShipmentTypeFunctionality><ShipmentType TypeCode='2'><Feature>Something Wrong</Feature></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithInvalidShipmentTypeFeature);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.ShipmentTypeShippingPolicy.ContainsKey(ShipmentTypeCode.Endicia));
            Assert.Equal(0, licenseCapabilities.ShipmentTypeShippingPolicy[ShipmentTypeCode.Endicia].Count);
        }

        [Fact]
        public void LicenseCapabilities_AddsBestRateUpsRestriction_WhenResponseContainsBestRateUpsRestriction()
        {
            string xmlWithBestRateUpsRestrictionShipmentTypeFeature = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><CustomerStatus><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Feature><Type>BestRateUpsRestriction</Type><Config>True</Config></Feature><Feature><Type>RateResultCount</Type><Config>5</Config></Feature></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithBestRateUpsRestrictionShipmentTypeFeature);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.ShipmentTypeShippingPolicy.ContainsKey(ShipmentTypeCode.BestRate));
            Assert.Equal("True", licenseCapabilities.ShipmentTypeShippingPolicy[ShipmentTypeCode.BestRate][ShippingPolicyType.BestRateUpsRestriction]);
        }

        [Fact]
        public void LicenseCapabilities_AddsRateResultCountFeature_WhenResponseContainsRateResultCountFeature()
        {
            string xmlWithRateResultCountShipmentTypeFeature = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><CustomerStatus><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Feature><Type>BestRateUpsRestriction</Type><Config>True</Config></Feature><Feature><Type>RateResultCount</Type><Config>5</Config></Feature></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithRateResultCountShipmentTypeFeature);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.ShipmentTypeShippingPolicy.ContainsKey(ShipmentTypeCode.BestRate));
            Assert.Equal("5", licenseCapabilities.ShipmentTypeShippingPolicy[ShipmentTypeCode.BestRate][ShippingPolicyType.RateResultCount]);
        }

        [Fact]
        public void LicenseCapabilities_SetsIsInTrial_WhenResponseContainsIsInTrial()
        {
            string xmlWithIsInTrial = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><IsInTrial>true</IsInTrial></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithIsInTrial);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.IsInTrial);
        }

        [Fact]
        public void LicenseCapabilities_SetsIsInTrialToDefault_WhenResponseDoesNotContainsIsInTrial()
        {
            string xmlWithIsInTrial = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithIsInTrial);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(false, licenseCapabilities.IsInTrial);
        }

        [Fact]
        public void LicenseCapabilities_SetsCustomDataSources_WhenResponseContainsCustomDataSources()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>yes</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.CustomDataSources);
        }

        [Fact]
        public void LicenseCapabilities_SetsCustomDataSources_WhenResponseDoesNotContainsCustomDataSources()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(false, licenseCapabilities.CustomDataSources);
        }
    }
}