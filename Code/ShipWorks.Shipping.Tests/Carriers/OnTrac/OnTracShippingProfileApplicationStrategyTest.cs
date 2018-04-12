using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.OnTrac
{
    public class OnTracShippingProfileApplicationStrategyTest
    {
        private readonly Mock<ICarrierAccountRetriever<OnTracAccountEntity, IOnTracAccountEntity>> accountRetriever;
        private readonly Mock<ShipmentType> shipmentType;
        private readonly ShipmentEntity shipment;
        private readonly ShippingProfileEntity profile;
        private readonly OnTracShippingProfileApplicationStrategy testObject;

        public OnTracShippingProfileApplicationStrategyTest()
        {
            var mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shipmentType = mock.Mock<ShipmentType>();
            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            shipmentTypeManager.Setup(s => s.Get(ShipmentTypeCode.OnTrac)).Returns(shipmentType);
            accountRetriever = mock.Mock<ICarrierAccountRetriever<OnTracAccountEntity, IOnTracAccountEntity>>();
            
            shipment = new ShipmentEntity
            {
                OnTrac = new OnTracShipmentEntity
                {
                    OnTracAccountID = 9,
                    Service = (int) OnTracServiceType.Ground,
                    PackagingType = (int) OnTracPackagingType.Letter,
                    Insurance = false,
                    SaturdayDelivery = false,
                    SignatureRequired = false,
                    DimsProfileID = 9,
                    DimsWeight = 9,
                    DimsLength = 9,
                    DimsHeight = 9,
                    DimsWidth = 9,
                    DimsAddWeight = false,
                    Reference1 = "shipmentReference1",
                    Reference2 = "shipmentReference2",
                    Instructions = "shipmentInstructions"
                },
                ContentWeight = 9,
                Insurance = false,
                ResidentialDetermination = (int) ResidentialDeterminationType.Residential
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
                OnTrac = new OnTracProfileEntity
                {
                    OnTracAccountID = 6,
                    Service = (int) OnTracServiceType.SunriseGold,
                    PackagingType = (int) OnTracPackagingType.Package,
                    SaturdayDelivery = true,
                    SignatureRequired = true,
                    Reference1 = "profileReference1",
                    Reference2 = "profileReference2",
                    Instructions = "profileInstructions",
                    ResidentialDetermination = (int) ResidentialDeterminationType.FromAddressValidation
                },
                Insurance = true,
            };

            testObject = mock.Create<OnTracShippingProfileApplicationStrategy>();
        }
        
        [Fact]
        public void ApplyProfile_AppliesOnTracAccountID_WhenOnTracAccountIDIsNotZero()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(6, shipment.OnTrac.OnTracAccountID);
        }
        
        [Fact]
        public void ApplyProfile_SetsOnTracAccountIDToFirstAccountID_WhenProfileAccountIDIsZeroAndOnTracAccountsExist()
        {
            accountRetriever.SetupGet(a => a.AccountsReadOnly).Returns(new[] { new OnTracAccountEntity { OnTracAccountID = 1 } });
            profile.OnTrac.OnTracAccountID = 0;
            
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(1, shipment.OnTrac.OnTracAccountID);
        }
        
        [Fact]
        public void ApplyProfile_AppliesService()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal((int) OnTracServiceType.SunriseGold, shipment.OnTrac.Service);
        }
        
        [Fact]
        public void ApplyProfile_AppliesPackagingType()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal((int) OnTracPackagingType.Package, shipment.OnTrac.PackagingType);
        }
        
        [Fact]
        public void ApplyProfile_AppliesInsurance()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.True(shipment.OnTrac.Insurance);
        }
        
        [Fact]
        public void ApplyProfile_AppliesSaturdayDelivery()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(true, shipment.OnTrac.SaturdayDelivery);
        }
        
        [Fact]
        public void ApplyProfile_AppliesSignatureRequired()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(true, shipment.OnTrac.SignatureRequired);
        }
        
        [Fact]
        public void ApplyProfile_AppliesWeight_WhenProfileWeightIsNotZero()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(5, shipment.ContentWeight);
        }
        
        [Fact]
        public void ApplyProfile_DoesNotApplyWeight_WhenProfileWeightIsNull()
        {
            profile.Packages[0].Weight = null;
            
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(9, shipment.ContentWeight);
        }
        
        [Fact]
        public void ApplyProfile_DoesNotApplyWeight_WhenProfileWeightIsZero()
        {
            profile.Packages[0].Weight = 0;
            
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(9, shipment.ContentWeight);
        }
        
        [Fact]
        public void ApplyProfile_AppliesDimsProfileID()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(0, shipment.OnTrac.DimsProfileID);
        }
        
        [Fact]
        public void ApplyProfile_AppliesDimsWeight()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(4, shipment.OnTrac.DimsWeight);
        }
        
        [Fact]
        public void ApplyProfile_AppliesDimsLength()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(1, shipment.OnTrac.DimsLength);
        }
        
        [Fact]
        public void ApplyProfile_AppliesDimsHeight()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(3, shipment.OnTrac.DimsHeight);
        }
        
        [Fact]
        public void ApplyProfile_AppliesDimsWidth()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(2, shipment.OnTrac.DimsWidth);
        }
        
        [Fact]
        public void ApplyProfile_AppliesDimsAddWeight()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.True(shipment.OnTrac.DimsAddWeight);
        }
        
        [Fact]
        public void ApplyProfile_AppliesReference1()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal("profileReference1", shipment.OnTrac.Reference1);
        }
        
        [Fact]
        public void ApplyProfile_AppliesReference2()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal("profileReference2", shipment.OnTrac.Reference2);
        }
        
        [Fact]
        public void ApplyProfile_AppliesInstructions()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal("profileInstructions", shipment.OnTrac.Instructions);
        }
        
        [Fact]
        public void ApplyProfile_AppliesResidentialDetermination()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal((int) ResidentialDeterminationType.FromAddressValidation, shipment.ResidentialDetermination);
        }
        
        
        [Fact]
        public void ApplyProfile_CallsUpdateTotalWeight_WhenShipmentTotalWeightChanges()
        {
            testObject.ApplyProfile(profile, shipment);
            
            shipmentType.Verify(s => s.UpdateTotalWeight(shipment), Times.Once);
        }
        
        [Fact]
        public void ApplyProfile_CallsUpdateDynamicShipmentData()
        {
            testObject.ApplyProfile(profile, shipment);

            shipmentType.Verify(s => s.UpdateDynamicShipmentData(shipment), Times.Once);
        }
    }
}