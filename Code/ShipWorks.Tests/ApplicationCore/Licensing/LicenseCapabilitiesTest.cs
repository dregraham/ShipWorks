﻿using System;
using System.Linq;
using System.Xml;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Editions;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Policies;
using ShipWorks.Stores;
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
            string xmlWithInvalidShipmentTypeRestrictions = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><CustomerStatus><Valid>true</Valid><ShipmentTypeFunctionality><ShipmentType TypeCode='2'><Restriction>Something Wrong</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithInvalidShipmentTypeRestrictions);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.ShipmentTypeRestriction.ContainsKey(ShipmentTypeCode.Endicia));
            Assert.Equal(false, licenseCapabilities.ShipmentTypeRestriction[ShipmentTypeCode.Endicia].Contains(ShipmentTypeRestrictionType.Disabled));
        }

        [Fact]
        public void LicenseCapabilities_DoesNotAddFeature_WhenResponseContainsInvalidFeature()
        {
            string xmlWithInvalidShipmentTypeFeature = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><CustomerStatus><Valid>true</Valid><ShipmentTypeFunctionality><ShipmentType TypeCode='2'><Feature>Something Wrong</Feature></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithInvalidShipmentTypeFeature);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.ShipmentTypeShippingPolicy.ContainsKey(ShipmentTypeCode.Endicia));
            Assert.Equal(0, licenseCapabilities.ShipmentTypeShippingPolicy[ShipmentTypeCode.Endicia].Count);
        }

        [Fact]
        public void LicenseCapabilities_AddsBestRateUpsRestriction_WhenResponseContainsBestRateUpsRestriction()
        {
            string xmlWithBestRateUpsRestrictionShipmentTypeFeature = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>	<UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>yes</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><CustomerStatus><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Feature><Type>BestRateUpsRestriction</Type><Config>True</Config></Feature><Feature><Type>RateResultCount</Type><Config>5</Config></Feature></ShipmentType></ShipmentTypeFunctionality><Valid>true</Valid></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithBestRateUpsRestrictionShipmentTypeFeature);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.ShipmentTypeShippingPolicy.ContainsKey(ShipmentTypeCode.BestRate));
            Assert.Equal("True", licenseCapabilities.ShipmentTypeShippingPolicy[ShipmentTypeCode.BestRate][ShippingPolicyType.BestRateUpsRestriction]);
        }

        [Fact]
        public void LicenseCapabilities_AddsRateResultCountFeature_WhenResponseContainsRateResultCountFeature()
        {
            string xmlWithRateResultCountShipmentTypeFeature = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>	<UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>yes</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><CustomerStatus><Valid>true</Valid><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Feature><Type>BestRateUpsRestriction</Type><Config>True</Config></Feature><Feature><Type>RateResultCount</Type><Config>5</Config></Feature></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithRateResultCountShipmentTypeFeature);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.ShipmentTypeShippingPolicy.ContainsKey(ShipmentTypeCode.BestRate));
            Assert.Equal("5", licenseCapabilities.ShipmentTypeShippingPolicy[ShipmentTypeCode.BestRate][ShippingPolicyType.RateResultCount]);
        }

        [Fact]
        public void LicenseCapabilities_SetsIsInTrial_WhenResponseContainsIsInTrial()
        {
            string xmlWithIsInTrial = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>yes</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><CustomerStatus><Valid>true</Valid></CustomerStatus><IsInTrial>true</IsInTrial></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithIsInTrial);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.IsInTrial);
        }

        [Fact]
        public void LicenseCapabilities_SetsIsInTrialToDefault_WhenResponseDoesNotContainsIsInTrial()
        {
            string xmlWithIsInTrial = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>yes</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><CustomerStatus><Valid>true</Valid></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithIsInTrial);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(false, licenseCapabilities.IsInTrial);
        }

        [Fact]
        public void LicenseCapabilities_GenericFileIsAllowed_WhenResponseContainsCustomDataSources()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>yes</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><CustomerStatus><Valid>true</Valid></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.IsChannelAllowed(StoreTypeCode.GenericFile));
        }

        [Fact]
        public void LicenseCapabilities_GenericFileIsNotAllowed_WhenResponseDoesNotContainsCustomDataSources()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(false, licenseCapabilities.IsChannelAllowed(StoreTypeCode.GenericFile));
        }

        [Fact]
        public void LicenseCapabilities_GenericModuleIsAllowed_WhenResponseDoesContainsCustomDataSourcesApi()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>yes</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.IsChannelAllowed(StoreTypeCode.GenericModule));
        }

        [Fact]
        public void LicenseCapabilities_GenericModuleIsNotAllowed_WhenResponseDoesNotContainsCustomDataSourcesApi()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(false, licenseCapabilities.IsChannelAllowed(StoreTypeCode.GenericModule));
        }

        [Fact]
        public void LicenseCapabilities_OdbcIsAllowed_WhenResponseDoesContainsCustomDataSourcesApi()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>yes</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.IsChannelAllowed(StoreTypeCode.Odbc));
        }

        [Fact]
        public void LicenseCapabilities_OdbcIsNotAllowed_WhenResponseDoesNotContainsCustomDataSourcesApi()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(false, licenseCapabilities.IsChannelAllowed(StoreTypeCode.Odbc));
        }

        [Fact]
        public void LicenseCapabilities_ThrowsShipWorksLicenseException_WhenResponseDoesNotContainsChannelLimit()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            ShipWorksLicenseException ex = Assert.Throws<ShipWorksLicenseException>(() => new LicenseCapabilities(xmlResponse));
            Assert.Equal("The license associated with this account is invalid.", ex.Message);
        }

        [Fact]
        public void LicenseCapabilities_SetsChannelLimit_WhenResponseContainsChannelLimit()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(1, licenseCapabilities.ChannelLimit);
        }

        [Fact]
        public void LicenseCapabilities_SetsShipmentLimit_WhenResponseContainsShipmentLimit()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(600, licenseCapabilities.ShipmentLimit);
        }

        [Fact]
        public void LicenseCapabilities_ThrowsShipWorksLicenseException_WhenResponseDoesNotContainsShipmentLimit()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            ShipWorksLicenseException ex = Assert.Throws<ShipWorksLicenseException>(() => new LicenseCapabilities(xmlResponse));
            Assert.Equal("The license associated with this account is invalid.", ex.Message);
        }

        [Fact]
        public void LicenseCapabilities_SetsActiveChannels_WhenResponseContainsActiveChannels()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>10</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(2, licenseCapabilities.ActiveChannels);
        }

        [Fact]
        public void LicenseCapabilities_ThrowsShipWorksLicenseException_WhenResponseDoesNotContainActiveChannels()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>10</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            ShipWorksLicenseException ex = Assert.Throws<ShipWorksLicenseException>(() => new LicenseCapabilities(xmlResponse));
            Assert.Equal("The license associated with this account is invalid.", ex.Message);
        }

        [Fact]
        public void LicenseCapabilities_SetsProcessedShipments_WhenResponseDoesContainProcessedShipments()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>10</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(10, licenseCapabilities.ProcessedShipments);
        }

        [Fact]
        public void LicenseCapabilities_ThrowsShipWorksLicenseException_WhenResponseDoesNotContainProcessedShipments()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            ShipWorksLicenseException ex = Assert.Throws<ShipWorksLicenseException>(() => new LicenseCapabilities(xmlResponse));
            Assert.Equal("The license associated with this account is invalid.", ex.Message);
        }

        [Fact]
        public void LicenseCapabilities_SetsBillingEndDate_WhenResponseDoesContainBillingEndDate()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>10</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(DateTime.Parse("2016-02-26T20:51:48"), licenseCapabilities.BillingEndDate);
        }

        [Fact]
        public void LicenseCapabilities_SetsStampsInsurance_WhenResponseDoesNotContainStampsInsurance()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(false, licenseCapabilities.StampsInsurance);
        }

        [Fact]
        public void LicenseCapabilities_SetsStampsInsurance_WhenResponseDoesContainStampsInsurance()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.StampsInsurance);
        }

        [Fact]
        public void LicenseCapabilities_SetsStampsDhl_WhenResponseDoesContainStampsDhl()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.StampsDhl);
        }

        [Fact]
        public void LicenseCapabilities_SetsStampsDhl_WhenResponseDoesNotContainStampsDhl()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(false, licenseCapabilities.StampsDhl);
        }

        [Fact]
        public void LicenseCapabilities_SetsStampsAscendiaConsolidator_WhenResponseDoesContainStampsAscendiaConsolidator()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.StampsAscendiaConsolidator);
        }

        [Fact]
        public void LicenseCapabilities_SetsStampsAscendiaConsolidator_WhenResponseDoesNotContainStampsAscendiaConsolidator()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(false, licenseCapabilities.StampsAscendiaConsolidator);
        }

        [Fact]
        public void LicenseCapabilities_SetsStampsDhlConsolidator_WhenResponseDoesContainStampsDhlConsolidator()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.StampsDhlConsolidator);
        }

        [Fact]
        public void LicenseCapabilities_SetsStampsDhlConsolidator_WhenResponseDoesNotContainStampsDhlConsolidator()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(false, licenseCapabilities.StampsDhlConsolidator);
        }

        [Fact]
        public void LicenseCapabilities_SetsStampsGlobegisticsConsolidator_WhenResponseDoesContainStampsGlobegisticsConsolidator()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.StampsGlobegisticsConsolidator);
        }

        [Fact]
        public void LicenseCapabilities_SetsStampsGlobegisticsConsolidator_WhenResponseDoesNotContainStampsGlobegisticsConsolidator()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(false, licenseCapabilities.StampsGlobegisticsConsolidator);
        }

        [Fact]
        public void LicenseCapabilities_SetsStampsIbcConsolidator_WhenResponseDoesContainStampsIbcConsolidator()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.StampsIbcConsolidator);
        }

        [Fact]
        public void LicenseCapabilities_SetsStampsIbcConsolidator_WhenResponseDoesNotContainStampsIbcConsolidator()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(false, licenseCapabilities.StampsIbcConsolidator);
        }

        [Fact]
        public void LicenseCapabilities_SetsStampsRrDonnelleyConsolidator_WhenResponseDoesContainStampsRrDonnelleyConsolidator()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.StampsRrDonnelleyConsolidator);
        }

        [Fact]
        public void LicenseCapabilities_SetsStampsRrDonnelleyConsolidator_WhenResponseDoesNotContainStampsRrDonnelleyConsolidator()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(false, licenseCapabilities.StampsRrDonnelleyConsolidator);
        }

        [Fact]
        public void LicenseCapabilities_SetsEndiciaInsurance_WhenResponseDoesContainEndiciaInsurance()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.EndiciaInsurance);
        }

        [Fact]
        public void LicenseCapabilities_SetsEndiciaInsurance_WhenResponseDoesNotContainEndiciaInsurance()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(false, licenseCapabilities.EndiciaInsurance);
        }

        [Fact]
        public void LicenseCapabilities_SetsEndiciaConsolidator_WhenResponseDoesContainEndiciaConsolidator()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.EndiciaConsolidator);
        }

        [Fact]
        public void LicenseCapabilities_SetsEndiciaConsolidator_WhenResponseDoesNotContainEndiciaConsolidator()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(false, licenseCapabilities.EndiciaConsolidator);
        }

        [Fact]
        public void LicenseCapabilities_SetsEndiciaScanBasedReturns_WhenResponseDoesContainEndiciaScanBasedReturns()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.EndiciaScanBasedReturns);
        }

        [Fact]
        public void LicenseCapabilities_SetsEndiciaScanBasedReturns_WhenResponseDoesNotContainEndiciaScanBasedReturns()
        {
            string xmlWithCustomDataSources = "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithCustomDataSources);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(false, licenseCapabilities.EndiciaScanBasedReturns);
        }

        [Fact]
        public void LicenseCapabilities_SetsShipmentLimit_FromPendingCapabilities_WhenEnterprisePlanIsPending()
        {
            XmlDocument xmlResponse = LoadPlanWithPendingEnterpriseCapabilities();

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(-1, licenseCapabilities.ShipmentLimit);
        }

        [Fact]
        public void LicenseCapabilities_SetsChannelLimit_FromPendingCapabilities_WhenEnterprisePlanIsPending()
        {
            XmlDocument xmlResponse = LoadPlanWithPendingEnterpriseCapabilities();

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(-1, licenseCapabilities.ChannelLimit);
        }

        [Fact]
        public void LicenseCapabilities_GenericFilesAllowed_WhenEnterprisePlanIsPending()
        {
            XmlDocument xmlResponse = LoadPlanWithPendingEnterpriseCapabilities();

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.True(licenseCapabilities.IsChannelAllowed(StoreTypeCode.GenericFile));
        }

        [Fact]
        public void LicenseCapabilities_GenericModuleIsAllowed_WhenEnterprisePlanIsPending()
        {
            XmlDocument xmlResponse = LoadPlanWithPendingEnterpriseCapabilities();

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.True(licenseCapabilities.IsChannelAllowed(StoreTypeCode.GenericModule));
        }

        [Fact]
        public void LicenseCapabilities_OdbcIsAllowed_WhenEnterprisePlanIsPending()
        {
            XmlDocument xmlResponse = LoadPlanWithPendingEnterpriseCapabilities();

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.True(licenseCapabilities.IsChannelAllowed(StoreTypeCode.Odbc));
        }

        [Fact]
        public void LicenseCapabilities_DoesNotAllowBestRate_WhenRateCompareIsNo()
        {
            string xmlWithRateCompareAsNo =
                "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>no</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithRateCompareAsNo);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.False(licenseCapabilities.IsBestRateAllowed);
        }

        [Fact]
        public void LicenseCapabilities_AllowsBestRate_WhenRateCompareIsYes()
        {
            string xmlWithRateCompareAsYes =
                "<?xml version='1.0'?><LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><UserCapabilities><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfShipments</Name><Value>600</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfChannels</Name><Value>1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>NumberOfUsers</Name><Value>-1</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomInvoice </Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomPackingSlips</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomEmail</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSources</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomDataSourcesAPI</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>CustomerManagement</Name><Value>no</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'><Name>RateCompare</Name><Value>yes</Value></NameValuePair></UserCapabilities><UserLevels><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfChannels</Name><Value>2</Value></NameValuePair><NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'><Name>NumberOfShipments</Name><Value>0</Value></NameValuePair></UserLevels><BillingEndDate>2016-02-26T20:51:48</BillingEndDate><IsInTrial>false</IsInTrial><PendingUserCapabilities /><RecurlyCustomerStatus>Current</RecurlyCustomerStatus><CustomerStatus><Key /><Machine /><Active>true</Active><Cancelled>false</Cancelled><DisabledReason /><Valid>true</Valid><CustomerID>31039</CustomerID><EndiciaDhlEnabled status='1' /><EndiciaInsuranceEnabled status='1' /><UpsSurePostEnabled status='1' /><EndiciaConsolidator status='1'>test</EndiciaConsolidator><EndiciaScanBasedReturns status='1' /><StampsAscendiaEnabled status='1'>1</StampsAscendiaEnabled><StampsDhlConsolidatorEnabled status='1'>1</StampsDhlConsolidatorEnabled><StampsGlobegisticsEnabled status='1'>1</StampsGlobegisticsEnabled><StampsIbcEnabled status='1'>1</StampsIbcEnabled><StampsRrDonnelleyEnabled status='1'>1</StampsRrDonnelleyEnabled><StampsInsuranceEnabled status='1'>1</StampsInsuranceEnabled><StampsDhlEnabled status='1'>1</StampsDhlEnabled><ShipmentTypeFunctionality><ShipmentType TypeCode='14'><Restriction>Disabled</Restriction></ShipmentType><ShipmentType TypeCode='2'><Restriction>AccountRegistration</Restriction><Restriction>Purchasing</Restriction><Restriction>Processing</Restriction></ShipmentType></ShipmentTypeFunctionality></CustomerStatus></LoginActivityResponse>";

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(xmlWithRateCompareAsYes);

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.True(licenseCapabilities.IsBestRateAllowed);
        }

        #region Capabilities are pulled from current plan when downgrading

        [Fact]
        public void LicenseCapabilities_SetsShipmentLimit_FromCurrentCapabilities_WhenEnterprisePlanIsCurrent()
        {
            XmlDocument xmlResponse = LoadPlanDowngradingFromEnterpriseCapabilities();

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(-1, licenseCapabilities.ShipmentLimit);
        }

        [Fact]
        public void LicenseCapabilities_SetsChannelLimit_FromCurrentCapabilities_WhenEnterprisePlanIsCurrent()
        {
            XmlDocument xmlResponse = LoadPlanDowngradingFromEnterpriseCapabilities();

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(-1, licenseCapabilities.ChannelLimit);
        }

        [Fact]
        public void LicenseCapabilities_GenericFilesAllowed_WhenEnterprisePlanIsCurrent()
        {
            XmlDocument xmlResponse = LoadPlanDowngradingFromEnterpriseCapabilities();

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.True(licenseCapabilities.IsChannelAllowed(StoreTypeCode.GenericFile));
        }

        [Fact]
        public void LicenseCapabilities_GenericModuleIsAllowed_WhenEnterprisePlanIsCurrent()
        {
            XmlDocument xmlResponse = LoadPlanDowngradingFromEnterpriseCapabilities();

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.True(licenseCapabilities.IsChannelAllowed(StoreTypeCode.GenericModule));
        }

        [Fact]
        public void LicenseCapabilities_OdbcIsAllowed_WhenEnterprisePlanIsCurrent()
        {
            XmlDocument xmlResponse = LoadPlanDowngradingFromEnterpriseCapabilities();

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.True(licenseCapabilities.IsChannelAllowed(StoreTypeCode.Odbc));
        }

        [Fact]
        public void LicenseCapabilities_AllowsBestRate_WhenEnterprisePlanIsCurrent()
        {
            XmlDocument xmlResponse = LoadPlanDowngradingFromEnterpriseCapabilities();

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.True(licenseCapabilities.IsBestRateAllowed);
        }

        #endregion Capabilities are pulled from current plan when downgrading


        #region Capabilities are pulled from pending plan when upgrading
        [Fact]
        public void LicenseCapabilities_SetsShipmentLimit_FromPendingCapabilities_WhenUpgradingPlans()
        {
            XmlDocument xmlResponse = LoadResponseWithUpgradedPendingCapabilities();

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(1200, licenseCapabilities.ShipmentLimit);
        }

        [Fact]
        public void LicenseCapabilities_SetsChannelLimit_FromPendingCapabilities_WhenUpgradingPlans()
        {
            XmlDocument xmlResponse = LoadResponseWithUpgradedPendingCapabilities();

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(3, licenseCapabilities.ChannelLimit);
        }

        [Fact]
        public void LicenseCapabilities_AllowsGenericFile_FromPendingCapabilities_WhenUpgradingPlans()
        {
            XmlDocument xmlResponse = LoadResponseWithUpgradedPendingCapabilities();

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.IsChannelAllowed(StoreTypeCode.GenericFile));
        }

        [Fact]
        public void LicenseCapabilities_AllowsGenericModule_FromPendingCapabilities_WhenUpgradingPlans()
        {
            XmlDocument xmlResponse = LoadResponseWithUpgradedPendingCapabilities();

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.IsChannelAllowed(StoreTypeCode.GenericModule));
        }

        [Fact]
        public void LicenseCapabilities_AllowsOdbc_FromPendingCapabilities_WhenUpgradingPlans()
        {
            XmlDocument xmlResponse = LoadResponseWithUpgradedPendingCapabilities();

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.Equal(true, licenseCapabilities.IsChannelAllowed(StoreTypeCode.Odbc));
        }

        [Fact]
        public void LicenseCapabilities_AllowsBestRate_FromPendingCapabilities_WhenUpgradingPlans_AndRateCompareIsYes()
        {
            XmlDocument xmlResponse = LoadResponseWithUpgradedPendingCapabilities();

            LicenseCapabilities licenseCapabilities = new LicenseCapabilities(xmlResponse);

            Assert.True(licenseCapabilities.IsBestRateAllowed);
        }


        #endregion Capabilities are pulled from pending plan when upgrading

        private XmlDocument LoadPlanDowngradingFromEnterpriseCapabilities()
        {
            string xmlWithPendingEnterpisePlan = @"<?xml version='1.0'?>
                        <LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                          <UserCapabilities>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>NumberOfShipments</Name>
                              <Value>-1</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>NumberOfChannels</Name>
                              <Value>-1</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>NumberOfUsers</Name>
                              <Value>-1</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomInvoice </Name>
                              <Value>yes</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomPackingSlips</Name>
                              <Value>yes</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomEmail</Name>
                              <Value>yes</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomDataSources</Name>
                              <Value>yes</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomDataSourcesAPI</Name>
                              <Value>yes</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomerManagement</Name>
                              <Value>yes</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>RateCompare</Name>
                              <Value>yes</Value>
                            </NameValuePair>
                          </UserCapabilities>
                          <UserLevels>
                            <NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'>
                              <Name>NumberOfChannels</Name>
                              <Value>1</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'>
                              <Name>NumberOfShipments</Name>
                              <Value>0</Value>
                            </NameValuePair>
                          </UserLevels>
                          <BillingEndDate>2016-03-19T20:32:33</BillingEndDate>
                          <IsInTrial>false</IsInTrial>
                          <PendingUserCapabilities>
                                  <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                                    <Name>NumberOfShipments</Name>
                                    <Value>100</Value>
                                </NameValuePair>
                                <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                                    <Name>NumberOfChannels</Name>
                                    <Value>1</Value>
                                </NameValuePair>
                                <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                                    <Name>NumberOfUsers</Name>
                                    <Value>-1</Value>
                                </NameValuePair>
                                <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                                    <Name>CustomInvoice </Name>
                                    <Value>no</Value>
                                </NameValuePair>
                                <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                                    <Name>CustomPackingSlips</Name>
                                    <Value>no</Value>
                                </NameValuePair>
                                <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                                    <Name>CustomEmail</Name>
                                    <Value>no</Value>
                                </NameValuePair>
                                <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                                    <Name>CustomDataSources</Name>
                                    <Value>no</Value>
                                </NameValuePair>
                                <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                                    <Name>CustomDataSourcesAPI</Name>
                                    <Value>no</Value>
                                </NameValuePair>
                                <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                                    <Name>CustomerManagement</Name>
                                    <Value>no</Value>
                                </NameValuePair>
                                <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                                    <Name>RateCompare</Name>
                                    <Value>no</Value>
                                </NameValuePair>
                          </PendingUserCapabilities>
                          <RecurlyCustomerStatus>Current</RecurlyCustomerStatus>
                          <CustomerStatus>
                            <Key />
                            <Machine />
                            <Active>true</Active>
                            <Cancelled>false</Cancelled>
                            <DisabledReason />
                            <Valid>true</Valid>
                            <CustomerID>32211</CustomerID>
                            <ShipmentTypeFunctionality>
                              <ShipmentType TypeCode='14'>
                                <Restriction>Disabled</Restriction>
                              </ShipmentType>
                              <ShipmentType TypeCode='2'>
                                <Restriction>AccountRegistration</Restriction>
                              </ShipmentType>
                              <ShipmentType TypeCode='16'>
                                <Restriction>Disabled</Restriction>
                              </ShipmentType>
                            </ShipmentTypeFunctionality>
                          </CustomerStatus>
                        </LoginActivityResponse>";

            XmlDocument document = new XmlDocument();
            document.LoadXml(xmlWithPendingEnterpisePlan);

            return document;
        }

        private XmlDocument LoadPlanWithPendingEnterpriseCapabilities()
        {
            string xmlWithPendingEnterpisePlan = @"<?xml version='1.0'?>
                        <LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                          <UserCapabilities>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>NumberOfShipments</Name>
                              <Value>100</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>NumberOfChannels</Name>
                              <Value>1</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>NumberOfUsers</Name>
                              <Value>-1</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomInvoice </Name>
                              <Value>no</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomPackingSlips</Name>
                              <Value>no</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomEmail</Name>
                              <Value>no</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomDataSources</Name>
                              <Value>no</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomDataSourcesAPI</Name>
                              <Value>no</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomerManagement</Name>
                              <Value>no</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>RateCompare</Name>
                              <Value>no</Value>
                            </NameValuePair>
                          </UserCapabilities>
                          <UserLevels>
                            <NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'>
                              <Name>NumberOfChannels</Name>
                              <Value>1</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'>
                              <Name>NumberOfShipments</Name>
                              <Value>0</Value>
                            </NameValuePair>
                          </UserLevels>
                          <BillingEndDate>2016-03-19T20:32:33</BillingEndDate>
                          <IsInTrial>false</IsInTrial>
                          <PendingUserCapabilities>
                              <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>NumberOfShipments</Name>
                              <Value>-1</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>NumberOfChannels</Name>
                              <Value>-1</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>NumberOfUsers</Name>
                              <Value>-1</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomInvoice </Name>
                              <Value>yes</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomPackingSlips</Name>
                              <Value>yes</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomEmail</Name>
                              <Value>yes</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomDataSources</Name>
                              <Value>yes</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomDataSourcesAPI</Name>
                              <Value>yes</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomerManagement</Name>
                              <Value>yes</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>RateCompare</Name>
                              <Value>yes</Value>
                            </NameValuePair>
                          </PendingUserCapabilities>
                          <RecurlyCustomerStatus>Current</RecurlyCustomerStatus>
                          <CustomerStatus>
                            <Key />
                            <Machine />
                            <Active>true</Active>
                            <Cancelled>false</Cancelled>
                            <DisabledReason />
                            <Valid>true</Valid>
                            <CustomerID>32211</CustomerID>
                            <ShipmentTypeFunctionality>
                              <ShipmentType TypeCode='14'>
                                <Restriction>Disabled</Restriction>
                              </ShipmentType>
                              <ShipmentType TypeCode='2'>
                                <Restriction>AccountRegistration</Restriction>
                              </ShipmentType>
                              <ShipmentType TypeCode='16'>
                                <Restriction>Disabled</Restriction>
                              </ShipmentType>
                            </ShipmentTypeFunctionality>
                          </CustomerStatus>
                        </LoginActivityResponse>";

            XmlDocument document = new XmlDocument();
            document.LoadXml(xmlWithPendingEnterpisePlan);

            return document;
        }

        private XmlDocument LoadResponseWithUpgradedPendingCapabilities()
        {
            string xmlWithPendingEnterpisePlan = @"<?xml version='1.0'?>
                        <LoginActivityResponse xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                          <UserCapabilities>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>NumberOfShipments</Name>
                              <Value>100</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>NumberOfChannels</Name>
                              <Value>1</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>NumberOfUsers</Name>
                              <Value>-1</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomInvoice </Name>
                              <Value>no</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomPackingSlips</Name>
                              <Value>no</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomEmail</Name>
                              <Value>no</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomDataSources</Name>
                              <Value>no</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomDataSourcesAPI</Name>
                              <Value>no</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomerManagement</Name>
                              <Value>no</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>RateCompare</Name>
                              <Value>no</Value>
                            </NameValuePair>
                          </UserCapabilities>
                          <UserLevels>
                            <NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'>
                              <Name>NumberOfChannels</Name>
                              <Value>1</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserLevelsV1.xsd'>
                              <Name>NumberOfShipments</Name>
                              <Value>0</Value>
                            </NameValuePair>
                          </UserLevels>
                          <BillingEndDate>2016-03-19T20:32:33</BillingEndDate>
                          <IsInTrial>false</IsInTrial>
                          <PendingUserCapabilities>
                              <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>NumberOfShipments</Name>
                              <Value>1200</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>NumberOfChannels</Name>
                              <Value>3</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>NumberOfUsers</Name>
                              <Value>-1</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomInvoice </Name>
                              <Value>yes</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomPackingSlips</Name>
                              <Value>yes</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomEmail</Name>
                              <Value>yes</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomDataSources</Name>
                              <Value>yes</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomDataSourcesAPI</Name>
                              <Value>yes</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>CustomerManagement</Name>
                              <Value>yes</Value>
                            </NameValuePair>
                            <NameValuePair xmlns='http://ShipWorks.com/UserCapabilitiesV1.xsd'>
                              <Name>RateCompare</Name>
                              <Value>yes</Value>
                            </NameValuePair>
                          </PendingUserCapabilities>
                          <RecurlyCustomerStatus>Current</RecurlyCustomerStatus>
                          <CustomerStatus>
                            <Key />
                            <Machine />
                            <Active>true</Active>
                            <Cancelled>false</Cancelled>
                            <DisabledReason />
                            <Valid>true</Valid>
                            <CustomerID>32211</CustomerID>
                            <ShipmentTypeFunctionality>
                              <ShipmentType TypeCode='14'>
                                <Restriction>Disabled</Restriction>
                              </ShipmentType>
                              <ShipmentType TypeCode='2'>
                                <Restriction>AccountRegistration</Restriction>
                              </ShipmentType>
                              <ShipmentType TypeCode='16'>
                                <Restriction>Disabled</Restriction>
                              </ShipmentType>
                            </ShipmentTypeFunctionality>
                          </CustomerStatus>
                        </LoginActivityResponse>";

            XmlDocument document = new XmlDocument();
            document.LoadXml(xmlWithPendingEnterpisePlan);

            return document;
        }
    }
}