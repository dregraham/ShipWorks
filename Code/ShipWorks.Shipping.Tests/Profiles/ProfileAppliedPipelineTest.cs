using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Microsoft.Reactive.Testing;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Profiles
{
    public class ProfileAppliedPipelineTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly TestMessenger testMessenger;
        private readonly TestScheduler scheduler;
        private readonly ProfileAppliedPipeline testObject;
        private readonly Mock<IInsuranceBehaviorChangeViewModel> insuranceBehaviorChangeViewModel;
        private readonly Dictionary<long, bool> originalInsuranceSelection = new Dictionary<long, bool>();
        private readonly Dictionary<long, bool> newInsuranceSelection = new Dictionary<long, bool>();

        public ProfileAppliedPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);

            Mock<ISchedulerProvider> scheduleProvider = mock.WithMockImmediateScheduler();

            scheduler = new TestScheduler();
            scheduleProvider.Setup(s => s.Default).Returns(scheduler);

            insuranceBehaviorChangeViewModel = mock.Mock<IInsuranceBehaviorChangeViewModel>();

            mock.MockFunc<IInsuranceBehaviorChangeViewModel>(insuranceBehaviorChangeViewModel);

            testObject = mock.Create<ProfileAppliedPipeline>();
        }

        [Fact]
        public void ProcessMessage_DelegatesToInsuranceBehaviorChangeViewModel_WhenOriginalAndNewShipmentsAreDifferent()
        {
            testObject.InitializeForCurrentSession();
            
            var originalShipments = new[] { new ShipmentEntity() { ShipmentID = 123, ShipmentTypeCode = ShipmentTypeCode.Amazon, Insurance = true } };
            var newShipments = new[] { new ShipmentEntity() { ShipmentID = 123, ShipmentTypeCode = ShipmentTypeCode.Usps, Insurance = false } };

            SendMessage(originalShipments, newShipments);

            insuranceBehaviorChangeViewModel.Verify(i => i.Notify(originalInsuranceSelection, newInsuranceSelection));
        }

        [Fact]
        public void ProcessMessage_DelegatesToInsuranceBehaviorChangeViewModel_WhenOriginalAndNewShipmentsAreTheSame()
        {
            testObject.InitializeForCurrentSession();

            var originalShipments = new[] { new ShipmentEntity() { ShipmentID = 123, ShipmentTypeCode = ShipmentTypeCode.Usps, Insurance = false } };
            var newShipments = new[] { new ShipmentEntity() { ShipmentID = 123, ShipmentTypeCode = ShipmentTypeCode.Usps, Insurance = true} };

            SendMessage(originalShipments, newShipments);

            insuranceBehaviorChangeViewModel.Verify(i => i.Notify(It.IsAny<Dictionary<long, bool>>(), It.IsAny<Dictionary<long, bool>>()), Times.Never);
        }

        private void SendMessage(ShipmentEntity[] originalShipments, ShipmentEntity[] newShipments)
        {
            originalInsuranceSelection.Add(originalShipments[0].ShipmentID, originalShipments[0].Insurance);

            newInsuranceSelection.Add(newShipments[0].ShipmentID, newShipments[0].Insurance);

            testMessenger.Send(new ProfileAppliedMessage(this, originalShipments, newShipments));
            scheduler.Start();
        }

        public void Dispose()
        {
            testObject?.Dispose();
            mock?.Dispose();
        }
    }
}
