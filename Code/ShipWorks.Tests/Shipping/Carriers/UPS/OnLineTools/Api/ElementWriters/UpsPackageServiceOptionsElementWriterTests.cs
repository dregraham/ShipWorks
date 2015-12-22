using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api.ElementWriters;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;


namespace ShipWorks.Tests.Shipping.Carriers.UPS.OnLineTools.Api.ElementWriters
{
    public class UpsPackageServiceOptionsElementWriterTests
    {
        UpsShipmentEntity shipment;
        UpsPackageEntity package;
        UpsServicePackageTypeSetting packageSetting;

        public UpsPackageServiceOptionsElementWriterTests()
        {
            shipment = new UpsShipmentEntity { Shipment = new ShipmentEntity() };
            package = shipment.Packages.AddNew();
            packageSetting = new UpsServicePackageTypeSetting(UpsServiceType.UpsStandard, UpsPackagingType.Box10Kg, WeightUnitOfMeasure.Kilograms, 0, 1000, 10, false, false, false);
        }

        protected XElement WriteServiceOptionsElement()
        {
            var element = new XElement("TestContainer");

            using (XmlWriter writer = element.CreateWriter())
            {
                var testObject = new UpsPackageServiceOptionsElementWriter(writer);
                testObject.WriteServiceOptionsElement(shipment, package, packageSetting);
            }

            return element;
        }

        [Fact]
        public void WriteServiceOptionsElement_WritesVerbalConfirmationName_Test()
        {
            package.VerbalConfirmationEnabled = true;
            package.VerbalConfirmationName = "Angus Bludgeonfordshire";

            XElement element = WriteServiceOptionsElement();

            var names =
                ((IEnumerable)element.XPathEvaluate("/PackageServiceOptions/VerbalConfirmation/ContactInfo/Name/text()"))
                    .Cast<XText>().Select(x => x.Value).ToList();
            
            Assert.Equal(new[] { package.VerbalConfirmationName }, names);
        }

        [Fact]
        public void WriteServiceOptionsElement_DoesNotWriteVerbalConfirmationName_WhenVerbalConfirmationEnabledIsFalse_Test()
        {
            package.VerbalConfirmationEnabled = false;
            package.VerbalConfirmationName = "Angus Bludgeonfordshire";

            XElement element = WriteServiceOptionsElement();

            var names =
                ((IEnumerable)element.XPathEvaluate("/PackageServiceOptions/VerbalConfirmation/ContactInfo/Name/text()"))
                    .Cast<XText>().Select(x => x.Value).ToList();

            Assert.DoesNotContain(package.VerbalConfirmationName, names);
        }

        [Fact]
        public void WriteServiceOptionsElement_WritesVerbalConfirmationPhoneNumber_Test()
        {
            package.VerbalConfirmationEnabled = true;
            package.VerbalConfirmationPhone = "(555) 867-5309";

            XElement element = WriteServiceOptionsElement();

            var phoneNumbers =
                ((IEnumerable)element.XPathEvaluate("/PackageServiceOptions/VerbalConfirmation/ContactInfo/Phone/Number/text()"))
                    .Cast<XText>().Select(x => x.Value).ToList();

            Assert.Equal(new[] { package.VerbalConfirmationPhone }, phoneNumbers);
        }

        [Fact]
        public void WriteServiceOptionsElement_DoesNotWriteVerbalConfirmationPhoneNumber_WhenVerbalConfirmationEnabledIsFalse_Test()
        {
            package.VerbalConfirmationEnabled = false;
            package.VerbalConfirmationPhone = "(555) 867-5309";

            XElement element = WriteServiceOptionsElement();

            var phoneNumbers =
                ((IEnumerable)element.XPathEvaluate("/PackageServiceOptions/VerbalConfirmation/ContactInfo/Phone/Number/text()"))
                    .Cast<XText>().Select(x => x.Value).ToList();

            Debug.Assert(phoneNumbers.Count == 0, "The phone number was written, even though VerbalConfirmationEnabled is false.");
        }

        [Fact]
        public void WriteServiceOptionsElement_WritesVerbalConfirmationPhoneExtension_Test()
        {
            package.VerbalConfirmationEnabled = true;
            package.VerbalConfirmationPhoneExtension = "1228";

            XElement element = WriteServiceOptionsElement();

            var phoneExtensions =
                ((IEnumerable)element.XPathEvaluate("/PackageServiceOptions/VerbalConfirmation/ContactInfo/Phone/Extension/text()"))
                    .Cast<XText>().Select(x => x.Value).ToList();

            Assert.Equal(new[] { package.VerbalConfirmationPhoneExtension }, phoneExtensions);
        }

        [Fact]
        public void WriteServiceOptionsElement_DoesNotWriteVerbalConfirmationPhoneExtension_WhenVerbalConfirmationEnabledIsFalse_Test()
        {
            package.VerbalConfirmationEnabled = false;
            package.VerbalConfirmationPhoneExtension = "1228";

            XElement element = WriteServiceOptionsElement();

            var phoneExtensions =
                ((IEnumerable)element.XPathEvaluate("/PackageServiceOptions/VerbalConfirmation/ContactInfo/Phone/Extension/text()"))
                    .Cast<XText>().Select(x => x.Value).ToList();

            Debug.Assert(phoneExtensions.Count == 0, "The phone extension was written, even though VerbalConfirmationEnabled is false.");
        }

        [Fact]
        public void WriteServiceOptionsElement_DoesNotWriteVerbalConfirmation_WhenShipmentIsReturn_Test()
        {
            package.VerbalConfirmationEnabled = true;
            shipment.Shipment.ReturnShipment = true;
            package.VerbalConfirmationName = "Samuel David James Rutherford III, Esq.";

            XElement element = WriteServiceOptionsElement();

            var confirmations =
                ((IEnumerable)element.XPathEvaluate("/PackageServiceOptions/VerbalConfirmation"))
                    .Cast<XElement>().ToList();

            Assert.Equal(0, confirmations.Count);
        }

        [Fact]
        public void WriteServiceOptionsElement_DoesNotWriteVerbalConfirmation_WhenVerbalConfirmationEnabledIsFalse_Test()
        {
            package.VerbalConfirmationEnabled = false;
            package.VerbalConfirmationName = "Samuel David James Rutherford III, Esq.";

            XElement element = WriteServiceOptionsElement();

            var confirmations =
                ((IEnumerable)element.XPathEvaluate("/PackageServiceOptions/VerbalConfirmation"))
                    .Cast<XElement>().ToList();

            Assert.Equal(0, confirmations.Count);
        }

        [Fact]
        public void WriteServiceOptionsElement_WritesDryIceRegulationSet_Test()
        {
            package.DryIceEnabled = true;
            package.DryIceRegulationSet = (int)UpsDryIceRegulationSet.Iata;

            XElement element = WriteServiceOptionsElement();

            var regulationSets =
                ((IEnumerable)element.XPathEvaluate("/PackageServiceOptions/DryIce/RegulationSet/text()"))
                    .Cast<XText>().Select(x => x.Value).ToList();

            Assert.Equal(new[] { EnumHelper.GetApiValue((UpsDryIceRegulationSet)package.DryIceRegulationSet) }, regulationSets);
        }

        [Fact]
        public void WriteServiceOptionsElement_WritesDryIceUnitOfMeasurementCode_Test()
        {
            package.DryIceEnabled = true;

            XElement element = WriteServiceOptionsElement();

            var units =
                ((IEnumerable)element.XPathEvaluate("/PackageServiceOptions/DryIce/DryIceWeight/UnitOfMeasurement/Code/text()"))
                    .Cast<XText>().Select(x => x.Value).ToList();

            Assert.Equal(new[] { "LBS" }, units);
        }

        [Fact]
        public void WriteServiceOptionsElement_WritesDryIceWeight_Test()
        {
            package.DryIceEnabled = true;
            package.DryIceWeight = 4.3; //kg, per settings

            XElement element = WriteServiceOptionsElement();

            var weights =
                ((IEnumerable)element.XPathEvaluate("/PackageServiceOptions/DryIce/DryIceWeight/Weight/text()"))
                    .Cast<XText>().Select(x => x.Value).ToList();

            var weightInLbs = WeightUtility.Convert(packageSetting.WeightUnitOfMeasure, WeightUnitOfMeasure.Pounds, package.DryIceWeight);

            Assert.Equal(new[] { weightInLbs.ToString("##0.0") }, weights);
        }

        [Fact]
        public void WriteServiceOptionsElement_WritesDryIceMedicalUseIndicator_Test()
        {
            package.DryIceEnabled = true;
            package.DryIceRegulationSet = (int)UpsDryIceRegulationSet.Cfr;
            package.DryIceIsForMedicalUse = true;

            XElement element = WriteServiceOptionsElement();

            var medicalUses =
                ((IEnumerable)element.XPathEvaluate("/PackageServiceOptions/DryIce/MedicalUseIndicator"))
                    .Cast<XElement>().ToList();

            Assert.Equal(1, medicalUses.Count);
        }

        [Fact]
        public void WriteServiceOptionsElement_DoesNotWriteDryIceMedicalUseIndicator_WhenNotForMedicalUse_Test()
        {
            package.DryIceEnabled = true;
            package.DryIceRegulationSet = (int)UpsDryIceRegulationSet.Cfr;
            package.DryIceIsForMedicalUse = false;

            XElement element = WriteServiceOptionsElement();

            var medicalUses =
                ((IEnumerable)element.XPathEvaluate("/PackageServiceOptions/DryIce/MedicalUseIndicator"))
                    .Cast<XElement>().ToList();

            Assert.Equal(0, medicalUses.Count);
        }

        [Fact]
        public void WriteServiceOptionsElement_DoesNotWriteDryIceMedicalUseIndicator_WhenRegulationSetIsNotCfr_Test()
        {
            package.DryIceEnabled = true;
            package.DryIceRegulationSet = (int)UpsDryIceRegulationSet.Iata;
            package.DryIceIsForMedicalUse = true;

            XElement element = WriteServiceOptionsElement();

            var medicalUses =
                ((IEnumerable)element.XPathEvaluate("/PackageServiceOptions/DryIce/MedicalUseIndicator"))
                    .Cast<XElement>().ToList();

            Assert.Equal(0, medicalUses.Count);
        }

        [Fact]
        public void WriteServiceOptionsElement_DoesNotWriteDryIce_WhenDryIceIsNotEnabled_Test()
        {
            package.DryIceEnabled = false;

            XElement element = WriteServiceOptionsElement();

            var dryIces =
                ((IEnumerable)element.XPathEvaluate("/PackageServiceOptions/DryIce"))
                    .Cast<XElement>().ToList();

            Assert.Equal(0, dryIces.Count);
        }
    }
}
