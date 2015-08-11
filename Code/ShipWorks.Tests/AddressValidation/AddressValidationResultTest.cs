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

        [TestInitialize]
        public void Initialize()
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
            Assert.IsTrue(result.IsEqualTo(adapter));
        }

        [Fact]
        public void IsEqualTo_ReturnsFalse_WhenStreet1IsDifferent()
        {
            adapter.Street1 = adapter.Street1.ToUpper();
            Assert.IsFalse(result.IsEqualTo(adapter));
        }

        [Fact]
        public void IsEqualTo_ReturnsFalse_WhenStreet2IsDifferent()
        {
            adapter.Street2 = adapter.Street2.ToUpper();
            Assert.IsFalse(result.IsEqualTo(adapter));
        }

        [Fact]
        public void IsEqualTo_ReturnsFalse_WhenStreet3IsDifferent()
        {
            adapter.Street3 = adapter.Street3.ToUpper();
            Assert.IsFalse(result.IsEqualTo(adapter));
        }

        [Fact]
        public void IsEqualTo_ReturnsFalse_WhenCityIsDifferent()
        {
            adapter.City = adapter.City.ToUpper();
            Assert.IsFalse(result.IsEqualTo(adapter));
        }

        [Fact]
        public void IsEqualTo_ReturnsFalse_WhenStateProvCodeIsDifferent()
        {
            adapter.StateProvCode = adapter.StateProvCode.ToLower();
            Assert.IsFalse(result.IsEqualTo(adapter));
        }

        [Fact]
        public void IsEqualTo_ReturnsFalse_WhenCountryCodeIsDifferent()
        {
            adapter.CountryCode = adapter.CountryCode.ToLower();
            Assert.IsFalse(result.IsEqualTo(adapter));
        }

        [Fact]
        public void IsEqualTo_ReturnsFalse_WhenPostalCodeIsDifferent()
        {
            adapter.PostalCode = "23456";
            Assert.IsFalse(result.IsEqualTo(adapter));
        }

        [Fact]
        public void CopyTo_CopiesResultToAdapter()
        {
            AddressAdapter newAdapter = new AddressAdapter();
            result.CopyTo(newAdapter);
            Assert.AreEqual("Street1", newAdapter.Street1);
            Assert.AreEqual("Street2", newAdapter.Street2);
            Assert.AreEqual("Street3", newAdapter.Street3);
            Assert.AreEqual("City", newAdapter.City);
            Assert.AreEqual("MO", newAdapter.StateProvCode);
            Assert.AreEqual("US", newAdapter.CountryCode);
            Assert.AreEqual("12345", newAdapter.PostalCode);
        }
    }
}
