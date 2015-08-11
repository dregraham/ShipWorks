using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business;
using Xunit;

namespace ShipWorks.Tests.Interapptive.Shared.Business
{
    public class GeographyTest
    {
        [Fact]
        public void GetCountryCode_ReturnsUS_WhenNameIsUnitedStates_Test()
        {
            Assert.Equal("US", Geography.GetCountryCode("United States"));
        }

        [Fact]
        public void GetCountryCode_ReturnsUS_WhenNameIsUnitedStatesInLowerCase_Test()
        {
            Assert.Equal("US", Geography.GetCountryCode("united states"));
        }

        [Fact]
        public void GetCountryCode_ReturnsUS_WhenNameIsUnitedStatesInUpperCase_Test()
        {
            Assert.Equal("US", Geography.GetCountryCode("UNITED STATES"));
        }

        [Fact]
        public void GetCountryCode_ReturnsUS_WhenNameIsUnitedStatesInMixedCase_Test()
        {
            Assert.Equal("US", Geography.GetCountryCode("UnItEd StAtEs"));
        }

        [Fact]
        public void GetCountryCode_ReturnsUS_WhenNameIsUSA_Test()
        {
            Assert.Equal("US", Geography.GetCountryCode("USA"));
        }

        [Fact]
        public void GetCountryCode_ReturnsUS_WhenNameIsUSAInLowerCase_Test()
        {
            Assert.Equal("US", Geography.GetCountryCode("usa"));
        }

        [Fact]
        public void GetCountryCode_ReturnsUS_WhenNameIsUSAInMixedCase_Test()
        {
            Assert.Equal("US", Geography.GetCountryCode("UsA"));
        }

        [Fact]
        public void GetCountryCode_ReturnsUS_WhenNameIsUnitedStatesOfAmerica_Test()
        {
            Assert.Equal("US", Geography.GetCountryCode("United States of America"));
        }

        [Fact]
        public void GetCountryCode_ReturnsUS_WhenNameIsUnitedStatesOfAmericaInLowerCase_Test()
        {
            Assert.Equal("US", Geography.GetCountryCode("united states of america"));
        }

        [Fact]
        public void GetCountryCode_ReturnsUS_WhenNameIsUnitedStatesOfAmericaInUpperCase_Test()
        {
            Assert.Equal("US", Geography.GetCountryCode("UNITED STATES OF AMERICA"));
        }

        [Fact]
        public void GetCountryCode_ReturnsUS_WhenNameIsUnitedStatesOfAmericaInMixedCase_Test()
        {
            Assert.Equal("US", Geography.GetCountryCode("uNiTeD StAtEs Of AmErIcA"));
        }

        [Fact]
        public void IsUSInternationalTerritory_ReturnsTrue_WhenCountryCodeIsInternationalTerritory()
        {
            AddressAdapter address = new AddressAdapter { CountryCode = "AS" };
            bool result = address.IsUSInternationalTerritory();
            Assert.True(result);
        }

        [Fact]
        public void IsUSInternationalTerritory_ReturnsTrue_WhenCountryCodeIsUSAndStateIsInternationalTerritory()
        {
            AddressAdapter address = new AddressAdapter { CountryCode = "US", StateProvCode = "AS"};
            bool result = address.IsUSInternationalTerritory();
            Assert.True(result);
        }

        [Fact]
        public void IsUSInternationalTerritory_ReturnsFalse_WhenCountryCodeIsUSAndStateIsMO()
        {
            AddressAdapter address = new AddressAdapter { CountryCode = "CA", StateProvCode = "MO" };
            bool result = address.IsUSInternationalTerritory();
            Assert.False(result);
        }

        [Fact]
        public void IsUSInternationalTerritory_ReturnsFalse_WhenCountryCodeIsCanadaAndStateIsInternationalTerritory()
        {
            AddressAdapter address = new AddressAdapter { CountryCode = "CA", StateProvCode = "AS" };
            bool result = address.IsUSInternationalTerritory();
            Assert.False(result);
        }

        [Fact]
        public void IsUSInternationalTerritory_ReturnsFalse_WhenCountryCodeIsCanadaAndStateIsON()
        {
            AddressAdapter address = new AddressAdapter { CountryCode = "CA", StateProvCode = "ON" };
            bool result = address.IsUSInternationalTerritory();
            Assert.False(result);
        }

        [Fact]
        public void GetStateProvCode_ReturnsEmptyString_WhenCountryCodeIsGreatBrittain()
        {
            string stateProvince = Geography.GetStateProvCode("something", "GB");
            Assert.True(string.IsNullOrEmpty(stateProvince));
        }

        [Fact]
        public void GetStateProvCode_ReturnsEmptyString_WhenStateProvinceIsEmptyAndCountryCodeIsGreatBrittain()
        {
            string stateProvince = Geography.GetStateProvCode(string.Empty, "GB");
            Assert.True(string.IsNullOrEmpty(stateProvince));
        }

        [Fact]
        public void GetStateProvCode_ReturnsPassedInStateProvince_WhenCountryCodeIsUnknown()
        {
            string expected = "something";
            string actual = Geography.GetStateProvCode(expected, "unknown country");
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetStateProvCode_ReturnsMO_WhenStateProvinceIsMissouriAndCountryCodeIsUs()
        {
            string expected = "MO";
            string actual = Geography.GetStateProvCode("Missouri", "US");
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetStateProvCode_ReturnsPassedInStateProvince_WhenCountryCodeIsNotPassedIn()
        {
            string expected = "something";
            string actual = Geography.GetStateProvCode(expected);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetStateProvCode_ReturnsEmptyString_WhenStateProvinceIsEmptyAndCountryCodeIsNotPassedIn()
        {
            string expected = string.Empty;
            string actual = Geography.GetStateProvCode(string.Empty);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetStateProvCode_ReturnsMO_WhenStateProvinceIsMissouriAndCountryCodeNotPassedIn()
        {
            string expected = "MO";
            string actual = Geography.GetStateProvCode("Missouri");
            Assert.Equal(expected, actual);
        }
    }
}
