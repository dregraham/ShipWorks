using Interapptive.Shared.Enums;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api.ElementWriters;
using System.Collections;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;


namespace ShipWorks.Tests.Shipping.Carriers.UPS.OnLineTools.Api.ElementWriters
{
    public class UpsRatePackageServiceOptionsElementWriterTests
    {
        UpsShipmentEntity shipment;
        UpsPackageEntity package;
        UpsServicePackageTypeSetting packageSetting;

        public UpsRatePackageServiceOptionsElementWriterTests()
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
                var testObject = new UpsRatePackageServiceOptionsElementWriter(writer);
                testObject.WriteServiceOptionsElement(shipment, package, packageSetting);
            }

            return element;
        }

        [Fact]
        public void WriteServiceOptionsElement_DoesNotWriteDryIce_Test()
        {
            package.DryIceEnabled = true;
            package.DryIceRegulationSet = (int)UpsDryIceRegulationSet.Cfr;
            package.DryIceIsForMedicalUse = true;

            XElement element = WriteServiceOptionsElement();

            var dryIces =
                ((IEnumerable)element.XPathEvaluate("/PackageServiceOptions/DryIce"))
                    .Cast<XElement>().ToList();

            Assert.Equal(0, dryIces.Count);
        }
    }
}
