using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business;
using Xunit;
using ShipWorks.AddressValidation;

namespace ShipWorks.Tests.AddressValidation
{
    public class AddressValidationResultTest
    {
        private AddressValidationResult result;
        private AddressAdapter adapter;

        public AddressValidationResultTest()
        {
            result = new AddressValidationResult
            {
                Street1 = "Street1",
                Street2 = "Street2",
                Street3 = "Street3",
                City = "City",
                StateProvCode = "MO",
                CountryCode = "US",
                PostalCode = "12345"
            };

            adapter = new AddressAdapter
            {
                Street1 = "Street1",
                Street2 = "Street2",
                Street3 = "Street3",
                City = "City",
                StateProvCode = "MO",
                CountryCode = "US",
                PostalCode = "12345"
            };
        }

        [Fact]
        public void IsEqualTo_ReturnsTrue_WhenAdapterIsEqual()
        {
            Assert.True(result.IsEqualTo(adapter));
        }

        [Fact]
        public void IsEqualTo_ReturnsFalse_WhenStreet1IsDifferent()
        {
            adapter.Street1 = adapter.Street1.ToUpper();
            Assert.False(result.IsEqualTo(adapter));
        }

        [Fact]
        public void IsEqualTo_ReturnsFalse_WhenStreet2IsDifferent()
        {
            adapter.Street2 = adapter.Street2.ToUpper();
            Assert.False(result.IsEqualTo(adapter));
        }

        [Fact]
        public void IsEqualTo_ReturnsFalse_WhenStreet3IsDifferent()
        {
            adapter.Street3 = adapter.Street3.ToUpper();
            Assert.False(result.IsEqualTo(adapter));
        }

        [Fact]
        public void IsEqualTo_ReturnsFalse_WhenCityIsDifferent()
        {
            adapter.City = adapter.City.ToUpper();
            Assert.False(result.IsEqualTo(adapter));
        }

        [Fact]
        public void IsEqualTo_ReturnsFalse_WhenStateProvCodeIsDifferent()
        {
            adapter.StateProvCode = adapter.StateProvCode.ToLower();
            Assert.False(result.IsEqualTo(adapter));
        }

        [Fact]
        public void IsEqualTo_ReturnsFalse_WhenCountryCodeIsDifferent()
        {
            adapter.CountryCode = adapter.CountryCode.ToLower();
            Assert.False(result.IsEqualTo(adapter));
        }

        [Fact]
        public void IsEqualTo_ReturnsFalse_WhenPostalCodeIsDifferent()
        {
            adapter.PostalCode = "23456";
            Assert.False(result.IsEqualTo(adapter));
        }

        [Fact]
        public void CopyTo_CopiesResultToAdapter()
        {
            AddressAdapter newAdapter = new AddressAdapter();
            result.CopyTo(newAdapter);
            Assert.Equal("Street1", newAdapter.Street1);
            Assert.Equal("Street2", newAdapter.Street2);
            Assert.Equal("Street3", newAdapter.Street3);
            Assert.Equal("City", newAdapter.City);
            Assert.Equal("MO", newAdapter.StateProvCode);
            Assert.Equal("US", newAdapter.CountryCode);
            Assert.Equal("12345", newAdapter.PostalCode);
        }
    }
}
