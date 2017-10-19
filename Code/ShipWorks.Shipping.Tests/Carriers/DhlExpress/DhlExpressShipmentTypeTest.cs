using Autofac.Extras.Moq;
using Moq;
using ShipEngine.ApiClient.Model;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public void TrackShipment_DelegatesTrackingToWebClient()
        {
            var testObject = mock.Create<DhlExpressShipmentType>();

            var shipment = new ShipmentEntity()
            {
                DhlExpress = new DhlExpressShipmentEntity()
                {
                    ShipEngineLabelID = "test"
                }
            };

            testObject.TrackShipment(shipment);

            mock.Mock<IShipEngineWebClient>().Verify(c => c.Track("test", ApiLogSource.DHLExpress), Times.Once);
        }

        [Fact]
        public void TrackShipment_DelegatesTrackingInformationFromWebClient_ToTrackingResultFactory()
        {
            var testObject = mock.Create<DhlExpressShipmentType>();

            var shipment = new ShipmentEntity()
            {
                DhlExpress = new DhlExpressShipmentEntity()
                {
                    ShipEngineLabelID = "test"
                }
            };

            var trackingInformation = new TrackingInformation();

            mock.Mock<IShipEngineWebClient>()
                .Setup(c => c.Track(AnyString, ApiLogSource.DHLExpress))
                .Returns(Task.FromResult(trackingInformation));

            testObject.TrackShipment(shipment);

            mock.Mock<IShipEngineTrackingResultFactory>().Verify(f => f.Create(trackingInformation));
        }

        [Fact]
        public void TrackShipment_ReturnsTrackingResultCreatedByFactory()
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
                .Setup(c => c.Create(It.IsAny<TrackingInformation>()))
                .Returns(trackingResult);

            var testResult = testObject.TrackShipment(shipment);

            Assert.Equal(trackingResult, testResult);
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
            
            mock.Mock<IShipEngineTrackingResultFactory>()
                .Setup(c => c.Create(It.IsAny<TrackingInformation>()))
                .Throws(new Exception());

            var testResult = testObject.TrackShipment(shipment);
            string expectedSummary = "<a href='http://www.dhl.com/en/express/tracking.html?AWB=test&brand=DHL' style='color:blue; background-color:white'>Click here to see tracking information</a>";

            Assert.Equal(expectedSummary, testResult.Summary);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
