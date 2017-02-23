using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Interapptive.Shared.IO.Hardware.Scales;
using Interapptive.Shared.Metrics;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    public class AutoWeighServiceTest : IDisposable
    {
        private readonly AutoMock mock;
        private AutoWeighService testObject;
        private ITrackedDurationEvent trackedDurationEvent;

        public AutoWeighServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            trackedDurationEvent = mock.Mock<ITrackedDurationEvent>().Object;

            testObject = mock.Create<AutoWeighService>();
        }

        [Fact]
        public async void AutoWeighService_DoesNotReadScale_WhenAutoWeighIsOff()
        {
            SetAutoWeighSetting(false);

            var uspsShipmentEntity = new ShipmentEntity()
            {
                ShipmentTypeCode = ShipmentTypeCode.Usps
            };

            var shipments = new List<ShipmentEntity>() {uspsShipmentEntity};

            
            await testObject.Apply(shipments, trackedDurationEvent);

            mock.Mock<IScaleReader>()
                .Verify(s=>s.ReadScale(), Times.Never);
        }



        private void SetAutoWeighSetting(bool allowAutoWeigh)
        {
            mock.Mock<IAutoPrintPermissions>()
                .Setup(p => p.AutoWeighOn())
                .Returns(allowAutoWeigh);
        }


        public void Dispose()
        {
            mock.Dispose();
        }

    }
}