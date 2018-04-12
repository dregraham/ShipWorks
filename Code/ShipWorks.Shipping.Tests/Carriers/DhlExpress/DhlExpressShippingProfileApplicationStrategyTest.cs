using System;
using System.Linq;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Shipping.Insurance;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Carriers.DhlExpress
{
    public class DhlExpressShippingProfileApplicationStrategyTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ShippingProfileEntity profile;
        private readonly ShipmentEntity shipment;
        private readonly PackageProfileEntity packageProfile;
        

        public DhlExpressShippingProfileApplicationStrategyTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            packageProfile = new PackageProfileEntity();
            
            profile = new ShippingProfileEntity
            {
                Packages = { packageProfile },
                DhlExpress = new DhlExpressProfileEntity()
            };
            
            shipment = new ShipmentEntity { DhlExpress = new DhlExpressShipmentEntity() };
        }

        [Fact]
        public void ApplyProfile_WeightApplied()
        {
            packageProfile.Weight = 7;
            Apply();
            Assert.Equal(7, shipment.DhlExpress.Packages.Single().Weight);
        }
        
        [Fact]
        public void ApplyProfile_AppliesProfilesPackageDims()
        {
            packageProfile.DimsProfileID = 0;
            packageProfile.DimsLength = 1;
            packageProfile.DimsWidth = 2;
            packageProfile.DimsHeight = 3;
            packageProfile.DimsWeight = 4;
            packageProfile.DimsAddWeight = true;
            
            Apply();
            var package = shipment.DhlExpress.Packages.Single();
            
            Assert.Equal(0, package.DimsProfileID);
            Assert.Equal(1, package.DimsLength);
            Assert.Equal(2, package.DimsWidth);
            Assert.Equal(3, package.DimsHeight);
            Assert.Equal(4, package.DimsWeight);
            Assert.True(package.DimsAddWeight);
        }

        [Fact]
        public void ApplyProfile_AppliesAccountID()
        {
            profile.DhlExpress.DhlExpressAccountID = 44;
            Apply();
            Assert.Equal(44, shipment.DhlExpress.DhlExpressAccountID);
        }

        [Fact]
        public void ApplyProfile_AppliesAccountIDFromRepository_WhenProfileAccountIDIsZero()
        {
            profile.DhlExpress.DhlExpressAccountID = 0;
            mock.Mock<ICarrierAccountRepository<DhlExpressAccountEntity, IDhlExpressAccountEntity>>()
                .SetupGet(r => r.Accounts).Returns(new[]
                {
                    new DhlExpressAccountEntity
                    {
                        DhlExpressAccountID = 77
                    }
                });
            
            Apply();
            Assert.Equal(77, shipment.DhlExpress.DhlExpressAccountID);
        }

        [Fact]
        public void ApplyProfile_AppliesDhlShipmentValues()
        {
            profile.DhlExpress.Service = 2;
            profile.DhlExpress.DeliveryDutyPaid = true;
            profile.DhlExpress.NonMachinable = true;
            profile.DhlExpress.SaturdayDelivery = true;
            profile.DhlExpress.NonDelivery = 3;
            profile.DhlExpress.Contents = 4;

            Apply();
            
            Assert.Equal(2, shipment.DhlExpress.Service);
            Assert.True(shipment.DhlExpress.DeliveredDutyPaid);
            Assert.True(shipment.DhlExpress.NonMachinable);
            Assert.True(shipment.DhlExpress.SaturdayDelivery);
            Assert.Equal(3, shipment.DhlExpress.NonDelivery);
            Assert.Equal(4, shipment.DhlExpress.Contents);
        }

        [Fact]
        public void ApplyProfile_PackageRemovedFromShipment_WhenSecondPackage_AndProfileHasSinglePackage()
        {
            shipment.DhlExpress.Packages.Add(new DhlExpressPackageEntity());
            shipment.DhlExpress.Packages.Add(new DhlExpressPackageEntity());

            Assert.Equal(2, shipment.DhlExpress.Packages.Count);
            Apply();
            Assert.Equal(1, shipment.DhlExpress.Packages.Count);
        }

        [Theory]
        [InlineData(true, 0)]
        [InlineData(false, 1)]
        public void ApplyProfile_DeletesProfileFromDataBase_OnlyIfNew(bool isNew, int timesDeleteIsCalled)
        {
            var secondPackage = new DhlExpressPackageEntity { IsNew = isNew };
            
            shipment.DhlExpress.Packages.Add(new DhlExpressPackageEntity());
            shipment.DhlExpress.Packages.Add(secondPackage);
            
            var sqlAdapterFactory = mock.FromFactory<ISqlAdapterFactory>()
                .Mock(x => x.Create());
            
            Apply();
            
            sqlAdapterFactory.Verify(a=>a.DeleteEntity(secondPackage), Times.Exactly(timesDeleteIsCalled));
        }

        [Fact]
        public void ApplyProfile_UpdateTotalWeightIsCalled_WhenPackageWeightChanges()
        {
            shipment.DhlExpress.Packages.Add(new DhlExpressPackageEntity(){Weight = 3});
            packageProfile.Weight = 2;
            
            Mock<ShipmentType> shipmentTypeMock = mock.CreateMock<ShipmentType>();
            mock.Mock<IShipmentTypeManager>().Setup(m => m.Get(shipment)).Returns(shipmentTypeMock.Object);
            
            Apply();
            
            shipmentTypeMock.Verify(s=>s.UpdateTotalWeight(shipment), Times.Once);
        }
        
        [Fact]
        public void ApplyProfile_UpdateTotalWeightDataIsNotCalled_WhenPackageWeightDoesNotChange()
        {
            shipment.DhlExpress.Packages.Add(new DhlExpressPackageEntity(){Weight = 2});
            packageProfile.Weight = 2;
            
            Mock<ShipmentType> shipmentTypeMock = mock.CreateMock<ShipmentType>();
            mock.Mock<IShipmentTypeManager>().Setup(m => m.Get(shipment)).Returns(shipmentTypeMock.Object);
            
            Apply();
            
            shipmentTypeMock.Verify(s=>s.UpdateTotalWeight(shipment), Times.Never);
        }
        
        [Fact]
        public void ApplyProfile_UpdateDynamicShipmentDataIsNotCalled_WhenPackageWeightDoesNotChange()
        {
            Mock<ShipmentType> shipmentTypeMock = mock.CreateMock<ShipmentType>();
            mock.Mock<IShipmentTypeManager>().Setup(m => m.Get(shipment)).Returns(shipmentTypeMock.Object);
            
            Apply();
            
            shipmentTypeMock.Verify(s=>s.UpdateDynamicShipmentData(shipment), Times.Once);
        }
        
         [Fact]
        public void ApplyProfile_DelegatesToShipmentTypeManagerWithShipment()
        {
            Apply();
            mock.Mock<IShipmentTypeManager>().Verify(s => s.Get(shipment));
        }

        [Fact]
        public void ApplyProfile_SetsOriginIDOnShipment()
        {
            profile.OriginID = 123;
            Apply();
            Assert.Equal(123, shipment.OriginOriginID);
        }

        [Fact]
        public void ApplyProfile_SetsReturnShipmentOnShipment()
        {
            profile.ReturnShipment = true;
            Apply();
            Assert.True(shipment.ReturnShipment);
        }

        [Fact]
        public void ApplyProfile_SetsRequestedLabelFormatOnShipment()
        {
            profile.RequestedLabelFormat = (int)ThermalLanguage.EPL;
            Apply();
            Assert.Equal(ThermalLanguage.EPL, (ThermalLanguage) shipment.OriginOriginID);
        }

        [Fact]
        public void ApplyProfile_DelegatesToShipmentTypeToSaveLabelFormat()
        {
            var shipmentType = mock.Mock<ShipmentType>();
            mock.Mock<IShipmentTypeManager>().Setup(s => s.Get(shipment)).Returns(shipmentType);
            
            profile.RequestedLabelFormat = (int) ThermalLanguage.EPL ;

            Apply();

            shipmentType.Verify(s => s.SaveRequestedLabelFormat((ThermalLanguage) shipment.RequestedLabelFormat, shipment));        }

        [Fact]
        public void ApplyProfile_SetsInsuranceValueOnPackage()
        {
            var insuranceChoice = mock.Mock<IInsuranceChoice>();
            var shipmentParcel = new ShipmentParcel(shipment, null, insuranceChoice.Object, new Editing.DimensionsAdapter());

            var shipmentType = mock.Mock<ShipmentType>();
            shipmentType.Setup(s => s.GetParcelCount(shipment)).Returns(1);
            shipmentType.Setup(s => s.GetParcelDetail(shipment, 0)).Returns(shipmentParcel);

            mock.Mock<IShipmentTypeManager>().Setup(s => s.Get(shipment)).Returns(shipmentType);

            profile.Insurance = true;

            Apply();

            insuranceChoice.VerifySet(i => i.Insured = true);
        }

        private void Apply()
        {
            var testObject = mock.Create<DhlExpressShippingProfileApplicationStrategy>();
            testObject.ApplyProfile(profile, shipment);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}