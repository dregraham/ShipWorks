using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Messaging;
using System.Reactive.Subjects;
using Interapptive.Shared.Threading;
using ShipWorks.Tests.Shared;
using Xunit;
using ShipWorks.Shipping.Services;
using ShipWorks.OrderLookup;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;
using Interapptive.Shared.IO.Hardware.Scales;
using ShipWorks.OrderLookup.ShipmentModelPipelines;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.ObservableRegistrations
{
    public class ChangeDimensionsPipelineTest
    {
        private readonly AutoMock mock;
        private readonly IMessenger messenger;
        readonly Mock<IPackageAdapter> package;
        readonly Mock<IOrderLookupShipmentModel> model;
        readonly ChangeDimensionsPipeline testObject;

        public ChangeDimensionsPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            messenger = new TestMessenger();
            mock.Provide<IMessenger>(messenger);
            mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());

            package = mock.CreateMock<IPackageAdapter>();
            model = mock.CreateMock<IOrderLookupShipmentModel>();
            model.SetupGet(m => m.PackageAdapters)
                .Returns(new[] { package.Object });

            testObject = mock.Create<ChangeDimensionsPipeline>();
        }

        [Fact]
        public void Register_AppliesDims_FromMessage()
        {
            testObject.Register(model.Object);
            messenger.Send(new ChangeDimensionsMessage(this, ScaleReadResult.Success(1, 2, 3, 4, ScaleType.Cubiscan)));
            package.VerifySet(p => p.DimsProfileID = 0);
            package.VerifySet(p => p.DimsLength = 2);
            package.VerifySet(p => p.DimsWidth = 3);
            package.VerifySet(p => p.DimsHeight = 4);
            package.VerifySet(p => p.ApplyAdditionalWeight = false);
        }

        [Fact]
        public void Register_DoesNotApplyDims_WhenNoDimensions()
        {
            testObject.Register(model.Object);
            messenger.Send(new ChangeDimensionsMessage(this, ScaleReadResult.Success(4, ScaleType.Cubiscan)));
            package.VerifySet(p => p.DimsProfileID = It.IsAny<long>(), Times.Never);
        }
    }
}
