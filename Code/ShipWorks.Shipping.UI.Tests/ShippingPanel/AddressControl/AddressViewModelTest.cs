using Autofac.Extras.Moq;
using Interapptive.Shared.Business;
using Moq;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.UI.ShippingPanel.AddressControl;
using ShipWorks.Tests.Shared;
using System;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.AddressControl
{
    public class AddressViewModelTest : IDisposable
    {
        AutoMock mock;

        public AddressViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void SetAddressFromOrigin_DelegatesToShippingOriginManager()
        {
            var testObject = mock.Create<AddressViewModel>();
            testObject.SetAddressFromOrigin(1, 2, 3, ShipmentTypeCode.Usps);

            mock.Mock<IShippingOriginManager>()
                .Verify(x => x.GetOriginAddress(1, 2, 3, ShipmentTypeCode.Usps));
        }

        [Fact]
        public void SetAddressFromOrigin_DoesNotUpdateAddress_WhenShippingOriginManagerReturnsNull()
        {
            mock.Mock<IShippingOriginManager>()
                .Setup(x => x.GetOriginAddress(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<ShipmentTypeCode>()))
                .Returns((PersonAdapter)null);

            var testObject = mock.Create<AddressViewModel>();
            testObject.City = "Foo";

            testObject.SetAddressFromOrigin(1, 2, 3, ShipmentTypeCode.Usps);

            Assert.Equal("Foo", testObject.City);
        }

        [Fact]
        public void SetAddressFromOrigin_UpdatesAddress_WhenShippingOriginManagerReturnsAddress()
        {
            mock.Mock<IShippingOriginManager>()
                .Setup(x => x.GetOriginAddress(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<ShipmentTypeCode>()))
                .Returns(new PersonAdapter { City = "Bar" });

            var testObject = mock.Create<AddressViewModel>();
            testObject.City = "Foo";

            testObject.SetAddressFromOrigin(1, 2, 3, ShipmentTypeCode.Usps);

            Assert.Equal("Bar", testObject.City);
        }

        [Fact]
        public void SelectAddressSuggestion_DelegatesToAddressSelector()
        {
            var testAddressSuggestion = new ValidatedAddressEntity();
            mock.Mock<IAddressSelector>()
                .Setup(x => x.SelectAddress(It.IsAny<AddressAdapter>(), testAddressSuggestion))
                .ReturnsAsync(new AddressAdapter())
                .Verifiable();

            var testObject = mock.Create<AddressViewModel>();
            testObject.SelectAddressSuggestionCommand.Execute(testAddressSuggestion);

            mock.Mock<IAddressSelector>().VerifyAll();
        }

        [Fact]
        public void SelectAddressSuggestion_SendsCurrentAddress()
        {
            AddressAdapter address = null;
            mock.Mock<IAddressSelector>()
                .Setup(x => x.SelectAddress(It.IsAny<AddressAdapter>(), It.IsAny<ValidatedAddressEntity>()))
                .ReturnsAsync(new AddressAdapter())
                .Callback((AddressAdapter a, ValidatedAddressEntity _) => address = a);

            var testObject = mock.Create<AddressViewModel>();
            testObject.Street = "1 Main";
            testObject.City = "Foo";
            testObject.StateProvCode = "Bar";
            testObject.CountryCode = "Baz";
            testObject.PostalCode = "12345";

            testObject.SelectAddressSuggestionCommand.Execute(new ValidatedAddressEntity());

            Assert.Equal("1 Main", address.Street1);
            Assert.Equal("Foo", address.City);
            Assert.Equal("Bar", address.StateProvCode);
            Assert.Equal("Baz", address.CountryCode);
            Assert.Equal("12345", address.PostalCode);
        }

        [Fact]
        public void SelectAddressSuggestion_UpdatesAddressDetails_FromReturnedAddress()
        {
            AddressAdapter address = new AddressAdapter
            {
                Street1 = "2 Main",
                City = "Foo2",
                StateProvCode = "Bar2",
                CountryCode = "Baz2",
                PostalCode = "22345",
                AddressValidationError = "Stuff",
                AddressValidationStatus = (int)AddressValidationStatusType.SuggestionIgnored
            };
            
            mock.Mock<IAddressSelector>()
                .Setup(x => x.SelectAddress(It.IsAny<AddressAdapter>(), It.IsAny<ValidatedAddressEntity>()))
                .ReturnsAsync(address);

            var testObject = mock.Create<AddressViewModel>();
            testObject.Street = "1 Main";
            testObject.City = "Foo";
            testObject.StateProvCode = "Bar";
            testObject.CountryCode = "Baz";
            testObject.PostalCode = "12345";

            testObject.SelectAddressSuggestionCommand.Execute(new ValidatedAddressEntity());

            Assert.Equal("2 Main", testObject.Street);
            Assert.Equal("Foo2", testObject.City);
            Assert.Equal("Bar2", testObject.StateProvCode);
            Assert.Equal("Baz2", testObject.CountryCode);
            Assert.Equal("22345", testObject.PostalCode);
            Assert.Equal("Stuff", testObject.ValidationMessage);
            Assert.Equal(AddressValidationStatusType.SuggestionIgnored, testObject.ValidationStatus);
        }

        [Fact]
        public void SelectAddressSuggestion_DoesNotModifyOtherDetails()
        {
            AddressAdapter address = new AddressAdapter
            {
                AddressValidationSuggestionCount = 0
            };

            mock.Mock<IAddressSelector>()
                .Setup(x => x.SelectAddress(It.IsAny<AddressAdapter>(), It.IsAny<ValidatedAddressEntity>()))
                .ReturnsAsync(address);

            var testObject = mock.Create<AddressViewModel>();
            testObject.FullName = "John Doe";
            testObject.Company = "Foo Company";
            testObject.Email = "bar@example.com";
            testObject.Phone = "314-555-1234";
            testObject.SuggestionCount = 3;

            testObject.SelectAddressSuggestionCommand.Execute(new ValidatedAddressEntity());

            Assert.Equal("John Doe", testObject.FullName);
            Assert.Equal("Foo Company", testObject.Company);
            Assert.Equal("bar@example.com", testObject.Email);
            Assert.Equal("314-555-1234", testObject.Phone);
            Assert.Equal(3, testObject.SuggestionCount);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
