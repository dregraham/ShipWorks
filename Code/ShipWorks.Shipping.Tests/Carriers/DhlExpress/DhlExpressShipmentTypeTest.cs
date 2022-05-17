using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Tests.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Carriers.DhlExpress
{
    public class DhlExpressShipmentTypeTest : IDisposable
    {
        private readonly DhlExpressShipmentType testObject;
        readonly AutoMock mock;

        public DhlExpressShipmentTypeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<DhlExpressShipmentType>();
        }

        [Fact]
        public void SupportsMultiplePackages_ReturnsTrue()
        {
            Assert.True(testObject.SupportsMultiplePackages);
        }

        [Fact]
        public void ShipmentTypeCode_ReturnsDhlExpress()
        {
            Assert.Equal(ShipmentTypeCode.DhlExpress, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void GetPackageAdapters_ReturnsPackageAdapterPerPackage()
        {
            var shipment = new ShipmentEntity();
            var dhlShipment = new DhlExpressShipmentEntity();
            dhlShipment.Packages.Add(new DhlExpressPackageEntity());
            dhlShipment.Packages.Add(new DhlExpressPackageEntity());
            dhlShipment.Packages.Add(new DhlExpressPackageEntity());

            shipment.DhlExpress = dhlShipment;
            
            Assert.Equal(3, testObject.GetPackageAdapters(shipment).Count());
        }
        
        [Fact]
        public void RedistributeContentWeight_SetsPackageWeightToShipmentWeightDividedByPackageCount()
        {
            var shipment = new ShipmentEntity();
            shipment.ContentWeight = 123;
            var dhlShipment = new DhlExpressShipmentEntity();
            dhlShipment.Packages.Add(new DhlExpressPackageEntity());
            dhlShipment.Packages.Add(new DhlExpressPackageEntity());
            dhlShipment.Packages.Add(new DhlExpressPackageEntity());

            shipment.DhlExpress = dhlShipment;

            Assert.True(DhlExpressShipmentType.RedistributeContentWeight(shipment));
            Assert.Equal(41, shipment.DhlExpress.Packages[0].Weight);
        }


        [Fact]
        public void RedistributeContentWeight_DoesNothingWhenPackageWeightAndShipmentWeightMatch()
        {
            var shipment = new ShipmentEntity();
            shipment.ContentWeight = 123;
            var dhlShipment = new DhlExpressShipmentEntity();
            dhlShipment.Packages.Add(new DhlExpressPackageEntity() { Weight = 41 });
            dhlShipment.Packages.Add(new DhlExpressPackageEntity() { Weight = 41 });
            dhlShipment.Packages.Add(new DhlExpressPackageEntity() { Weight = 41 });

            shipment.DhlExpress = dhlShipment;

            Assert.False(DhlExpressShipmentType.RedistributeContentWeight(shipment));
        }

        [Fact]
        public void UpdateTotalWeight_SetsShipmentContentWeight()
        {
            var shipment = new ShipmentEntity();
            shipment.ContentWeight = 123;
            var dhlShipment = new DhlExpressShipmentEntity();
            dhlShipment.Packages.Add(new DhlExpressPackageEntity() { Weight = 2 });
            dhlShipment.Packages.Add(new DhlExpressPackageEntity() { Weight = 3 });
            dhlShipment.Packages.Add(new DhlExpressPackageEntity() { Weight = 4 });

            shipment.DhlExpress = dhlShipment;

            testObject.UpdateTotalWeight(shipment);

            Assert.Equal(9, shipment.ContentWeight);
        }

        [Fact]
        public void UpdateTotalWeight_SetsShipmentTotalWeight()
        {
            var shipment = new ShipmentEntity();
            shipment.ContentWeight = 123;
            var dhlShipment = new DhlExpressShipmentEntity();
            dhlShipment.Packages.Add(new DhlExpressPackageEntity() { Weight = 2 });
            dhlShipment.Packages.Add(new DhlExpressPackageEntity() { Weight = 3 });
            dhlShipment.Packages.Add(new DhlExpressPackageEntity() { Weight = 4, DimsAddWeight = true, DimsWeight = 3 });

            shipment.DhlExpress = dhlShipment;

            testObject.UpdateTotalWeight(shipment);

            Assert.Equal(12, shipment.TotalWeight);
        }

        [Fact]
        public void GetParcelCount_ReturnsParcelCount()
        {
            var shipment = new ShipmentEntity();
            shipment.ContentWeight = 123;
            var dhlShipment = new DhlExpressShipmentEntity();
            dhlShipment.Packages.Add(new DhlExpressPackageEntity() { Weight = 2 });
            dhlShipment.Packages.Add(new DhlExpressPackageEntity() { Weight = 3 });
            dhlShipment.Packages.Add(new DhlExpressPackageEntity() { Weight = 4, DimsAddWeight = true, DimsWeight = 3 });

            shipment.DhlExpress = dhlShipment;
            
            Assert.Equal(3,testObject.GetParcelCount(shipment));
        }

        [Fact]
        public void TrackShipment_CallsTrackWithCarrierAndTrackingNumber_WhenShipEngineLabelIDIsNull()
        {
            var shipment = new ShipmentEntity
            {
                TrackingNumber = "foo",
                DhlExpress = new DhlExpressShipmentEntity
                {
                    ShipEngineLabelID = null
                }
            };

            var webClient = mock.Mock<IShipEngineWebClient>();

            var testObject = mock.Create<DhlExpressShipmentType>();

            testObject.TrackShipment(shipment);

            webClient.Verify(x => x.Track("dhl_express", "foo", ApiLogSource.DHLExpress), Times.Once);
        }

        [Fact]
        public void TrackShipment_CallsTrackWithShipEngineLabelID_WhenShipEngineLabelIDExists()
        {
            var shipment = new ShipmentEntity
            {
                TrackingNumber = "foo",
                DhlExpress = new DhlExpressShipmentEntity
                {
                    ShipEngineLabelID = "bar"
                }
            };

            var webClient = mock.Mock<IShipEngineWebClient>();

            var testObject = mock.Create<DhlExpressShipmentType>();

            testObject.TrackShipment(shipment);

            webClient.Verify(x => x.Track("bar", ApiLogSource.DHLExpress), Times.Once);
        }

        [Fact]
        public void TrackShipment_ReturnsCannedResult_WhenExceptionIsThrown()
        {
            var testObject = mock.Create<DhlExpressShipmentType>();

            var shipment = new ShipmentEntity()
            {
                TrackingNumber = "test",
                DhlExpress = new DhlExpressShipmentEntity()
                {
                    ShipEngineLabelID = "foo"
                }
            };
            
            mock.Mock<IShipEngineWebClient>()
                .Setup(c => c.Track("foo", ApiLogSource.DHLExpress))
                .Throws(new Exception());

            var testResult = testObject.TrackShipment(shipment);
            string expectedSummary = "<a href='http://www.dhl.com/en/express/tracking.html?AWB=test&brand=DHL' style='color:blue; background-color:white'>Click here to view tracking information online</a>";

            Assert.Equal(expectedSummary, testResult.Summary);
        }

        [Fact]
        public void Track_DelegatesTrackingInformationFromWebClient_ToTrackingResultFactory()
        {
            var testObject = mock.Create<DhlExpressShipmentType>();

            var shipment = new ShipmentEntity()
            {
                DhlExpress = new DhlExpressShipmentEntity()
                {
                    ShipEngineLabelID = "test"
                }
            };

            var trackingInformation = new ShipWorks.Shipping.ShipEngine.DTOs.TrackingInformation();

            mock.Mock<IShipEngineWebClient>()
                .Setup(c => c.Track(AnyString, ApiLogSource.DHLExpress))
                .Returns(Task.FromResult(trackingInformation));

            testObject.TrackShipment(shipment);

            mock.Mock<IShipEngineTrackingResultFactory>().Verify(f => f.Create(trackingInformation));
        }

        [Fact]
        public void Track_ReturnsTrackingResultCreatedByFactory()
        {
            var testObject = mock.Create<DhlExpressShipmentType>();

            var shipment = new ShipmentEntity()
            {
                DhlExpress = new DhlExpressShipmentEntity()
                {
                    ShipEngineLabelID = "test"
                }
            };

            TrackingResult trackingResult = new TrackingResult();

            mock.Mock<IShipEngineTrackingResultFactory>()
                .Setup(c => c.Create(It.IsAny<ShipWorks.Shipping.ShipEngine.DTOs.TrackingInformation>()))
                .Returns(trackingResult);

            var testResult = testObject.TrackShipment(shipment);

            Assert.Equal(trackingResult, testResult);
        }

        [Theory]
        [InlineData(true, 1.1, 1.2, 2.3)]
        [InlineData(false, 1.1, 1.2, 1.2)]
        [InlineData(true, 0, 1.2, 1.2)]
        [InlineData(false, 0, 1.2, 1.2)]
        public void GetParcelDetail_HasCorrectTotalWeight(bool dimsAddWeight, double dimsWeight, double weight, double expectedTotalWeight)
        {
            var testObject = mock.Create<DhlExpressShipmentType>();

            var shipment = new ShipmentEntity()
            {
                TrackingNumber = "test",
                DhlExpress = new DhlExpressShipmentEntity()
            };
            shipment.DhlExpress.Packages.Add(new DhlExpressPackageEntity()
            {
                DimsAddWeight = dimsAddWeight,
                DimsWeight = dimsWeight,
                Weight = weight
            });

            var parcelDetail = testObject.GetParcelDetail(shipment, 0);

            Assert.Equal(expectedTotalWeight, parcelDetail.TotalWeight);
        }


        [Theory]
        [InlineData("", true, "")]
        [InlineData("foo", false, "")]
        [InlineData("foo", true, "http://www.dhl.com/en/express/tracking.html?AWB=foo&brand=DHL")]
        public void GetCarrierTrackingUrl_ReturnsCorrectTrackingUrl(string trackingNumber, bool processed, string expectedUrl)
        {
            ShipmentEntity shipment = new ShipmentEntity
            {
                TrackingNumber = trackingNumber,
                Processed = processed
            };

            var trackingUrl = testObject.GetCarrierTrackingUrl(shipment);
            Assert.Equal(expectedUrl, trackingUrl);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
