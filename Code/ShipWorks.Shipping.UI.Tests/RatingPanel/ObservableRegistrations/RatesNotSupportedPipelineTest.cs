﻿using System;
using System.Linq;
using System.Reactive.Subjects;
using Autofac.Extras.Moq;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Threading;
using ShipWorks.Messaging.Messages.Dialogs;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.UI.RatingPanel;
using ShipWorks.Shipping.UI.RatingPanel.ObservableRegistrations;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.RatingPanel.ObservableRegistrations
{
    public class RatesNotSupportedPipelineTest : IDisposable
    {
        readonly AutoMock mock;
        readonly Subject<IShipWorksMessage> subject;

        public RatesNotSupportedPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            subject = new Subject<IShipWorksMessage>();
            mock.Provide<IObservable<IShipWorksMessage>>(subject);
            mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());
        }

        [Fact]
        public void Register_CallsSetRateResults_WhenOpenShippingDialogMessageReceived()
        {
            var viewModel = mock.CreateMock<RatingPanelViewModel>();
            var testObject = mock.Create<ShippingDialogOpeningPipeline>();
            testObject.Register(viewModel.Object);

            subject.OnNext(new ShippingDialogOpeningMessage(this));

            viewModel.Verify(x => x.SetRateResults(Enumerable.Empty<RateResult>(), string.Empty, Enumerable.Empty<object>()));
        }

        public void Dispose()
        {
            mock.Dispose();
            subject.Dispose();
        }
    }
}
