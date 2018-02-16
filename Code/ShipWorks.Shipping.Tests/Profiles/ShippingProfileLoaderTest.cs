using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Profiles
{
    public class ShippingProfileLoaderTest
    {
        private readonly AutoMock mock;
        private readonly ShippingProfileLoader testObject;
        private readonly Mock<ISqlAdapterFactory> sqlAdapterFactory;
        private readonly Mock<ISqlAdapter> sqlAdapter;

        public ShippingProfileLoaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            
            sqlAdapter = mock.Mock<ISqlAdapter>();
            sqlAdapterFactory = mock.Mock<ISqlAdapterFactory>();
            sqlAdapterFactory.Setup(a => a.Create()).Returns(sqlAdapter);

            testObject = mock.Create<ShippingProfileLoader>();
        }

        [Fact]
        public void LoadProfileData_DoesNotFetchEntityCollection_IfProfileIsNewAndRefreshIfPresentIsTrue()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = (int) ShipmentTypeCode.None };

            profile.IsNew = true;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntityCollection(profile.Packages, It.IsAny<RelationPredicateBucket>()), Times.Never);
        }

        [Fact]
        public void LoadProfileData_DoesFetchEntityCollection_IfProfileIsNotNewAndRefreshIfPresentIsTrue()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = (int) ShipmentTypeCode.None };

            profile.IsNew = false;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntityCollection(profile.Packages, It.IsAny<RelationPredicateBucket>()));
        }

        [Fact]
        public void LoadProfileData_DoesNotFetchEntityCollection_IfProfileIsNotNewAndRefreshIfPresentIsFalse()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = (int) ShipmentTypeCode.None };

            profile.IsNew = false;
            bool refreshIfPresent = false;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntityCollection(profile.Packages, It.IsAny<RelationPredicateBucket>()), Times.Never);
        }

        [Fact]
        public void LoadProfileData_DoesNotFetchEntityCollection_IfProfileIsNewAndRefreshIfPresentIsFalse()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = (int) ShipmentTypeCode.None };

            profile.IsNew = true;
            bool refreshIfPresent = false;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntityCollection(profile.Packages, It.IsAny<RelationPredicateBucket>()), Times.Never);
        }

        [Fact]
        public void LoadProfileData_DoesNotFetchEntity_WhenShipmentTypeCodeIsNone()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = (int) ShipmentTypeCode.None };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<IEntity2>()), Times.Never);
        }

        [Fact]
        public void LoadProfileData_DoesFetchEntity_WhenShipmentTypeCodeIsNotNone()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = (int) ShipmentTypeCode.UpsOnLineTools };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<IEntity2>()));
        }


        [Fact]
        public void LoadProfileData_DoesNotFetchEntity_WhenShipmentTypeCodeIsNull()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = null };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<IEntity2>()), Times.Never);
        }

        [Fact]
        public void LoadProfileData_FetchesUpsProfileEntithy_WhenProfileIsUps()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = (int) ShipmentTypeCode.UpsOnLineTools };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<UpsProfileEntity>()));
        }

        [Fact]
        public void LoadProfileData_FetchesFedExProfileEntity_WhenProfileIsFedEx()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = (int) ShipmentTypeCode.FedEx };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<FedExProfileEntity>()));
        }

        [Fact]
        public void LoadProfileData_FetchesOnTracProfileEntity_WhenProfileIsOnTrac()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = (int) ShipmentTypeCode.OnTrac };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<OnTracProfileEntity>()));
        }

        [Fact]
        public void LoadProfileData_FetchesIParcelProfileEntity_WhenProfileIsIParcel()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = (int) ShipmentTypeCode.iParcel };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<IParcelProfileEntity>()));
        }
        
        [Fact]
        public void LoadProfileData_FetchesOtherProfileEntity_WhenProfileIsOther()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = (int) ShipmentTypeCode.Other };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<OtherProfileEntity>()));
        }

        [Fact]
        public void LoadProfileData_FetchesBestRateProfileEntity_WhenProfileIsBestRate()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = (int) ShipmentTypeCode.BestRate };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<BestRateProfileEntity>()));
        }

        [Fact]
        public void LoadProfileData_FetchesAmazonProfileEntity_WhenProfileIsAmazon()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = (int) ShipmentTypeCode.Amazon };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<AmazonProfileEntity>()));
        }

        [Fact]
        public void LoadProfileData_FetchesDhlExpressProfileEntity_WhenProfileIsDhlExpress()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = (int) ShipmentTypeCode.DhlExpress };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<DhlExpressProfileEntity>()));
        }

        [Fact]
        public void LoadProfileData_FetchesAsendiaProfileEntity_WhenProfileIsAsendia()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = (int) ShipmentTypeCode.Asendia };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<AsendiaProfileEntity>()));
        }

        [Fact]
        public void LoadProfileData_FetchesChildrenProfileEntity_WhenProfileIsUsps()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = (int) ShipmentTypeCode.Usps };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;
            
            sqlAdapter.Setup(a => a.FetchEntity(It.IsAny<IEntity2>())).Callback<IEntity2>((e) => e.Fields.State = EntityState.Fetched);
            
            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<PostalProfileEntity>()));
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<UspsProfileEntity>()));
        }

        [Fact]
        public void LoadProfileData_FetchesChildrenProfileEntity_WhenProfileIsUspsExpress1()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = (int) ShipmentTypeCode.Express1Usps };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            sqlAdapter.Setup(a => a.FetchEntity(It.IsAny<IEntity2>())).Callback<IEntity2>((e) => e.Fields.State = EntityState.Fetched);

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<PostalProfileEntity>()));
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<UspsProfileEntity>()));
        }

        [Fact]
        public void LoadProfileData_FetchesChildrenProfileEntity_WhenProfileIsEndicia()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = (int) ShipmentTypeCode.Endicia };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            sqlAdapter.Setup(a => a.FetchEntity(It.IsAny<IEntity2>())).Callback<IEntity2>((e) => e.Fields.State = EntityState.Fetched);

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<PostalProfileEntity>()));
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<EndiciaProfileEntity>()));
        }

        [Fact]
        public void LoadProfileData_FetchesChildrenProfileEntity_WhenProfileIsEndiciaExpress1()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = (int) ShipmentTypeCode.Express1Endicia };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            sqlAdapter.Setup(a => a.FetchEntity(It.IsAny<IEntity2>())).Callback<IEntity2>((e) => e.Fields.State = EntityState.Fetched);

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<PostalProfileEntity>()));
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<EndiciaProfileEntity>()));
        }
        
        [Fact]
        public void LoadProfileData_FetchesChildrenProfileEntity_WhenProfileIsPostalWebTools()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = (int) ShipmentTypeCode.PostalWebTools };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            sqlAdapter.Setup(a => a.FetchEntity(It.IsAny<IEntity2>())).Callback<IEntity2>((e) => e.Fields.State = EntityState.Fetched);

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<PostalProfileEntity>()));
        }
    }
}
