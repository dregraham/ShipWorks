using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.ObservableRegistrations
{
    public class SaveAfterAddressValidationPipelineTest : IDisposable
    {
        readonly AutoMock mock;

        public SaveAfterAddressValidationPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Register_DelegatesToSaveToDatabase_WhenPropertyIsValidationStatus()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>();
            var testObject = mock.Create<SaveAfterAddressValidationPipeline>();
            testObject.Register(viewModel.Object);

            viewModel.Object.Destination.ValidationStatus = AddressValidationStatusType.HasSuggestions;

            viewModel.Verify(x => x.SaveToDatabase());
        }

        [Fact]
        public void Register_DoesNotDelegateToSaveToDatabase_WhenPropertyIsNotValidationStatus()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>();
            var testObject = mock.Create<SaveAfterAddressValidationPipeline>();
            testObject.Register(viewModel.Object);

            viewModel.Object.Destination.Company = "Foo";

            viewModel.Verify(x => x.SaveToDatabase(), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
