using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.iParcel
{
    public class iParcelShippingProfileApplicationStrategyTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<ICarrierAccountRetriever<IParcelAccountEntity, IIParcelAccountEntity>> accountRetriever;
        private readonly Mock<ShipmentType> shipmentType;
        private readonly ShipmentEntity shipment;
        private readonly ShippingProfileEntity profile;
        private readonly iParcelShippingProfileApplicationStrategy testObject;

        public iParcelShippingProfileApplicationStrategyTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shipmentType = mock.Mock<ShipmentType>();
            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            shipmentTypeManager.Setup(s => s.Get(ShipmentTypeCode.iParcel)).Returns(shipmentType);
            accountRetriever = mock.Mock<ICarrierAccountRetriever<IParcelAccountEntity, IIParcelAccountEntity>>();
            
            shipment = new ShipmentEntity
            {
                IParcel = new IParcelShipmentEntity
                {
                    Packages =
                    {
                        new IParcelPackageEntity
                        {
                            DimsProfileID = 9,
                            DimsLength = 9,
                            DimsWidth = 9,
                            DimsHeight = 9,
                            DimsWeight = 9,
                            DimsAddWeight = false,
                            SkuAndQuantities = "packageSkuAndQuantities"
                        }
                    },
                    IParcelAccountID = 9,
                    Service = (int) iParcelServiceType.SaverDeferred,
                    Reference = "shipmentReference",
                    TrackByEmail = false,
                    TrackBySMS = false,
                    IsDeliveryDutyPaid = false
                },
                ContentWeight = 9,
                Insurance = false
            };
            profile = new ShippingProfileEntity
            {
                Packages =
                {
                    new PackageProfileEntity
                    {
                        DimsProfileID = 0,
                        DimsLength = 1,
                        DimsWidth = 2,
                        DimsHeight = 3,
                        DimsWeight = 4,
                        DimsAddWeight = true,
                        Weight = 5
                    }
                },
                IParcel = new IParcelProfileEntity
                {
                    SkuAndQuantities = "profileSkuAndQuantities",
                    IParcelAccountID = 6,
                    Service = (int) iParcelServiceType.Immediate,
                    Reference = "profileReference",
                    TrackByEmail = true,
                    TrackBySMS = true,
                    IsDeliveryDutyPaid = true
                },
                Insurance = true
            };

            testObject = mock.Create<iParcelShippingProfileApplicationStrategy>();
        }

        [Fact]
        public void ApplyProfile_AddPackagesToShipment_WhenShipmentHasFewerPackagesThanProfile()
        {
            profile.Packages.Add(new PackageProfileEntity());
            
            testObject.ApplyProfile(profile, shipment);
            
            Assert.Equal(2, shipment.IParcel.Packages.Count);
        }

        [Fact]
        public void ApplyProfile_AppliesPackageProfilesWeight_WhenSinglePackageProfile()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(5, shipment.IParcel.Packages[0].Weight);
        }
        
        [Fact]
        public void ApplyProfile_AppliesPackageProfilesWeight_WhenMultiplePackageProfiles()
        {
            profile.Packages.Add(new PackageProfileEntity {Weight = 10});
            
            testObject.ApplyProfile(profile, shipment);
            
            Assert.Equal(10, shipment.IParcel.Packages[1].Weight);
        }
        
        [Fact]
        public void ApplyProfile_AppliesDimsProfileID()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(0, shipment.IParcel.Packages[0].DimsProfileID);
        }
        
        [Fact]
        public void ApplyProfile_AppliesDimsLength_WhenDimsProfileIDIsNotNull()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(1, shipment.IParcel.Packages[0].DimsLength);
        }
        
        [Fact]
        public void ApplyProfile_AppliesDimsWidth_WhenDimsProfileIDIsNotNull()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(2, shipment.IParcel.Packages[0].DimsWidth);
        }
        
        [Fact]
        public void ApplyProfile_AppliesDimsHeight_WhenDimsProfileIDIsNotNull()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(3, shipment.IParcel.Packages[0].DimsHeight);
        }
        
        [Fact]
        public void ApplyProfile_AppliesDimsWeight_WhenDimsProfileIDIsNotNull()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(4, shipment.IParcel.Packages[0].DimsWeight);
        }
        
        [Fact]
        public void ApplyProfile_AppliesDimsAddWeight_WhenDimsProfileIDIsNotNull()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.True(shipment.IParcel.Packages[0].DimsAddWeight);
        }
        
        [Fact]
        public void ApplyProfile_DoesNotApplyDims_WhenDimsProfileIDIsNull()
        {
            profile.Packages[0].DimsProfileID = null;
            
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(9, shipment.IParcel.Packages[0].DimsLength);
            Assert.Equal(9, shipment.IParcel.Packages[0].DimsWidth);
            Assert.Equal(9, shipment.IParcel.Packages[0].DimsHeight);
            Assert.Equal(9, shipment.IParcel.Packages[0].DimsWeight);
            Assert.False(shipment.IParcel.Packages[0].DimsAddWeight);
        }

        [Fact]
        public void ApplyProfile_RemovesPackagesFromShipment_WhenProfileHasFewerPackagesThanShipment()
        {
            shipment.IParcel.Packages.Add(new IParcelPackageEntity());
            
            testObject.ApplyProfile(profile, shipment);
            
            Assert.Equal(1, shipment.IParcel.Packages.Count);
        }
        
        [Fact]
        public void ApplyProfile_DelegatesToSqlAdapter_WhenProfileHasFewerPackagesThanShipmentAndExtraPackageIsNotNew()
        {
            var sqlAdapter = mock.Mock<ISqlAdapter>();
            mock.Mock<ISqlAdapterFactory>().Setup(f => f.Create()).Returns(sqlAdapter);
            
            var extraPackage = new IParcelPackageEntity {IsNew = false};
            shipment.IParcel.Packages.Add(extraPackage);
            
            testObject.ApplyProfile(profile, shipment);
            
            sqlAdapter.Verify(a => a.DeleteEntity(extraPackage), Times.Once);
        }

        [Fact]
        public void ApplyProfile_AppliesSkuAndQuantities()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal("profileSkuAndQuantities", shipment.IParcel.Packages[0].SkuAndQuantities);
        }
        
        [Fact]
        public void ApplyProfile_AppliesiParcelAccountID_WheniParcelAccountIDIsNotZero()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(6, shipment.IParcel.IParcelAccountID);
        }
        
        [Fact]
        public void ApplyProfile_SetsiParcelAccountIDToFirstAccountID_WhenProfileAccountIDIsZeroAndiParcelAccountsExist()
        {
            accountRetriever.SetupGet(a => a.AccountsReadOnly).Returns(new[] { new IParcelAccountEntity {IParcelAccountID = 1 } });
            profile.IParcel.IParcelAccountID = 0;
            
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(1, shipment.IParcel.IParcelAccountID);
        }
        
        [Fact]
        public void ApplyProfile_AppliesService()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal((int) iParcelServiceType.Immediate, shipment.IParcel.Service);
        }
        
        [Fact]
        public void ApplyProfile_AppliesReference()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal("profileReference", shipment.IParcel.Reference);
        }
        
        [Fact]
        public void ApplyProfile_AppliesTrackByEmail()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.True(shipment.IParcel.TrackByEmail);
        }
        
        [Fact]
        public void ApplyProfile_AppliesTrackBySMS()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.True(shipment.IParcel.TrackBySMS);
        }
        
        [Fact]
        public void ApplyProfile_AppliesIsDeliveryDutyPaid()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.True(shipment.IParcel.IsDeliveryDutyPaid);
        }
        
        [Fact]
        public void ApplyProfile_CallsUpdateTotalWeight_WhenShipmentTotalWeightChanges()
        {
            testObject.ApplyProfile(profile, shipment);
            
            shipmentType.Verify(s => s.UpdateTotalWeight(shipment), Times.Once);
        }
        
        [Fact]
        public void ApplyProfile_CallsUpdateTotalWeight_WhenShipmentTotalWeightDoesNotChange()
        {
            profile.Packages[0].Weight = null;
            
            testObject.ApplyProfile(profile, shipment);
            
            shipmentType.Verify(s => s.UpdateTotalWeight(shipment), Times.Never);
        }
        
        [Fact]
        public void ApplyProfile_CallsUpdateDynamicShipmentData()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.True(shipment.IParcel.IsDeliveryDutyPaid);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}