using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.AddressValidation;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Tests.Shared;
using ShipWorks.UI.Controls.AddressControl;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.AddressControl
{
    public class AddressViewModelTest : IDisposable
    {
        readonly AutoMock mock;
        readonly PersonAdapter entityBasedAdapter = new PersonAdapter(new ShipmentEntity { ShipmentID = 3 }, "Ship");

        public AddressViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Load_PopulatesAllProperties()
        {
            PersonAdapter person = new PersonAdapter
            {
                Street1 = "1 Main",
                City = "Foo",
                StateProvCode = "Bar",
                CountryCode = "Baz",
                PostalCode = "12345",
                FirstName = "John",
                LastName = "Doe",
                Company = "Foo Company",
                Email = "bar@example.com",
                Phone = "314-555-1234"
            };

            var testObject = mock.Create<AddressViewModel>();
            testObject.Load(person);

            Assert.Equal(testObject.Street, "1 Main");
            Assert.Equal(testObject.City, "Foo");
            Assert.Equal(testObject.StateProvCode, "Bar");
            Assert.Equal(testObject.CountryCode, "Baz");
            Assert.Equal(testObject.PostalCode, "12345");
            Assert.Equal(testObject.FullName, "John Doe");
            Assert.Equal(testObject.Company, "Foo Company");
            Assert.Equal(testObject.Email, "bar@example.com");
            Assert.Equal(testObject.Phone, "314-555-1234");
        }

        [Fact]
        public void Load_DoesNotGetAddressSuggestions_WhenPersonIsNotFromEntity()
        {
            var testObject = mock.Create<AddressViewModel>();

            testObject.Load(new PersonAdapter());

            mock.Mock<IValidatedAddressScope>()
                .Verify(x => x.LoadValidatedAddresses(It.IsAny<long>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Load_DoesNotSetValidationDetails_WhenPersonIsNotFromEntity()
        {
            var testObject = mock.Create<AddressViewModel>();

            testObject.Load(new PersonAdapter
            {
                AddressValidationSuggestionCount = 3,
                AddressValidationStatus = (int) AddressValidationStatusType.HasSuggestions,
                AddressValidationError = "Foo bar"
            });

            Assert.NotEqual(testObject.SuggestionCount, 3);
            Assert.NotEqual(testObject.ValidationStatus, AddressValidationStatusType.HasSuggestions);
            Assert.NotEqual(testObject.ValidationMessage, "Foo bar");
        }

        [Fact]
        public void Load_GetsAddressSuggestions_WhenPersonIsFromEntity()
        {
            var shipment = new ShipmentEntity
            {
                ShipmentID = 12,
                ShipAddressValidationSuggestionCount = 3,
                ShipAddressValidationStatus = (int) AddressValidationStatusType.HasSuggestions,
                ShipAddressValidationError = "Foo bar"
            };
            var testObject = mock.Create<AddressViewModel>();

            testObject.Load(new PersonAdapter(shipment, "Ship"));

            Assert.Equal(testObject.SuggestionCount, 3);
            Assert.Equal(testObject.ValidationStatus, AddressValidationStatusType.HasSuggestions);
            Assert.Equal(testObject.ValidationMessage, "Foo bar");
        }

        [Fact]
        public void Load_FormatsAddressSuggestions_WhenPersonIsFromEntity()
        {
            ValidatedAddressEntity validatedAddress1 = new ValidatedAddressEntity();
            ValidatedAddressEntity validatedAddress2 = new ValidatedAddressEntity();

            mock.Mock<IValidatedAddressScope>()
                .Setup(x => x.LoadValidatedAddresses(It.IsAny<long>(), It.IsAny<string>()))
                .Returns(new[] { validatedAddress1, validatedAddress2 });
            mock.Mock<IAddressSelector>()
                .Setup(x => x.FormatAddress(validatedAddress1))
                .Returns("Foo");
            mock.Mock<IAddressSelector>()
                .Setup(x => x.FormatAddress(validatedAddress2))
                .Returns("Bar");

            var testObject = mock.Create<AddressViewModel>();

            testObject.Load(new PersonAdapter(new ShipmentEntity(), "Ship"));

            Assert.Contains(new KeyValuePair<string, ValidatedAddressEntity>("Foo", validatedAddress1), testObject.AddressSuggestions);
            Assert.Contains(new KeyValuePair<string, ValidatedAddressEntity>("Bar", validatedAddress2), testObject.AddressSuggestions);
        }

        [Fact]
        public void Load_SetsValidationDetails_WhenPersonIsFromEntity()
        {
            var shipment = new ShipmentEntity { ShipmentID = 12 };
            var testObject = mock.Create<AddressViewModel>();

            testObject.Load(new PersonAdapter(shipment, "Ship"));

            mock.Mock<IValidatedAddressScope>()
                .Verify(x => x.LoadValidatedAddresses(12, "Ship"));
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
                .Returns((PersonAdapter) null);

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

            mock.VerifyAll = true;
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
                AddressValidationStatus = (int) AddressValidationStatusType.SuggestionIgnored
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
            Assert.Equal(AddressValidationStatusType.SuggestionIgnored, testObject.ValidationStatus);
        }

        [Fact]
        public void SelectAddressSuggestion_DoesNotModifyOtherDetails()
        {
            AddressAdapter address = new AddressAdapter
            {
                AddressValidationSuggestionCount = 0,
                AddressValidationError = "Foo bar"
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
            testObject.ValidationMessage = "Stuff";

            testObject.SelectAddressSuggestionCommand.Execute(new ValidatedAddressEntity());

            Assert.Equal("John Doe", testObject.FullName);
            Assert.Equal("Foo Company", testObject.Company);
            Assert.Equal("bar@example.com", testObject.Email);
            Assert.Equal("314-555-1234", testObject.Phone);
            Assert.Equal("Stuff", testObject.ValidationMessage);
            Assert.Equal(3, testObject.SuggestionCount);
        }

        [Fact]
        public void ValidateCommand_DoesNotCallValidator_WhenLoadedWithNonEntityAddress()
        {
            var testObject = mock.Create<AddressViewModel>();
            testObject.Load(new PersonAdapter());

            testObject.ValidateCommand.Execute(null);

            mock.Mock<IAddressValidator>()
                .Verify(x => x.ValidateAsync(It.IsAny<AddressAdapter>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public void ValidateCommand_SendsCurrentValues_ToValidator()
        {
            AddressAdapter address = null;
            mock.Mock<IAddressValidator>()
                .Setup(x => x.ValidateAsync(It.IsAny<AddressAdapter>(), It.IsAny<bool>()))
                .ReturnsAsync(ValidatedAddressData.Empty)
                .Callback((AddressAdapter a, bool _) => address = a);

            var testObject = mock.Create<AddressViewModel>();
            testObject.Load(new PersonAdapter(new ShipmentEntity(), string.Empty));

            testObject.Street = "1 Main";
            testObject.City = "Foo";
            testObject.StateProvCode = "Bar";
            testObject.CountryCode = "Baz";
            testObject.PostalCode = "12345";

            testObject.ValidateCommand.Execute(null);

            Assert.Equal("1 Main", address.Street1);
            Assert.Equal("Foo", address.City);
            Assert.Equal("Bar", address.StateProvCode);
            Assert.Equal("Baz", address.CountryCode);
            Assert.Equal("12345", address.PostalCode);
        }

        [Fact]
        public void ValidateCommand_StoresReturnedAddresses_InValidatedAddressScope()
        {
            var original = new ValidatedAddressEntity();
            var address1 = new ValidatedAddressEntity();
            var data = new ValidatedAddressData(original, new[] { address1 });

            mock.Mock<IAddressSelector>()
                .Setup(x => x.FormatAddress(It.IsAny<ValidatedAddressEntity>()))
                .Returns((ValidatedAddressEntity x) => x.GetHashCode().ToString());
            mock.Mock<IAddressValidator>()
                .Setup(x => x.ValidateAsync(It.IsAny<AddressAdapter>(), It.IsAny<bool>()))
                .ReturnsAsync(data);

            var testObject = mock.Create<AddressViewModel>();
            testObject.Load(new PersonAdapter(new ShipmentEntity { ShipmentID = 3 }, "Ship"));

            testObject.ValidateCommand.Execute(null);

            mock.Mock<IValidatedAddressScope>()
                .Verify(x => x.StoreAddresses(3, data.AllAddresses, "Ship"));
        }

        [Fact]
        public void ValidateCommand_SetsValidationDetails_AfterValidation()
        {
            var data = new ValidatedAddressData(new ValidatedAddressEntity(),
                new[] { new ValidatedAddressEntity(), new ValidatedAddressEntity() });

            mock.Mock<IAddressSelector>()
                .Setup(x => x.FormatAddress(It.IsAny<ValidatedAddressEntity>()))
                .Returns((ValidatedAddressEntity x) => x.GetHashCode().ToString());
            mock.Mock<IAddressValidator>()
                .Setup(x => x.ValidateAsync(It.IsAny<AddressAdapter>(), It.IsAny<bool>()))
                .ReturnsAsync(data)
                .Callback((AddressAdapter a, bool _) =>
                {
                    a.AddressValidationError = "Foo";
                    a.AddressValidationSuggestionCount = 6;
                    a.AddressValidationStatus = (int) AddressValidationStatusType.Error;
                });

            var testObject = mock.Create<AddressViewModel>();
            testObject.Load(new PersonAdapter(new ShipmentEntity { ShipmentID = 3 }, "Ship"));

            testObject.ValidateCommand.Execute(null);

            Assert.Equal("Foo", testObject.ValidationMessage);
            Assert.Equal(6, testObject.SuggestionCount);
            Assert.Equal(AddressValidationStatusType.Error, testObject.ValidationStatus);
        }

        [Fact]
        public void ValidateCommand_SetsAddressDetails_AfterValidation()
        {
            var data = new ValidatedAddressData(new ValidatedAddressEntity(),
                new[] { new ValidatedAddressEntity(), new ValidatedAddressEntity() });

            mock.Mock<IAddressSelector>()
                .Setup(x => x.FormatAddress(It.IsAny<ValidatedAddressEntity>()))
                .Returns((ValidatedAddressEntity x) => x.GetHashCode().ToString());
            mock.Mock<IAddressValidator>()
                .Setup(x => x.ValidateAsync(It.IsAny<AddressAdapter>(), It.IsAny<bool>()))
                .ReturnsAsync(data)
                .Callback((AddressAdapter a, bool _) =>
                {
                    a.Street1 = "2 Main";
                    a.City = "Foo2";
                    a.StateProvCode = "Bar2";
                    a.CountryCode = "Baz2";
                    a.PostalCode = "22345";
                });

            var testObject = mock.Create<AddressViewModel>();
            testObject.Load(new PersonAdapter(new ShipmentEntity { ShipmentID = 3 }, "Ship"));
            testObject.Street = "1 Main";
            testObject.City = "Foo";
            testObject.StateProvCode = "Bar";
            testObject.CountryCode = "Baz";
            testObject.PostalCode = "12345";

            testObject.ValidateCommand.Execute(null);

            Assert.Equal("2 Main", testObject.Street);
            Assert.Equal("Foo2", testObject.City);
            Assert.Equal("Bar2", testObject.StateProvCode);
            Assert.Equal("Baz2", testObject.CountryCode);
            Assert.Equal("22345", testObject.PostalCode);
        }

        [Fact]
        public void ValidateCommand_DoesNotUpdateDetails_WhenLoadedAddressChangesDuringValidation()
        {
            AddressViewModel testObject = null;
            mock.Mock<IAddressSelector>()
                .Setup(x => x.FormatAddress(It.IsAny<ValidatedAddressEntity>()))
                .Returns((ValidatedAddressEntity x) => x.GetHashCode().ToString());
            mock.Mock<IAddressValidator>()
                .Setup(x => x.ValidateAsync(It.IsAny<AddressAdapter>(), It.IsAny<bool>()))
                .ReturnsAsync(ValidatedAddressData.Empty)
                .Callback((AddressAdapter a, bool _) =>
                {
                    a.Street1 = "2 Main";
                    a.City = "Foo2";
                    a.StateProvCode = "Bar2";
                    a.CountryCode = "Baz2";
                    a.PostalCode = "22345";

                    // Simulate another process loading a new address during validation
                    testObject.Load(new PersonAdapter(new ShipmentEntity { ShipmentID = 7 }, "Ship"));
                });

            testObject = mock.Create<AddressViewModel>();
            testObject.Load(new PersonAdapter(new ShipmentEntity { ShipmentID = 3 }, "Ship"));
            testObject.Street = "1 Main";
            testObject.City = "Foo";
            testObject.StateProvCode = "Bar";
            testObject.CountryCode = "Baz";
            testObject.PostalCode = "12345";

            testObject.ValidateCommand.Execute(null);

            Assert.NotEqual("2 Main", testObject.Street);
            Assert.NotEqual("Foo2", testObject.City);
            Assert.NotEqual("Bar2", testObject.StateProvCode);
            Assert.NotEqual("Baz2", testObject.CountryCode);
            Assert.NotEqual("22345", testObject.PostalCode);
        }

        [Fact]
        public void SetStreet_ResetsValidationStatus_WhenItIsAValidationProperty() =>
            VerifyPropertySetterResetsValidationStatus(x => x.Street = "Foo");

        [Fact]
        public void SetCountryCode_ResetsValidationStatus_WhenItIsAValidationProperty() =>
            VerifyPropertySetterResetsValidationStatus(x => x.CountryCode = "Foo");

        [Fact]
        public void SetPostalCode_ResetsValidationStatus_WhenItIsAValidationProperty() =>
            VerifyPropertySetterResetsValidationStatus(x => x.PostalCode = "Foo");

        [Fact]
        public void SetStateProvCode_ResetsValidationStatus_WhenItIsAValidationProperty() =>
            VerifyPropertySetterResetsValidationStatus(x => x.StateProvCode = "Foo");

        [Fact]
        public void SetCity_ResetsValidationStatus_WhenItIsAValidationProperty() =>
            VerifyPropertySetterResetsValidationStatus(x => x.City = "Foo");

        [Fact]
        public void SetName_DoesNotResetValidationStatus_WhenItIsNotAValidationProperty()
        {
            entityBasedAdapter.AddressValidationStatus = (int) AddressValidationStatusType.Fixed;

            AddressViewModel testObject = mock.Create<AddressViewModel>();
            testObject.IsAddressValidationEnabled = true;
            testObject.Load(entityBasedAdapter);

            testObject.FullName = "Foo Bar";

            Assert.Equal(AddressValidationStatusType.Fixed, testObject.ValidationStatus);
        }

        [Fact]
        public void Set_ClearsAddressSuggestions_WhenPropertyIsValidationProperty()
        {
            AddressViewModel testObject = mock.Create<AddressViewModel>();
            testObject.IsAddressValidationEnabled = true;
            testObject.Load(entityBasedAdapter);

            testObject.Street = "Foo";

            mock.Mock<IValidatedAddressScope>()
                .Verify(x => x.ClearAddresses(3, "Ship"));
        }

        [Fact]
        public void Set_DoesNotClearAddressSuggestions_WhenPropertyIsValidationPropertyButAddressIsNotEntityBacked()
        {
            AddressViewModel testObject = mock.Create<AddressViewModel>();
            testObject.IsAddressValidationEnabled = true;
            testObject.Load(new PersonAdapter());

            testObject.Street = "Foo";

            mock.Mock<IValidatedAddressScope>()
                .Verify(x => x.ClearAddresses(It.IsAny<long>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void ShowValidationMessage_CallsShowInformation_WithValidationMessage()
        {
            entityBasedAdapter.AddressValidationError = "Foo Bar";

            AddressViewModel testObject = mock.Create<AddressViewModel>();
            testObject.IsAddressValidationEnabled = true;
            testObject.Load(entityBasedAdapter);

            testObject.ShowValidationMessageCommand.Execute(null);

            mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowInformation("Foo Bar"));
        }

        public void VerifyPropertySetterResetsValidationStatus(Action<AddressViewModel> setAction)
        {
            entityBasedAdapter.AddressValidationStatus = (int) AddressValidationStatusType.Fixed;

            AddressViewModel testObject = mock.Create<AddressViewModel>();
            testObject.IsAddressValidationEnabled = true;
            testObject.Load(entityBasedAdapter);

            setAction(testObject);

            Assert.Equal(AddressValidationStatusType.NotChecked, testObject.ValidationStatus);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
