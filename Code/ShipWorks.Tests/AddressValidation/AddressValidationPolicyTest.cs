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
        public void ShouldAutoValidate_WithAddressValidationStoreSettingType(AddressValidationStoreSettingType setting, bool expected)
        {
            Assert.Equal(expected, AddressValidationPolicy.ShouldAutoValidate(setting));
        }

        [Fact]
        public void ShouldAutoValidateWithStoreAndAdapter_ReturnsFalse_WhenStoreIsNull()
        {
            Assert.False(AddressValidationPolicy.ShouldAutoValidate(null, new AddressAdapter()));
        }

        [Fact]
        public void ShouldAutoValidateWithStoreAndAdapter_ReturnsFalse_WhenAdapterIsNull()
        {
            Assert.False(AddressValidationPolicy.ShouldAutoValidate(new StoreEntity(), null));
        }

        [Fact]
        public void ShouldAutoValidateWithStoreAndAdapter_ReturnsFalse_WhenAdapterAddressHasBeenValidated()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Fixed;

            Assert.False(AddressValidationPolicy.ShouldAutoValidate(new StoreEntity(), adapter));
        }

        [Fact]
        public void ShouldAutoValidateWithStoreAndAdapter_ReturnsFalse_WhenAdapterAddressIsInternationalAndInternationalValidationIsDisabled()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Pending;
            adapter.CountryCode = "CA";

            StoreEntity store = new StoreEntity();
            store.InternationalAddressValidationSetting = AddressValidationStoreSettingType.ValidationDisabled;

            Assert.False(AddressValidationPolicy.ShouldAutoValidate(store, adapter));
        }

        [Fact]
        public void ShouldAutoValidateWithStoreAndAdapter_ReturnsTrue_WhenAdapterAddressIsInternationalAndInternationalValidationIsEnabled()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Pending;
            adapter.CountryCode = "CA";

            StoreEntity store = new StoreEntity();
            store.InternationalAddressValidationSetting = AddressValidationStoreSettingType.ValidateAndApply;

            Assert.True(AddressValidationPolicy.ShouldAutoValidate(store, adapter));
        }

        [Fact]
        public void ShouldAutoValidateWithStoreAndAdapter_ReturnsFalse_WhenAdapterAddressIsInternationalAndInternationalValidationIsManualValidationOnly()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Pending;
            adapter.CountryCode = "CA";

            StoreEntity store = new StoreEntity();
            store.InternationalAddressValidationSetting = AddressValidationStoreSettingType.ManualValidationOnly;

            Assert.False(AddressValidationPolicy.ShouldAutoValidate(store, adapter));
        }

        [Fact]
        public void ShouldAutoValidateWithStoreAndAdapter_ReturnsFalse_WhenAdapterAddressIsDomesticAndDomesticValidationIsDisabled()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Pending;
            adapter.CountryCode = "US";

            StoreEntity store = new StoreEntity();
            store.DomesticAddressValidationSetting = AddressValidationStoreSettingType.ValidationDisabled;

            Assert.False(AddressValidationPolicy.ShouldAutoValidate(store, adapter));
        }

        [Fact]
        public void ShouldAutoValidateWithStoreAndAdapter_ReturnsTrue_WhenAdapterAddressIsDomesticAndDomesticValidationIsEnabled()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Pending;
            adapter.CountryCode = "US";

            StoreEntity store = new StoreEntity();
            store.DomesticAddressValidationSetting = AddressValidationStoreSettingType.ValidateAndApply;

            Assert.True(AddressValidationPolicy.ShouldAutoValidate(store, adapter));
        }
        
        [Fact]
        public void ShouldManuallyValidateWithStoreAndAdapter_ReturnsFalse_WhenStoreIsNull()
        {
            Assert.False(AddressValidationPolicy.ShouldManuallyValidate(null, new AddressAdapter()));
        }

        [Fact]
        public void ShouldManuallyValidateWithStoreAndAdapter_ReturnsFalse_WhenAdapterIsNull()
        {
            Assert.False(AddressValidationPolicy.ShouldManuallyValidate(new StoreEntity(), null));
        }

        [Fact]
        public void ShouldManuallyValidateWithStoreAndAdapter_ReturnsFalse_WhenAdapterAddressHasBeenValidated()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Fixed;

            Assert.False(AddressValidationPolicy.ShouldManuallyValidate(new StoreEntity(), adapter));
        }

        [Fact]
        public void ShouldManuallyValidateWithStoreAndAdapter_ReturnsFalse_WhenAdapterAddressIsInternationalAndInternationalValidationIsDisabled()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Pending;
            adapter.CountryCode = "CA";

            StoreEntity store = new StoreEntity();
            store.InternationalAddressValidationSetting = AddressValidationStoreSettingType.ValidationDisabled;

            Assert.False(AddressValidationPolicy.ShouldManuallyValidate(store, adapter));
        }

        [Fact]
        public void ShouldManuallyValidateWithStoreAndAdapter_ReturnsTrue_WhenAdapterAddressIsInternationalAndInternationalValidationIsEnabled()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Pending;
            adapter.CountryCode = "CA";

            StoreEntity store = new StoreEntity();
            store.InternationalAddressValidationSetting = AddressValidationStoreSettingType.ValidateAndApply;

            Assert.True(AddressValidationPolicy.ShouldManuallyValidate(store, adapter));
        }

        [Fact]
        public void ShouldManuallyValidateWithStoreAndAdapter_ReturnsFalse_WhenAdapterAddressIsDomesticAndDomesticValidationIsDisabled()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Pending;
            adapter.CountryCode = "US";

            StoreEntity store = new StoreEntity();
            store.DomesticAddressValidationSetting = AddressValidationStoreSettingType.ValidationDisabled;

            Assert.False(AddressValidationPolicy.ShouldManuallyValidate(store, adapter));
        }

        [Fact]
        public void ShouldManuallyValidateWithStoreAndAdapter_ReturnsTrue_WhenAdapterAddressIsDomesticAndDomesticValidationIsEnabled()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Pending;
            adapter.CountryCode = "US";

            StoreEntity store = new StoreEntity();
            store.DomesticAddressValidationSetting = AddressValidationStoreSettingType.ValidateAndApply;

            Assert.True(AddressValidationPolicy.ShouldManuallyValidate(store, adapter));
        }

        [Fact]
        public void ShouldManuallyValidateWithStoreAndAdapter_ReturnsTrue_WhenAdapterAddressIsDomesticAndDomesticValidationIsManualValidationOnly()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Pending;
            adapter.CountryCode = "US";

            StoreEntity store = new StoreEntity();
            store.DomesticAddressValidationSetting = AddressValidationStoreSettingType.ManualValidationOnly;

            Assert.True(AddressValidationPolicy.ShouldManuallyValidate(store, adapter));
        }
    }
}
