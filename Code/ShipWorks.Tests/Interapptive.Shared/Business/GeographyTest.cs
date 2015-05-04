using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShipWorks.Tests.Interapptive.Shared.Business
{
    [TestClass]
    public class GeographyTest
    {
        [TestMethod]
        public void GetCountryCode_ReturnsUS_WhenNameIsUnitedStates_Test()
        {
            Assert.AreEqual("US", Geography.GetCountryCode("United States"));
        }

        [TestMethod]
        public void GetCountryCode_ReturnsUS_WhenNameIsUnitedStatesInLowerCase_Test()
        {
            Assert.AreEqual("US", Geography.GetCountryCode("united states"));
        }

        [TestMethod]
        public void GetCountryCode_ReturnsUS_WhenNameIsUnitedStatesInUpperCase_Test()
        {
            Assert.AreEqual("US", Geography.GetCountryCode("UNITED STATES"));
        }

        [TestMethod]
        public void GetCountryCode_ReturnsUS_WhenNameIsUnitedStatesInMixedCase_Test()
        {
            Assert.AreEqual("US", Geography.GetCountryCode("UnItEd StAtEs"));
        }

        [TestMethod]
        public void GetCountryCode_ReturnsUS_WhenNameIsUSA_Test()
        {
            Assert.AreEqual("US", Geography.GetCountryCode("USA"));
        }

        [TestMethod]
        public void GetCountryCode_ReturnsUS_WhenNameIsUSAInLowerCase_Test()
        {
            Assert.AreEqual("US", Geography.GetCountryCode("usa"));
        }

        [TestMethod]
        public void GetCountryCode_ReturnsUS_WhenNameIsUSAInMixedCase_Test()
        {
            Assert.AreEqual("US", Geography.GetCountryCode("UsA"));
        }

        [TestMethod]
        public void GetCountryCode_ReturnsUS_WhenNameIsUnitedStatesOfAmerica_Test()
        {
            Assert.AreEqual("US", Geography.GetCountryCode("United States of America"));
        }

        [TestMethod]
        public void GetCountryCode_ReturnsUS_WhenNameIsUnitedStatesOfAmericaInLowerCase_Test()
        {
            Assert.AreEqual("US", Geography.GetCountryCode("united states of america"));
        }

        [TestMethod]
        public void GetCountryCode_ReturnsUS_WhenNameIsUnitedStatesOfAmericaInUpperCase_Test()
        {
            Assert.AreEqual("US", Geography.GetCountryCode("UNITED STATES OF AMERICA"));
        }

        [TestMethod]
        public void GetCountryCode_ReturnsUS_WhenNameIsUnitedStatesOfAmericaInMixedCase_Test()
        {
            Assert.AreEqual("US", Geography.GetCountryCode("uNiTeD StAtEs Of AmErIcA"));
        }

        [TestMethod]
        public void IsUSInternationalTerritory_ReturnsTrue_WhenCountryCodeIsInternationalTerritory()
        {
            AddressAdapter address = new AddressAdapter { CountryCode = "AS" };
            bool result = address.IsUSInternationalTerritory();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsUSInternationalTerritory_ReturnsTrue_WhenCountryCodeIsUSAndStateIsInternationalTerritory()
        {
            AddressAdapter address = new AddressAdapter { CountryCode = "US", StateProvCode = "AS"};
            bool result = address.IsUSInternationalTerritory();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsUSInternationalTerritory_ReturnsFalse_WhenCountryCodeIsUSAndStateIsMO()
        {
            AddressAdapter address = new AddressAdapter { CountryCode = "CA", StateProvCode = "MO" };
            bool result = address.IsUSInternationalTerritory();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsUSInternationalTerritory_ReturnsFalse_WhenCountryCodeIsCanadaAndStateIsInternationalTerritory()
        {
            AddressAdapter address = new AddressAdapter { CountryCode = "CA", StateProvCode = "AS" };
            bool result = address.IsUSInternationalTerritory();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsUSInternationalTerritory_ReturnsFalse_WhenCountryCodeIsCanadaAndStateIsON()
        {
            AddressAdapter address = new AddressAdapter { CountryCode = "CA", StateProvCode = "ON" };
            bool result = address.IsUSInternationalTerritory();
            Assert.IsFalse(result);
        }
    }
}
