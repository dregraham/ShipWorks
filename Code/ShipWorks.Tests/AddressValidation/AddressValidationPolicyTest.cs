using Interapptive.Shared.Business;
using Interapptive.Shared.Enums;
using ShipWorks.AddressValidation;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Tests.AddressValidation
{
    public class AddressValidationPolicyTest
    {
        [Theory]
        [InlineData(AddressValidationStatusType.NotChecked, true)]
        [InlineData(AddressValidationStatusType.Pending, true)]
        [InlineData(AddressValidationStatusType.BadAddress, false)]
        [InlineData(AddressValidationStatusType.Valid, false)]
        [InlineData(AddressValidationStatusType.Fixed, false)]
        [InlineData(AddressValidationStatusType.SuggestionIgnored, false)]
        [InlineData(AddressValidationStatusType.HasSuggestions, false)]
        [InlineData(AddressValidationStatusType.SuggestionSelected, false)]
        [InlineData(AddressValidationStatusType.Error, true)]
        [InlineData(AddressValidationStatusType.WillNotValidate, false)]
        public void ShouldValidate_WithAddressValidationStatusType(AddressValidationStatusType status, bool expected)
        {
            Assert.Equal(expected, AddressValidationPolicy.ShouldValidate(status));
        }

        [Theory]
        [InlineData(AddressValidationStoreSettingType.ValidateAndApply, true)]
        [InlineData(AddressValidationStoreSettingType.ValidateAndNotify, true)]
        [InlineData(AddressValidationStoreSettingType.ManualValidationOnly, false)]
        [InlineData(AddressValidationStoreSettingType.ValidationDisabled, false)]
        public void ShouldValidate_WithAddressValidationStoreSettingType(AddressValidationStoreSettingType setting, bool expected)
        {
            Assert.Equal(expected, AddressValidationPolicy.ShouldValidate(setting));
        }

        [Fact]
        public void ShouldValidateWithStoreAndAdapter_ReturnsFalse_WhenStoreIsNull()
        {
            Assert.False(AddressValidationPolicy.ShouldValidate(null, new AddressAdapter()));
        }

        [Fact]
        public void ShouldValidateWithStoreAndAdapter_ReturnsFalse_WhenAdapterIsNull()
        {
            Assert.False(AddressValidationPolicy.ShouldValidate(new StoreEntity(), null));
        }

        [Fact]
        public void ShouldValidateWithStoreAndAdapter_ReturnsFalse_WhenAdapterAddressHasBeenValidated()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Fixed;

            Assert.False(AddressValidationPolicy.ShouldValidate(new StoreEntity(), adapter));
        }

        [Fact]
        public void ShouldValidateWithStoreAndAdapter_ReturnsFalse_WhenAdapterAddressIsInternationalAndInternationalValidationIsDisabled()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Pending;
            adapter.CountryCode = "CA";

            StoreEntity store = new StoreEntity();
            store.InternationalAddressValidationSetting = AddressValidationStoreSettingType.ValidationDisabled;

            Assert.False(AddressValidationPolicy.ShouldValidate(store, adapter));
        }

        [Fact]
        public void ShouldValidateWithStoreAndAdapter_ReturnsTrue_WhenAdapterAddressIsInternationalAndInternationalValidationIsEnabled()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Pending;
            adapter.CountryCode = "CA";

            StoreEntity store = new StoreEntity();
            store.InternationalAddressValidationSetting = AddressValidationStoreSettingType.ValidateAndApply;

            Assert.True(AddressValidationPolicy.ShouldValidate(store, adapter));
        }

        [Fact]
        public void ShouldValidateWithStoreAndAdapter_ReturnsFalse_WhenAdapterAddressIsDomesticAndDomesticValidationIsDisabled()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Pending;
            adapter.CountryCode = "US";

            StoreEntity store = new StoreEntity();
            store.DomesticAddressValidationSetting = AddressValidationStoreSettingType.ValidationDisabled;

            Assert.False(AddressValidationPolicy.ShouldValidate(store, adapter));
        }

        [Fact]
        public void ShouldValidateWithStoreAndAdapter_ReturnsTrue_WhenAdapterAddressIsDomesticAndDomesticValidationIsEnabled()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Pending;
            adapter.CountryCode = "US";

            StoreEntity store = new StoreEntity();
            store.DomesticAddressValidationSetting = AddressValidationStoreSettingType.ValidateAndApply;

            Assert.True(AddressValidationPolicy.ShouldValidate(store, adapter));
        }

        [Fact]
        public void ShouldValidateWithAddressAdapter_ReturnsFalse_WhenCountryCodeIsBlank()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Pending;
            adapter.CountryCode = "";
            adapter.Street1 = "123 Main ST";

            Assert.False(AddressValidationPolicy.ShouldValidate(adapter));
        }

        [Fact]
        public void ShouldValidateWithAddressAdapter_ReturnsFalse_WhenStreet1IsBlank()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Pending;
            adapter.CountryCode = "US";
            adapter.Street1 = "";

            Assert.False(AddressValidationPolicy.ShouldValidate(adapter));
        }

        [Fact]
        public void ShouldValidateWithAddressAdapter_ReturnsTrue_WhenStreet1AndCountryAreNotBlank()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Pending;
            adapter.CountryCode = "US";
            adapter.Street1 = "123 Main St";

            Assert.True(AddressValidationPolicy.ShouldValidate(adapter));
        }
    }
}
