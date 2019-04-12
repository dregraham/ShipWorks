﻿using System;
using System.Collections.Generic;
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
        private readonly IDictionary<long, (bool before, bool after)> insuranceSelections = new Dictionary<long, (bool before, bool after)>();

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
            
            var originalShipments = new[] { new ShipmentEntity() { ShipmentID = 123, ShipmentTypeCode = ShipmentTypeCode.AmazonSFP, Insurance = true, IsNew = false} };
            var newShipments = new[] { new ShipmentEntity() { ShipmentID = 123, ShipmentTypeCode = ShipmentTypeCode.Usps, Insurance = false, IsNew = false } };

            SendMessage(originalShipments, newShipments);

            insuranceBehaviorChangeViewModel.Verify(i => i.Notify(insuranceSelections));
        }

        [Fact]
        public void ProcessMessage_DoesNotDelegatesToInsuranceBehaviorChangeViewModel_WhenOriginalAndNewShipmentsAreTheSameShipmentType()
        {
            testObject.InitializeForCurrentSession();

            var originalShipments = new[] { new ShipmentEntity() { ShipmentID = 123, ShipmentTypeCode = ShipmentTypeCode.Usps, Insurance = false, IsNew = false } };
            var newShipments = new[] { new ShipmentEntity() { ShipmentID = 123, ShipmentTypeCode = ShipmentTypeCode.Usps, Insurance = true, IsNew = false } };

            SendMessage(originalShipments, newShipments);

            insuranceBehaviorChangeViewModel.Verify(i => i.Notify(It.IsAny<Dictionary<long, bool>>(), It.IsAny<Dictionary<long, bool>>()), Times.Never);
        }

        [Fact]
        public void ProcessMessage_DoesNotDelegatesToInsuranceBehaviorChangeViewModel_WhenOriginalShipmentIsNew()
        {
            testObject.InitializeForCurrentSession();

            var originalShipments = new[] { new ShipmentEntity() { ShipmentID = 123, ShipmentTypeCode = ShipmentTypeCode.Usps, Insurance = false, IsNew = true } };
            var newShipments = new[] { new ShipmentEntity() { ShipmentID = 123, ShipmentTypeCode = ShipmentTypeCode.Usps, Insurance = true, IsNew = false } };

            SendMessage(originalShipments, newShipments);

            insuranceBehaviorChangeViewModel.Verify(i => i.Notify(It.IsAny<Dictionary<long, bool>>(), It.IsAny<Dictionary<long, bool>>()), Times.Never);
        }

        private void SendMessage(ShipmentEntity[] originalShipments, ShipmentEntity[] newShipments)
        {
            insuranceSelections.Add(originalShipments[0].ShipmentID, (originalShipments[0].Insurance, newShipments[0].Insurance));

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
