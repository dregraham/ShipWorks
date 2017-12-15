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
    public class AddressAdapterExtensionsTest
    {
        [Fact]
        public void UpdateValidationStatus_SetsAddressValidationStatusToBadAddress_WhenCountryIsBlank()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Pending;
            adapter.CountryCode = "";
            adapter.Street1 = "123 Main ST";

            adapter.UpdateValidationStatus();

            Assert.Equal((int)AddressValidationStatusType.BadAddress, adapter.AddressValidationStatus);
        }

        [Fact]
        public void UpdateValidationStatus_SetsAddressValidationStatusToBadAddress_WhenStreet1IsBlank()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Pending;
            adapter.CountryCode = "US";
            adapter.Street1 = "";

            adapter.UpdateValidationStatus();

            Assert.Equal((int)AddressValidationStatusType.BadAddress, adapter.AddressValidationStatus);
        }

        [Fact]
        public void UpdateValidationStatus_SetsAddressValidationStatusToNotChecked_WhenStreet1AndCountryAreNotBlank()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Pending;
            adapter.CountryCode = "US";
            adapter.Street1 = "123 Main St";

            adapter.UpdateValidationStatus();

            Assert.Equal((int)AddressValidationStatusType.NotChecked, adapter.AddressValidationStatus);
        }

        [Fact]
        public void UpdateValidationStatus_SetsAddressValidationStatusToPending_WhenAddressIsInternationalAndInternationalValidationIsTurnedOn()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Error;
            adapter.CountryCode = "CA";
            adapter.Street1 = "123 Main St";

            adapter.UpdateValidationStatus(new StoreEntity() { InternationalAddressValidationSetting = AddressValidationStoreSettingType.ValidateAndApply });

            Assert.Equal((int)AddressValidationStatusType.Pending, adapter.AddressValidationStatus);
        }

        [Fact]
        public void UpdateValidationStatus_SetsAddressValidationStatusToNotChecked_WhenAddressIsInternationalAndInternationalValidationIsTurnedOff()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Error;
            adapter.CountryCode = "CA";
            adapter.Street1 = "123 Main St";

            adapter.UpdateValidationStatus(new StoreEntity() { InternationalAddressValidationSetting = AddressValidationStoreSettingType.ValidationDisabled });

            Assert.Equal((int)AddressValidationStatusType.NotChecked, adapter.AddressValidationStatus);
        }

        [Fact]
        public void UpdateValidationStatus_SetsAddressValidationStatusToPending_WhenAddressIsDomesticAndInternationalValidationIsTurnedOn()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Error;
            adapter.CountryCode = "US";
            adapter.Street1 = "123 Main St";

            adapter.UpdateValidationStatus(new StoreEntity() { DomesticAddressValidationSetting = AddressValidationStoreSettingType.ValidateAndApply });

            Assert.Equal((int)AddressValidationStatusType.Pending, adapter.AddressValidationStatus);
        }

        [Fact]
        public void UpdateValidationStatus_SetsAddressValidationStatusToNotChecked_WhenAddressIsDomesticAndInternationalValidationIsTurnedOff()
        {
            AddressAdapter adapter = new AddressAdapter();
            adapter.AddressValidationStatus = (int)AddressValidationStatusType.Error;
            adapter.CountryCode = "US";
            adapter.Street1 = "123 Main St";

            adapter.UpdateValidationStatus(new StoreEntity() { DomesticAddressValidationSetting = AddressValidationStoreSettingType.ValidationDisabled });

            Assert.Equal((int)AddressValidationStatusType.NotChecked, adapter.AddressValidationStatus);
        }
    }
}
