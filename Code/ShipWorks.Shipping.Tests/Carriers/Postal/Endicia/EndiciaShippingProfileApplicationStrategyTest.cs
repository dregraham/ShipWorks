using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Postal.Endicia
{
    public class EndiciaShippingProfileApplicationStrategyTest
    {
        private readonly AutoMock mock;

        public EndiciaShippingProfileApplicationStrategyTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void ApplyProfile_SetsEndiciaAccountID()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.Postal.Endicia.EndiciaAccountID = 123;

            var testObject = mock.Create<EndiciaShippingProfileApplicationStrategy>();
            
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(123, shipment.Postal.Endicia.EndiciaAccountID);
        }

        [Fact]
        public void ApplyProfile_SetsEndiciaStealthPostage()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.Postal.Endicia.StealthPostage = true;

            var testObject = mock.Create<EndiciaShippingProfileApplicationStrategy>();
            
            testObject.ApplyProfile(profile, shipment);

            Assert.True(shipment.Postal.Endicia.StealthPostage);
        }

        [Fact]
        public void ApplyProfile_SetsEndiciaReferenceID()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.Postal.Endicia.ReferenceID = "123";

            var testObject = mock.Create<EndiciaShippingProfileApplicationStrategy>();
            
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal("123", shipment.Postal.Endicia.ReferenceID);
        }

        [Fact]
        public void ApplyProfile_SetsEndiciaScanBasedReturn()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.Postal.Endicia.ScanBasedReturn = true;

            var testObject = mock.Create<EndiciaShippingProfileApplicationStrategy>();
            
            testObject.ApplyProfile(profile, shipment);

            Assert.True(shipment.Postal.Endicia.ScanBasedReturn);
        }

        [Fact]
        public void ApplyProfile_SetsEndiciaInsurance()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.Insurance = true;

            var testObject = mock.Create<EndiciaShippingProfileApplicationStrategy>();
            
            testObject.ApplyProfile(profile, shipment);

            Assert.True(shipment.Postal.Endicia.Insurance);
        }

        [Fact]
        public void ApplyProfile_DelegatesToShipmentTypeManagerWithShipment()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();

            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            var testObject = mock.Create<EndiciaShippingProfileApplicationStrategy>();
            
            testObject.ApplyProfile(profile, shipment);

            shipmentTypeManager.Verify(s => s.Get(shipment));
        }

        [Fact]
        public void ApplyProfile_SetsOriginIDOnShipment()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.OriginID = 123;

            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            var testObject = mock.Create<EndiciaShippingProfileApplicationStrategy>();

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(123, shipment.OriginOriginID);
        }

        [Fact]
        public void ApplyProfile_SetsReturnShipmentOnShipment()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.ReturnShipment = true;
            
            var testObject = mock.Create<EndiciaShippingProfileApplicationStrategy>();

            testObject.ApplyProfile(profile, shipment);

            Assert.True(shipment.ReturnShipment);
        }

        [Fact]
        public void ApplyProfile_SetsRequestedLabelFormatOnShipment()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.RequestedLabelFormat = (int) ThermalLanguage.EPL;
            
            var testObject = mock.Create<EndiciaShippingProfileApplicationStrategy>();

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(ThermalLanguage.EPL, (ThermalLanguage) shipment.OriginOriginID);
        }

        [Fact]
        public void ApplyProfile_DelegatesToShipmentTypeToSaveLabelFormat()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();

            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            var shipmentType = mock.Mock<ShipmentType>();
            shipmentTypeManager.Setup(s => s.Get(shipment)).Returns(shipmentType);

            var testObject = mock.Create<EndiciaShippingProfileApplicationStrategy>();
            
            testObject.ApplyProfile(profile, shipment);

            shipmentType.Verify(s => s.SaveRequestedLabelFormat((ThermalLanguage) shipment.RequestedLabelFormat, shipment));
        }

        [Fact]
        public void ApplyProfile_SetsInsuranceValueOnPackage()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.Insurance = true;

            var insuranceChoice = mock.Mock<IInsuranceChoice>();
            
            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            var shipmentType = mock.Mock<ShipmentType>();
            shipmentType.Setup(s => s.GetParcelCount(shipment)).Returns(1);
            var shipmentParcel = new ShipmentParcel(shipment, null, insuranceChoice.Object, new Editing.DimensionsAdapter());

            shipmentType.Setup(s => s.GetParcelDetail(shipment, 0)).Returns(shipmentParcel);

            shipmentTypeManager.Setup(s => s.Get(shipment)).Returns(shipmentType);

            var testObject = mock.Create<EndiciaShippingProfileApplicationStrategy>();

            testObject.ApplyProfile(profile, shipment);

            insuranceChoice.VerifySet(i => i.Insured = true);
        }

        private (ShippingProfileEntity, ShipmentEntity) GetEmptyShipmentAndProfile()
        {
            var profile = new ShippingProfileEntity()
            {
                Postal = new PostalProfileEntity()
                {
                    Endicia = new EndiciaProfileEntity()
                }
            };

            profile.Packages.Add(new PackageProfileEntity());

            return (profile, new ShipmentEntity()
            {
                Postal = new PostalShipmentEntity()
                {
                    Endicia = new EndiciaShipmentEntity()
                }
            });
        }
    }
}
