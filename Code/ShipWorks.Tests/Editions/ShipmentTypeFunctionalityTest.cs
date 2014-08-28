using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Editions;
using ShipWorks.Shipping;

namespace ShipWorks.Tests.Editions
{
    [TestClass]
    public class ShipmentTypeFunctionalityTest
    {
        private XmlDocument xml;
        private XPathNavigator path;

        [TestInitialize]
        public void Initialize()
        {
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
		<ShipmentType TypeCode=""3"">
			<Restriction>Disabled</Restriction>
		</ShipmentType>
		<ShipmentType TypeCode=""6"">
			<Restriction>AccountRegistration</Restriction>
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
		<ShipmentType TypeCode=""3"">
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
		<ShipmentType TypeCode=""3"">
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
		<ShipmentType TypeCode=""3"">
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

        [TestMethod]
        public void Parse_AddsKeysForAllShipmentTypeCodes_Test()
        {
            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Parse(path);

            Assert.IsTrue(functionality[ShipmentTypeCode.FedEx].Any());
            Assert.IsTrue(functionality[ShipmentTypeCode.Stamps].Any());
        }

        [TestMethod]
        public void Parse_AddsDisabledRestriction_WhenListedInTheRestrictionSet_Test()
        {
            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Parse(path);

            IEnumerable<ShipmentTypeRestrictionType> restrictions = functionality[ShipmentTypeCode.Stamps];

            Assert.IsTrue(restrictions.Contains(ShipmentTypeRestrictionType.Disabled));
        }

        [TestMethod]
        public void Parse_AddsAccountRegistrationRestriction_WhenListedInTheRestrictionSet_Test()
        {
            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Parse(path);

            IEnumerable<ShipmentTypeRestrictionType> restrictions = functionality[ShipmentTypeCode.FedEx];

            Assert.IsTrue(restrictions.Contains(ShipmentTypeRestrictionType.AccountRegistration));
        }

        [TestMethod]
        public void Parse_AddsMultipleRestrictionForShipmentType_WhenMultipleRestrictionsListedInTheRestrictionSet_Test()
        {
            SetupXmlWithMultipleFedExRestrictions();

            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Parse(path);
            IEnumerable<ShipmentTypeRestrictionType> restrictions = functionality[ShipmentTypeCode.FedEx];

            Assert.AreEqual(2, restrictions.Count());
        }

        [TestMethod]
        public void Parse_HasNoRestrictions_WhenNoShipmentTypesAreInXml_Test()
        {
            SetupXmlWithNoShipmentTypes();
            
            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Parse(path);

            foreach (ShipmentTypeCode typeCode in Enum.GetValues(typeof (ShipmentTypeCode)))
            {
                IEnumerable<ShipmentTypeRestrictionType> restrictions = functionality[typeCode];
                Assert.AreEqual(0, restrictions.Count());
            }
        }

        [TestMethod]
        public void Parse_OnlyAddsDistinctRestrictions_WhenDuplicateRestrictionsListedInTheRestrictionSet_Test()
        {
            SetupXmlWithDuplicateFedExRestrictions();

            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Parse(path);
            IEnumerable<ShipmentTypeRestrictionType> restrictions = functionality[ShipmentTypeCode.FedEx];

            Assert.AreEqual(1, restrictions.Count());
        }

        [TestMethod]
        public void Parse_HasNoRestrictions_WhenRestrictionSetIsEmpty_Test()
        {
            SetupXmlWithNoFedExRestrictions();

            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Parse(path);
            IEnumerable<ShipmentTypeRestrictionType> restrictions = functionality[ShipmentTypeCode.FedEx];

            Assert.AreEqual(0, restrictions.Count());
        }

        [TestMethod]
        public void Parse_OnlyAddsDistinctRestrictions_WhenDuplicateShipmentTypeNodesInXml_Test()
        {
            // Each node configured with the same two restrictions
            SetupXmlWithDuplicateShipmentTypeNodes();

            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Parse(path);
            IEnumerable<ShipmentTypeRestrictionType> restrictions = functionality[ShipmentTypeCode.FedEx];
            
            Assert.AreEqual(2, restrictions.Distinct().Count());
        }

        [TestMethod]
        public void Parse_OnlyAddsDistinctRestrictions_WhenShipmentTypeListedMultipleTimes_WithDifferentRestrictionsInEachNode_Test()
        {
            // Each node configured with the same two restrictions
            SetupXmlWithDuplicateShipmentTypeNodes();

            ShipmentTypeFunctionality functionality = ShipmentTypeFunctionality.Parse(path);
            IEnumerable<ShipmentTypeRestrictionType> restrictions = functionality[ShipmentTypeCode.FedEx];

            Assert.AreEqual(2, restrictions.Distinct().Count());
        }
    }
}
