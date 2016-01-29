using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Common.Logging.Configuration;
using Xunit;
using Microsoft.XmlDiffPatch;
using ShipWorks.Editions;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Policies;

namespace ShipWorks.Tests.Editions
{
    public class ShipmentTypeFunctionalityTest
    {
        private XmlDocument xml;
        private XPathNavigator path;

        public ShipmentTypeFunctionalityTest()
        {
            // Because we're now caching policy data, we need to make sure we clear the cache before each test run
            ShippingPolicies.ClearCache();

            SetupXmlWithMultipleShipmentTypesWithSingleRestrictionEach();
        }

        public void SetupXmlWithMultipleShipmentTypesWithSingleRestrictionEach()
        {
            xml = new XmlDocument();
            xml.LoadXml(@"<?xml version=""1.0"" standalone=""yes""?>
<License>
	<Key>FBIN4-6CR3T-6EXUL-MI4XF-CART66LITE-BRIAN@INTERAPPTIVE.COM</Key>
	<Machine>10.1.10.131/cart66lite/wp-</Machine>
	<Active>true</Active>
	<Cancelled>false</Cancelled>
	<DisabledReason/>
	<Valid>true</Valid>
	<StoreID>29043</StoreID>
	<CustomerID>53</CustomerID>
	<Version>Checked</Version>
	<EndiciaDhlEnabled status=""1""/>
	<EndiciaInsuranceEnabled status=""1""/>
	<UpsSurePostEnabled status=""1""/>
	<EndiciaConsolidator status=""1"">Something or other</EndiciaConsolidator>
	<EndiciaScanBasedReturns status=""1""/>
    <ShipmentTypeFunctionality>
		<ShipmentType TypeCode=""15"">
			<Restriction>Disabled</Restriction>
            <Feature>
                <Type>BestRateUpsRestriction</Type>
                <Config>True</Config>
            </Feature>
            <Feature>
                <Type>RateResultCount</Type>
                <Config>2</Config>
            </Feature>
		</ShipmentType>
		<ShipmentType TypeCode=""6"">
			<Restriction>AccountRegistration</Restriction>
		</ShipmentType>
		<ShipmentType TypeCode='2'>
			<Restriction>RateDiscountMessaging</Restriction>
		</ShipmentType>
	</ShipmentTypeFunctionality>
</License>");

            path = xml.CreateNavigator();
        }

        public void SetupXmlWithMultipleFedExRestrictions()
        {
            xml = new XmlDocument();
            xml.LoadXml(@"<?xml version=""1.0"" standalone=""yes""?>
<License>
	<Key>FBIN4-6CR3T-6EXUL-MI4XF-CART66LITE-BRIAN@INTERAPPTIVE.COM</Key>
	<Machine>10.1.10.131/cart66lite/wp-</Machine>
	<Active>true</Active>
	<Cancelled>false</Cancelled>
	<DisabledReason/>
	<Valid>true</Valid>
	<StoreID>29043</StoreID>
	<CustomerID>53</CustomerID>
	<Version>Checked</Version>
	<EndiciaDhlEnabled status=""1""/>
	<EndiciaInsuranceEnabled status=""1""/>
	<UpsSurePostEnabled status=""1""/>
	<EndiciaConsolidator status=""1"">Something or other</EndiciaConsolidator>
	<EndiciaScanBasedReturns status=""1""/>
    <ShipmentTypeFunctionality>
		<ShipmentType TypeCode=""15"">
			<Restriction>Disabled</Restriction>
		</ShipmentType>
		<ShipmentType TypeCode=""6"">
			<Restriction>AccountRegistration</Restriction>
            <Restriction>Disabled</Restriction>
		</ShipmentType>
	</ShipmentTypeFunctionality>
</License>");

            path = xml.CreateNavigator();
        }

        public void SetupXmlWithNoFedExRestrictions()
        {
            xml = new XmlDocument();
            xml.LoadXml(@"<?xml version=""1.0"" standalone=""yes""?>
<License>
	<Key>FBIN4-6CR3T-6EXUL-MI4XF-CART66LITE-BRIAN@INTERAPPTIVE.COM</Key>
	<Machine>10.1.10.131/cart66lite/wp-</Machine>
	<Active>true</Active>
	<Cancelled>false</Cancelled>
	<DisabledReason/>
	<Valid>true</Valid>
	<StoreID>29043</StoreID>
	<CustomerID>53</CustomerID>
	<Version>Checked</Version>
	<EndiciaDhlEnabled status=""1""/>
	<EndiciaInsuranceEnabled status=""1""/>
	<UpsSurePostEnabled status=""1""/>
	<EndiciaConsolidator status=""1"">Something or other</EndiciaConsolidator>
	<EndiciaScanBasedReturns status=""1""/>
    <ShipmentTypeFunctionality>
		<ShipmentType TypeCode=""15"">
			<Restriction>Disabled</Restriction>
		</ShipmentType>
		<ShipmentType TypeCode=""6"">
		</ShipmentType>
	</ShipmentTypeFunctionality>
</License>");

            path = xml.CreateNavigator();
        }

        public void SetupXmlWithNoShipmentTypes()
        {
            xml = new XmlDocument();
            xml.LoadXml(@"<?xml version=""1.0"" standalone=""yes""?>
<License>
	<Key>FBIN4-6CR3T-6EXUL-MI4XF-CART66LITE-BRIAN@INTERAPPTIVE.COM</Key>
	<Machine>10.1.10.131/cart66lite/wp-</Machine>
	<Active>true</Active>
	<Cancelled>false</Cancelled>
	<DisabledReason/>
	<Valid>true</Valid>
	<StoreID>29043</StoreID>
	<CustomerID>53</CustomerID>
	<Version>Checked</Version>
	<EndiciaDhlEnabled status=""1""/>
	<EndiciaInsuranceEnabled status=""1""/>
	<UpsSurePostEnabled status=""1""/>
	<EndiciaConsolidator status=""1"">Something or other</EndiciaConsolidator>
	<EndiciaScanBasedReturns status=""1""/>
    <ShipmentTypeFunctionality />
</License>");

            path = xml.CreateNavigator();
        }

        public void SetupXmlWithDuplicateFedExRestrictions()
        {
            xml = new XmlDocument();
            xml.LoadXml(@"<?xml version=""1.0"" standalone=""yes""?>
<License>
	<Key>FBIN4-6CR3T-6EXUL-MI4XF-CART66LITE-BRIAN@INTERAPPTIVE.COM</Key>
	<Machine>10.1.10.131/cart66lite/wp-</Machine>
	<Active>true</Active>
	<Cancelled>false</Cancelled>
	<DisabledReason/>
	<Valid>true</Valid>
	<StoreID>29043</StoreID>
	<CustomerID>53</CustomerID>
	<Version>Checked</Version>
	<EndiciaDhlEnabled status=""1""/>
	<EndiciaInsuranceEnabled status=""1""/>
	<UpsSurePostEnabled status=""1""/>
	<EndiciaConsolidator status=""1"">Something or other</EndiciaConsolidator>
	<EndiciaScanBasedReturns status=""1""/>
    <ShipmentTypeFunctionality>
		<ShipmentType TypeCode=""15"">
			<Restriction>Disabled</Restriction>
		</ShipmentType>
		<ShipmentType TypeCode=""6"">
            <Restriction>Disabled</Restriction>
            <Restriction>Disabled</Restriction>
		</ShipmentType>
	</ShipmentTypeFunctionality>
</License>");

            path = xml.CreateNavigator();
        }

        public void SetupXmlWithDuplicateShipmentTypeNodes()
        {
            xml = new XmlDocument();
            xml.LoadXml(@"<?xml version=""1.0"" standalone=""yes""?>
<License>
	<Key>FBIN4-6CR3T-6EXUL-MI4XF-CART66LITE-BRIAN@INTERAPPTIVE.COM</Key>
	<Machine>10.1.10.131/cart66lite/wp-</Machine>
	<Active>true</Active>
	<Cancelled>false</Cancelled>
	<DisabledReason/>
	<Valid>true</Valid>
	<StoreID>29043</StoreID>
	<CustomerID>53</CustomerID>
	<Version>Checked</Version>
	<EndiciaDhlEnabled status=""1""/>
	<EndiciaInsuranceEnabled status=""1""/>
	<UpsSurePostEnabled status=""1""/>
	<EndiciaConsolidator status=""1"">Something or other</EndiciaConsolidator>
	<EndiciaScanBasedReturns status=""1""/>
    <ShipmentTypeFunctionality>
		<ShipmentType TypeCode=""6"">
            <Restriction>AccountRegistration</Restriction>
            <Restriction>Disabled</Restriction>
		</ShipmentType>
		<ShipmentType TypeCode=""6"">
            <Restriction>Disabled</Restriction>
            <Restriction>AccountRegistration</Restriction>
		</ShipmentType>
	</ShipmentTypeFunctionality>
</License>");

            path = xml.CreateNavigator();
        }

        public void SetupXmlWithShipmentTypeListedMultipleTimesWithDifferentRestrictions()
        {
            xml = new XmlDocument();
            xml.LoadXml(@"<?xml version=""1.0"" standalone=""yes""?>
<License>
	<Key>FBIN4-6CR3T-6EXUL-MI4XF-CART66LITE-BRIAN@INTERAPPTIVE.COM</Key>
	<Machine>10.1.10.131/cart66lite/wp-</Machine>
	<Active>true</Active>
	<Cancelled>false</Cancelled>
	<DisabledReason/>
	<Valid>true</Valid>
	<StoreID>29043</StoreID>
	<CustomerID>53</CustomerID>
	<Version>Checked</Version>
	<EndiciaDhlEnabled status=""1""/>
	<EndiciaInsuranceEnabled status=""1""/>
	<UpsSurePostEnabled status=""1""/>
	<EndiciaConsolidator status=""1"">Something or other</EndiciaConsolidator>
	<EndiciaScanBasedReturns status=""1""/>
    <ShipmentTypeFunctionality>
		<ShipmentType TypeCode=""6"">
            <Restriction>AccountRegistration</Restriction>
		</ShipmentType>
		<ShipmentType TypeCode=""6"">
            <Restriction>Disabled</Restriction>
            <Restriction>AccountRegistration</Restriction>
		</ShipmentType>
	</ShipmentTypeFunctionality>
</License>");

            path = xml.CreateNavigator();
        }

        public void SetupXmlWithoutShipmentTypeFunctionalityNode()
        {
            xml = new XmlDocument();
            xml.LoadXml(@"<?xml version=""1.0"" standalone=""yes""?>
<License>
	<Key>FBIN4-6CR3T-6EXUL-MI4XF-CART66LITE-BRIAN@INTERAPPTIVE.COM</Key>
	<Machine>10.1.10.131/cart66lite/wp-</Machine>
	<Active>true</Active>
	<Cancelled>false</Cancelled>
	<DisabledReason/>
	<Valid>true</Valid>
	<StoreID>29043</StoreID>
	<CustomerID>53</CustomerID>
	<Version>Checked</Version>
	<EndiciaDhlEnabled status=""1""/>
	<EndiciaInsuranceEnabled status=""1""/>
	<UpsSurePostEnabled status=""1""/>
	<EndiciaConsolidator status=""1"">Something or other</EndiciaConsolidator>
	<EndiciaScanBasedReturns status=""1""/>
    <ThisShouldNotBeFound>
		<ShipmentType TypeCode=""15"">
			<Restriction>Disabled</Restriction>
		</ShipmentType>
		<ShipmentType TypeCode=""6"">
			<Restriction>AccountRegistration</Restriction>
		</ShipmentType>
	</ThisShouldNotBeFound>
</License>");

            path = xml.CreateNavigator();
        }

        public void SetupXmlWithFedExProcessingRestriction()
        {
            xml = new XmlDocument();
            xml.LoadXml(@"<?xml version=""1.0"" standalone=""yes""?>
<License>
	<Key>FBIN4-6CR3T-6EXUL-MI4XF-CART66LITE-BRIAN@INTERAPPTIVE.COM</Key>
	<Machine>10.1.10.131/cart66lite/wp-</Machine>
	<Active>true</Active>
	<Cancelled>false</Cancelled>
	<DisabledReason/>
	<Valid>true</Valid>
	<StoreID>29043</StoreID>
	<CustomerID>53</CustomerID>
	<Version>Checked</Version>
	<EndiciaDhlEnabled status=""1""/>
	<EndiciaInsuranceEnabled status=""1""/>
	<UpsSurePostEnabled status=""1""/>
	<EndiciaConsolidator status=""1"">Something or other</EndiciaConsolidator>
	<EndiciaScanBasedReturns status=""1""/>
    <ShipmentTypeFunctionality>
		<ShipmentType TypeCode=""15"">
			<Restriction>AccountRegistration</Restriction>
		</ShipmentType>
		<ShipmentType TypeCode=""6"">
			<Restriction>Processing</Restriction>
		</ShipmentType>
	</ShipmentTypeFunctionality>
</License>");

            path = xml.CreateNavigator();
        }

        public void SetupXmlWithFedExPurchasingRestriction()
        {
            xml = new XmlDocument();
            xml.LoadXml(@"<?xml version=""1.0"" standalone=""yes""?>
<License>
	<Key>FBIN4-6CR3T-6EXUL-MI4XF-CART66LITE-BRIAN@INTERAPPTIVE.COM</Key>
	<Machine>10.1.10.131/cart66lite/wp-</Machine>
	<Active>true</Active>
	<Cancelled>false</Cancelled>
	<DisabledReason/>
	<Valid>true</Valid>
	<StoreID>29043</StoreID>
	<CustomerID>53</CustomerID>
	<Version>Checked</Version>
	<EndiciaDhlEnabled status=""1""/>
	<EndiciaInsuranceEnabled status=""1""/>
	<UpsSurePostEnabled status=""1""/>
	<EndiciaConsolidator status=""1"">Something or other</EndiciaConsolidator>
	<EndiciaScanBasedReturns status=""1""/>
    <ShipmentTypeFunctionality>
		<ShipmentType TypeCode=""15"">
			<Restriction>AccountRegistration</Restriction>
		</ShipmentType>
		<ShipmentType TypeCode=""6"">
			<Restriction>Purchasing</Restriction>
		</ShipmentType>
	</ShipmentTypeFunctionality>
</License>");

            path = xml.CreateNavigator();
        }

        public void SetupXmlWithFedExConversionRestriction()
        {
            xml = new XmlDocument();
            xml.LoadXml(@"<?xml version=""1.0"" standalone=""yes""?>
<License>
	<Key>FBIN4-6CR3T-6EXUL-MI4XF-CART66LITE-BRIAN@INTERAPPTIVE.COM</Key>
	<Machine>10.1.10.131/cart66lite/wp-</Machine>
	<Active>true</Active>
	<Cancelled>false</Cancelled>
	<DisabledReason/>
	<Valid>true</Valid>
	<StoreID>29043</StoreID>
	<CustomerID>53</CustomerID>
	<Version>Checked</Version>
	<EndiciaDhlEnabled status=""1""/>
	<EndiciaInsuranceEnabled status=""1""/>
	<UpsSurePostEnabled status=""1""/>
	<EndiciaConsolidator status=""1"">Something or other</EndiciaConsolidator>
	<EndiciaScanBasedReturns status=""1""/>
    <ShipmentTypeFunctionality>
		<ShipmentType TypeCode=""15"">
			<Restriction>AccountRegistration</Restriction>
		</ShipmentType>
		<ShipmentType TypeCode=""6"">
			<Restriction>ShippingAccountConversion</Restriction>
		</ShipmentType>
	</ShipmentTypeFunctionality>
</License>");

            path = xml.CreateNavigator();
        }

        [Fact]
        public void Deserialize_AddsKeysForAllShipmentTypeCodes()
        {
            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Deserialize(1, path);

            Assert.True(functionality[ShipmentTypeCode.FedEx].Any());
            Assert.True(functionality[ShipmentTypeCode.Usps].Any());
        }

        [Fact]
        public void Deserialize_AddsDisabledRestriction_WhenListedInTheRestrictionSet()
        {
            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Deserialize(1, path);

            IEnumerable<ShipmentTypeRestrictionType> restrictions = functionality[ShipmentTypeCode.Usps];

            Assert.True(restrictions.Contains(ShipmentTypeRestrictionType.Disabled));
        }

        [Fact]
        public void Deserialize_AddsAccountRegistrationRestriction_WhenListedInTheRestrictionSet()
        {
            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Deserialize(1, path);

            IEnumerable<ShipmentTypeRestrictionType> restrictions = functionality[ShipmentTypeCode.FedEx];

            Assert.True(restrictions.Contains(ShipmentTypeRestrictionType.AccountRegistration));
        }

        [Fact]
        public void Deserialize_AddsRateDiscountMessagingRestriction_WhenListedInTheRestrictionSet()
        {
            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Deserialize(1, path);

            IEnumerable<ShipmentTypeRestrictionType> restrictions = functionality[ShipmentTypeCode.Endicia];

            Assert.True(restrictions.Contains(ShipmentTypeRestrictionType.RateDiscountMessaging));
        }

        [Fact]
        public void Deserialize_AddsMultipleRestrictionForShipmentType_WhenMultipleRestrictionsListedInTheRestrictionSet()
        {
            SetupXmlWithMultipleFedExRestrictions();

            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Deserialize(1, path);
            IEnumerable<ShipmentTypeRestrictionType> restrictions = functionality[ShipmentTypeCode.FedEx];

            Assert.Equal(2, restrictions.Count());
        }

        [Fact]
        public void Deserialize_HasNoRestrictions_WhenNoShipmentTypesAreInXml()
        {
            SetupXmlWithNoShipmentTypes();
            
            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Deserialize(1, path);

            foreach (ShipmentTypeCode typeCode in Enum.GetValues(typeof (ShipmentTypeCode)))
            {
                IEnumerable<ShipmentTypeRestrictionType> restrictions = functionality[typeCode];
                Assert.Equal(0, restrictions.Count());
            }
        }

        [Fact]
        public void Deserialize_OnlyAddsDistinctRestrictions_WhenDuplicateRestrictionsListedInTheRestrictionSet()
        {
            SetupXmlWithDuplicateFedExRestrictions();

            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Deserialize(1, path);
            IEnumerable<ShipmentTypeRestrictionType> restrictions = functionality[ShipmentTypeCode.FedEx];

            Assert.Equal(1, restrictions.Count());
        }

        [Fact]
        public void Deserialize_HasNoRestrictions_WhenRestrictionSetIsEmpty()
        {
            SetupXmlWithNoFedExRestrictions();

            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Deserialize(1, path);
            IEnumerable<ShipmentTypeRestrictionType> restrictions = functionality[ShipmentTypeCode.FedEx];

            Assert.Equal(0, restrictions.Count());
        }

        [Fact]
        public void Deserialize_HasNoRestrictions_WhenShipmentTypeFunctionalityIsMissing()
        {
            SetupXmlWithoutShipmentTypeFunctionalityNode();

            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Deserialize(1, path);
            IEnumerable<ShipmentTypeRestrictionType> restrictions = functionality[ShipmentTypeCode.FedEx];

            Assert.Equal(0, restrictions.Count());
        }

        [Fact]
        public void Deserialize_OnlyAddsDistinctRestrictions_WhenDuplicateShipmentTypeNodesInXml()
        {
            // Each node configured with the same two restrictions
            SetupXmlWithDuplicateShipmentTypeNodes();

            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Deserialize(1, path);
            IEnumerable<ShipmentTypeRestrictionType> restrictions = functionality[ShipmentTypeCode.FedEx];
            
            Assert.Equal(2, restrictions.Distinct().Count());
        }

        [Fact]
        public void Deserialize_OnlyAddsDistinctRestrictions_WhenShipmentTypeListedMultipleTimes_WithDifferentRestrictionsInEachNode()
        {
            // Each node configured with the same two restrictions
            SetupXmlWithDuplicateShipmentTypeNodes();

            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Deserialize(1, path);
            IEnumerable<ShipmentTypeRestrictionType> restrictions = functionality[ShipmentTypeCode.FedEx];

            Assert.Equal(2, restrictions.Distinct().Count());
        }

        [Fact]
        public void Deserialize_AddsProcessingRestriction()
        {
            SetupXmlWithFedExProcessingRestriction();

            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Deserialize(1, path);
            IEnumerable<ShipmentTypeRestrictionType> restrictions = functionality[ShipmentTypeCode.FedEx];

            Assert.Equal(1, restrictions.Count(r => r == ShipmentTypeRestrictionType.Processing));
        }

        [Fact]
        public void Deserialize_AddsPurchasingRestriction()
        {
            SetupXmlWithFedExPurchasingRestriction();

            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Deserialize(1, path);
            IEnumerable<ShipmentTypeRestrictionType> restrictions = functionality[ShipmentTypeCode.FedEx];

            Assert.Equal(1, restrictions.Count(r => r == ShipmentTypeRestrictionType.Purchasing));
        }

        [Fact]
        public void Deserialize_AddsShippingAccountConversionRestriction()
        {
            SetupXmlWithFedExConversionRestriction();

            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Deserialize(1, path);
            IEnumerable<ShipmentTypeRestrictionType> restrictions = functionality[ShipmentTypeCode.FedEx];

            Assert.Equal(1, restrictions.Count(r => r == ShipmentTypeRestrictionType.ShippingAccountConversion));
        }

        [Fact]
        public void ToString_ReturnsOriginalShipmentTypeFunctionalityXml()
        {
            SetupXmlWithMultipleShipmentTypesWithSingleRestrictionEach();
            string expectedRawXml = @"<ShipmentTypeFunctionality>
		<ShipmentType TypeCode=""15"">
			<Restriction>Disabled</Restriction>
             <Feature>
                <Type>BestRateUpsRestriction</Type>
                <Config>True</Config>
            </Feature>
            <Feature>
                <Type>RateResultCount</Type>
                <Config>2</Config>
            </Feature>
		</ShipmentType>
		<ShipmentType TypeCode=""6"">
			<Restriction>AccountRegistration</Restriction>
		</ShipmentType>
		<ShipmentType TypeCode='2'>
			<Restriction>RateDiscountMessaging</Restriction>
		</ShipmentType>
	</ShipmentTypeFunctionality>";

            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Deserialize(1, path);

            Assert.True(CompareXmlToText(functionality.ToXElement(), expectedRawXml));
        }

        [Fact]
        public void ToXElement_ReturnsOriginalShipmentTypeFunctionalityElement()
        {
            SetupXmlWithMultipleShipmentTypesWithSingleRestrictionEach();
            string expectedRawXml = @"<ShipmentTypeFunctionality>
		<ShipmentType TypeCode=""15"">
			<Restriction>Disabled</Restriction>
            <Feature>
                <Type>BestRateUpsRestriction</Type>
                <Config>True</Config>
            </Feature>
            <Feature>
                <Type>RateResultCount</Type>
                <Config>2</Config>
            </Feature>
		</ShipmentType>
		<ShipmentType TypeCode=""6"">
			<Restriction>AccountRegistration</Restriction>
		</ShipmentType>
		<ShipmentType TypeCode='2'>
			<Restriction>RateDiscountMessaging</Restriction>
		</ShipmentType>
	</ShipmentTypeFunctionality>";

            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Deserialize(1, path);

            Assert.True(CompareXmlToText(functionality.ToXElement(), expectedRawXml));
        }

        [Fact]
        public void ToXElement_ReturnsShipmentTypeFunctionalityElement_WhenSourceisMissingShipmentTypeFunctionalityNode()
        {
            SetupXmlWithoutShipmentTypeFunctionalityNode();
            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Deserialize(1, path);

            XElement xElement = functionality.ToXElement();

            Assert.Equal("ShipmentTypeFunctionality", xElement.Name);
        }

        [Fact]
        public void ToXElement_ReturnsEmptyElement_WhenSourceIsMissingShipmentTypeFunctionalityNode()
        {
            SetupXmlWithoutShipmentTypeFunctionalityNode();
            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Deserialize(1, path);

            XElement xElement = functionality.ToXElement();

            Assert.True(xElement.IsEmpty);
        }

        [Fact]
        public void Deserialize_XElementsAreStored()
        {
            List<KeyValuePair<ShipmentTypeCode, IEnumerable<XElement>>> storedPolicies = null;
            long storedStoreId = 0;
            
            ShipmentTypeFunctionality.Deserialize(31, XElement.Parse(path.OuterXml), (storeId, policies) =>
            {
                storedStoreId = storeId;
                storedPolicies = policies;
            } );

            Assert.Equal(31, storedStoreId);
            
            Assert.Equal(1,storedPolicies.Count);
            Assert.Equal(ShipmentTypeCode.Usps, storedPolicies.First().Key);
            Assert.Equal(2,storedPolicies.First().Value.Count());
            Assert.True(
                CompareXmlToText(
                    storedPolicies.First().Value.First(),
                    "<Feature><Type>BestRateUpsRestriction</Type><Config>True</Config></Feature>"),
                "Xml Didn't Match");
            Assert.True(
                CompareXmlToText(
                    storedPolicies.First().Value.Last(),
                    " <Feature><Type>RateResultCount</Type><Config>2</Config></Feature>"),
                "Xml Didn't Match");
        }

        private bool CompareXmlToText(XElement xElement, string xmlText)
        {
               XmlDocument expectedDocument = new XmlDocument();
            expectedDocument.LoadXml(xmlText);

            XmlDocument actualDocument = new XmlDocument();
            actualDocument.LoadXml(xElement.ToString());

            XmlDiff diff = new XmlDiff(XmlDiffOptions.IgnoreWhitespace);
            return diff.Compare(expectedDocument, actualDocument);
        }
    }
}
