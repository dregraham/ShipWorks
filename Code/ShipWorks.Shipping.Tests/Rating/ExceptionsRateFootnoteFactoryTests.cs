using System;
using Autofac.Extras.Moq;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Rating
{
    public class ExceptionsRateFootnoteFactoryTests : IDisposable
    {
        readonly AutoMock mock;

        public ExceptionsRateFootnoteFactoryTests()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            mock.Mock<IExceptionsRateFootnoteViewModel>().SetupAllProperties();
        }

        [Fact]
        public void CreateViewModel_SetsDetailedInformation_ToExceptionMessage()
        {
            var testObject = new ExceptionsRateFootnoteFactory(ShipmentTypeCode.Other, new Exception("Foo"));
            var viewModel = testObject.CreateViewModel(null, mock.Container) as IExceptionsRateFootnoteViewModel;

            Assert.Equal("Foo", viewModel.DetailedMessage);
        }

        [Fact]
        public void CreateViewModel_SetsErrorMessageToRateError_WhenExceptionIsNotPackageException()
        {
            var testObject = new ExceptionsRateFootnoteFactory(ShipmentTypeCode.Other, new Exception("Foo"));
            var viewModel = testObject.CreateViewModel(null, mock.Container) as IExceptionsRateFootnoteViewModel;

            Assert.Contains("errors occurred", viewModel.ErrorText);
        }

        [Fact]
        public void CreateViewModel_SetsErrorMessageToPackageError_WhenExceptionIsPackageException()
        {
            var testObject = new ExceptionsRateFootnoteFactory(ShipmentTypeCode.Other, new InvalidPackageDimensionsException("Foo"));
            var viewModel = testObject.CreateViewModel(null, mock.Container) as IExceptionsRateFootnoteViewModel;

            Assert.Contains("package", viewModel.ErrorText);
        }

        [Fact]
        public void CreateViewModel_SetsErrorMessageToPackageError_WhenDeeplyNestedExceptionIsPackageException()
        {
            Exception ex = new Exception("Foo", new InvalidPackageDimensionsException());

            // This is an absurdly deep nest, but it will ensure a proper search of nested exceptions
            for (int i = 0; i < 100; i++)
            {
                ex = new Exception($"Level {i}", ex);
            }

            var testObject = new ExceptionsRateFootnoteFactory(ShipmentTypeCode.Other, ex);
            var viewModel = testObject.CreateViewModel(null, mock.Container) as IExceptionsRateFootnoteViewModel;

            Assert.Contains("package", viewModel.ErrorText);
        }

        [Fact]
        public void CreateFootnote_ReturnsExceptionsRateFootnoteControl_WhenExceptionIsNotPackageException()
        {
            var testObject = new ExceptionsRateFootnoteFactory(ShipmentTypeCode.Other, new Exception("Foo"));
            var control = testObject.CreateFootnote(null);

            Assert.IsAssignableFrom<ExceptionsRateFootnoteControl>(control);
        }

        [Fact]
        public void CreateFootnote_ReturnsInvalidPackageDimensionsRateFootnoteControl_WhenExceptionIsPackageException()
        {
            var testObject = new ExceptionsRateFootnoteFactory(ShipmentTypeCode.Other, new InvalidPackageDimensionsException("Foo"));
            var control = testObject.CreateFootnote(null);

            Assert.IsAssignableFrom<InvalidPackageDimensionsRateFootnoteControl>(control);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
