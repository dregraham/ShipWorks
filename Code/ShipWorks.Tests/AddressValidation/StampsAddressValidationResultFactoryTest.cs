using ShipWorks.AddressValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using Xunit;
using ShipWorks.AddressValidation.Enums;

namespace ShipWorks.Tests.AddressValidation
{
    public class StampsAddressValidationResultFactoryTest
    {
        StampsAddressValidationResultFactory testObject;

        public StampsAddressValidationResultFactoryTest()
        {
            testObject = new StampsAddressValidationResultFactory();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CreateAddressValidationResult_ReturnsAddressValidationResultWithIsValid(bool isValid)
        {
            Assert.Equal(isValid, testObject.CreateAddressValidationResult(new Address(), isValid, new UspsAddressValidationResults()).IsValid);
        }

        [Theory]
        [InlineData(true, ValidationDetailStatusType.Yes)]
        [InlineData(false, ValidationDetailStatusType.No)]
        [InlineData(null, ValidationDetailStatusType.Unknown)]
        public void CreateAddressValidationResult_ReturnsAddressValidationResultWithPOBox(bool? poBox, ValidationDetailStatusType expected)
        {
            Assert.Equal(expected, testObject.CreateAddressValidationResult(new Address(), true, new UspsAddressValidationResults() { IsPoBox = poBox }).POBox);
        }

        [Theory]
        [InlineData(ResidentialDeliveryIndicatorType.Yes, ValidationDetailStatusType.Yes)]
        [InlineData(ResidentialDeliveryIndicatorType.No, ValidationDetailStatusType.No)]
        [InlineData(ResidentialDeliveryIndicatorType.Unknown, ValidationDetailStatusType.Unknown)]
        [InlineData(ResidentialDeliveryIndicatorType.Unsupported, ValidationDetailStatusType.Unknown)]
        public void CreateAddressValidationResult_ReturnsAddressValidationResultWithResidentialStatus(ResidentialDeliveryIndicatorType status, ValidationDetailStatusType expected)
        {
            Assert.Equal(expected, testObject.CreateAddressValidationResult(new Address(), true, new UspsAddressValidationResults() { ResidentialIndicator = status }).ResidentialStatus);
        }

        [Theory]
        [InlineData("12345","56789","0000","12345")]
        [InlineData("", "56789", "0000", "56789")]
        [InlineData("", "56789", "1234", "56789-1234")]
        public void CreateAddressValidationResult_ReturnsAddressValidationResultWithPostalCode(string postalCode, string zipCode, string zipCodeAddOn, string expected)
        {
            var address = new Address()
            {
                PostalCode = postalCode,
                ZIPCode = zipCode,
                ZIPCodeAddOn = zipCodeAddOn
            };

            Assert.Equal(expected, testObject.CreateAddressValidationResult(address, true, new UspsAddressValidationResults()).PostalCode);
        }

        [Theory]
        [InlineData("MO", null, "US", "MO")]
        [InlineData(null, "MO", "US", "MO")]
        [InlineData("MO", "IL", "US", "MO")]
        [InlineData("MO", "IL", "GB", "")]
        [InlineData("MO", "IL", "DE", "")]
        [InlineData("MO", "IL", "gb", "")]
        [InlineData("MO", "IL", "de", "")]
        [InlineData("", "", "", "")]
        [InlineData(null, null, "", "")]
        public void CreateAddressValidationResult_ReturnsAddressValidationResultWithStateProvinceCode(string state, string province, string countryCode, string expected)
        {
            var address = new Address()
            {
                State = state,
                Province = province,
                Country = countryCode
            };

            Assert.Equal(expected, testObject.CreateAddressValidationResult(address, true, new UspsAddressValidationResults()).StateProvCode);
        }

        [Fact]
        public void CreateAddressValidationResult_ReturnsAddressValidationResultWithAddress()
        {
            var address = new Address()
            {
                Address1 = "16204 Bay Harbour Ct",
                Address2 = "123",
                Address3 = "456",
                City = "Wildwood"
            };

            Assert.Equal("16204 Bay Harbour Ct", testObject.CreateAddressValidationResult(address, true, new UspsAddressValidationResults()).Street1);
            Assert.Equal("123", testObject.CreateAddressValidationResult(address, true, new UspsAddressValidationResults()).Street2);
            Assert.Equal("456", testObject.CreateAddressValidationResult(address, true, new UspsAddressValidationResults()).Street3);
            Assert.Equal("Wildwood", testObject.CreateAddressValidationResult(address, true, new UspsAddressValidationResults()).City);
        }


        [Fact]
        public void CreateAddressValidationResult_ReturnsAddressValidationResultWithAddress_WhenAddressPartsAreNull()
        {
            var address = new Address()
            {
                Address1 = null,
                Address2 = null,
                Address3 = null,
                City = null
            };

            Assert.Equal(string.Empty, testObject.CreateAddressValidationResult(address, true, new UspsAddressValidationResults()).Street1);
            Assert.Equal(string.Empty, testObject.CreateAddressValidationResult(address, true, new UspsAddressValidationResults()).Street2);
            Assert.Equal(string.Empty, testObject.CreateAddressValidationResult(address, true, new UspsAddressValidationResults()).Street3);
            Assert.Equal(string.Empty, testObject.CreateAddressValidationResult(address, true, new UspsAddressValidationResults()).City);
        }
    }
}
