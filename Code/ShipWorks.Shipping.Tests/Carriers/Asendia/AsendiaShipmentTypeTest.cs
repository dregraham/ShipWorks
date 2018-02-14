using Autofac.Extras.Moq;
using Interapptive.Shared.Enums;
using Moq;
using ShipEngine.ApiClient.Model;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Asendia;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.ExtensionMethods;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Asendia
{
    public class AsendiaShipmentTypeTest : IDisposable
    {
        AutoMock mock;

        public AsendiaShipmentTypeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }
        
        [Fact]
        public void ShipmentTypeCode_IsAsendia()
        {
            var testObject = mock.Create<AsendiaShipmentType>();

            Assert.Equal(ShipmentTypeCode.Asendia, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void ConfigurePrimaryProfile_SetsAsendiaProfileDefaults()
        {
            var repo = mock.Mock<ICarrierAccountRepository<AsendiaAccountEntity, IAsendiaAccountEntity>>();
            repo.SetupGet(r => r.AccountsReadOnly).Returns(new[] { new AsendiaAccountEntity() { AsendiaAccountID = 123456789 } });

            AsendiaShipmentType testObject = mock.Create<AsendiaShipmentType>();

            ShippingProfileEntity profile = new ShippingProfileEntity()
            {
                Asendia = new AsendiaProfileEntity()                
            };

            testObject.ConfigurePrimaryProfile(profile);
            PackageProfileEntity packageProfile = profile.PackageProfile.Single();

            Assert.Equal(123456789, profile.Asendia.AsendiaAccountID);
            Assert.Equal(AsendiaServiceType.AsendiaPriorityTracked, profile.Asendia.Service);
            Assert.Equal((int) ShipEngineContentsType.Merchandise, profile.Asendia.Contents);
            Assert.Equal((int) ShipEngineNonDeliveryType.ReturnToSender, profile.Asendia.NonDelivery);
            Assert.False(profile.Asendia.NonMachinable);
            Assert.Equal(0, packageProfile.Weight);
            Assert.Equal(0, packageProfile.DimsProfileID);
            Assert.Equal(0, packageProfile.DimsLength);
            Assert.Equal(0, packageProfile.DimsWidth);
            Assert.Equal(0, packageProfile.DimsHeight);
            Assert.Equal(0, packageProfile.DimsWeight);
            Assert.True(packageProfile.DimsAddWeight);
        }

        [Fact]
        public void ConfigurePrimaryProfile_SetsAsendiaAccountIDToZero_WhenNoAccounts()
        {
            var repo = mock.Mock<ICarrierAccountRepository<AsendiaAccountEntity, IAsendiaAccountEntity>>();
            repo.SetupGet(r => r.AccountsReadOnly).Returns(new AsendiaAccountEntity[0]);

            AsendiaShipmentType testObject = mock.Create<AsendiaShipmentType>();

            ShippingProfileEntity profile = new ShippingProfileEntity()
            {
                Asendia = new AsendiaProfileEntity()
            };

            testObject.ConfigurePrimaryProfile(profile);

            Assert.Equal(0, profile.Asendia.AsendiaAccountID);
        }

        [Fact]
        public void ConfigurePrimaryProfile_SetsAsendiaAccountIDToFirstAccountID_WhenAccountsExist()
        {
            var repo = mock.Mock<ICarrierAccountRepository<AsendiaAccountEntity, IAsendiaAccountEntity>>();
            repo.SetupGet(r => r.AccountsReadOnly).Returns(new[] { new AsendiaAccountEntity() { AsendiaAccountID = 123456789 }, new AsendiaAccountEntity() { AsendiaAccountID = 987654321 } });

            AsendiaShipmentType testObject = mock.Create<AsendiaShipmentType>();

            ShippingProfileEntity profile = new ShippingProfileEntity()
            {
                Asendia = new AsendiaProfileEntity()
            };

            testObject.ConfigurePrimaryProfile(profile);

            Assert.Equal(123456789, profile.Asendia.AsendiaAccountID);
        }

        [Fact]
        public void TrackShipment_DelegatesTrackingToWebClient()
        {
            var testObject = mock.Create<AsendiaShipmentType>();

            var shipment = new ShipmentEntity()
            {
                Asendia = new AsendiaShipmentEntity()
                {
                    ShipEngineLabelID = "test"
                }
            };

            testObject.TrackShipment(shipment);

            mock.Mock<IShipEngineWebClient>().Verify(c => c.Track("test", ApiLogSource.Asendia), Times.Once);
        }

        [Fact]
        public void TrackShipment_DelegatesTrackingInformationFromWebClient_ToTrackingResultFactory()
        {
            var testObject = mock.Create<AsendiaShipmentType>();

            var shipment = new ShipmentEntity()
            {
                Asendia = new AsendiaShipmentEntity()
                {
                    ShipEngineLabelID = "test"
                }
            };

            var trackingInformation = new TrackingInformation();

            mock.Mock<IShipEngineWebClient>()
                .Setup(c => c.Track(ParameterShorteners.AnyString, ApiLogSource.Asendia))
                .Returns(Task.FromResult(trackingInformation));

            testObject.TrackShipment(shipment);

            mock.Mock<IShipEngineTrackingResultFactory>().Verify(f => f.Create(trackingInformation));
        }

        [Fact]
        public void TrackShipment_ReturnsTrackingResultCreatedByFactory()
        {
            var testObject = mock.Create<AsendiaShipmentType>();

            var shipment = new ShipmentEntity()
            {
                Asendia = new AsendiaShipmentEntity()
                {
                    ShipEngineLabelID = "test"
                }
            };

            TrackingResult trackingResult = new TrackingResult();

            mock.Mock<IShipEngineTrackingResultFactory>()
                .Setup(c => c.Create(It.IsAny<TrackingInformation>()))
                .Returns(trackingResult);

            var testResult = testObject.TrackShipment(shipment);

            Assert.Equal(trackingResult, testResult);
        }

        [Fact]
        public void TrackShipment_ReturnsCannedResult_WhenExceptionIsThrown()
        {
            var testObject = mock.Create<AsendiaShipmentType>();

            var shipment = new ShipmentEntity()
            {
                TrackingNumber = "test",
                Asendia = new AsendiaShipmentEntity()
                {
                    ShipEngineLabelID = "foo"
                }
            };

            mock.Mock<IShipEngineTrackingResultFactory>()
                .Setup(c => c.Create(It.IsAny<TrackingInformation>()))
                .Throws(new Exception());

            var testResult = testObject.TrackShipment(shipment);
            string expectedSummary = $"<a href='http://tracking.asendiausa.com/t.aspx?p={shipment.TrackingNumber}' style='color:blue; background-color:white'>Click here to view tracking information online</a>";

            Assert.Equal(expectedSummary, testResult.Summary);
        }

        [Theory]
        [InlineData(true, 9.99)]
        [InlineData(false, 6.66)]
        public void Insured_ReturnsInsuranceFromShipment(bool insured, decimal insuranceValue)
        {
            ShipmentEntity shipment = new ShipmentEntity
            {
                Insurance = !insured,
                Asendia = new AsendiaShipmentEntity
                {
                    Insurance = insured,
                    InsuranceValue = insuranceValue
                }
            };

            var testObject = mock.Create<AsendiaShipmentType>();
            ShipmentParcel parcel = testObject.GetParcelDetail(shipment, 0);

            Assert.Equal(insured, parcel.Insurance.Insured);
            Assert.Equal(insuranceValue, parcel.Insurance.InsuranceValue);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
