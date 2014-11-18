﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShipWorks.Tests.Interapptive.Shared.Business
{
    [TestClass]
    public class PersonUtilityTests
    {
        [TestMethod]
        public void GetPhoneDigits10_RemovesLeading1_Test()
        {
            string result = PersonUtility.GetPhoneDigits10("13145551212");
            Assert.AreEqual("3145551212", result);
            Assert.AreEqual(10, result.Length);
        }

        [TestMethod]
        public void GetPhoneDigits10_LeavesLeading0_Test()
        {
            string result = PersonUtility.GetPhoneDigits10("03145551212");
            Assert.AreEqual("0314555121", result);
            Assert.AreEqual(10, result.Length);
        }

        [TestMethod]
        public void GetPhoneDigits10_ReturnsFullValue_WhenLengthLessThan10_Test()
        {
            string result = PersonUtility.GetPhoneDigits10("1551212");
            Assert.AreEqual("1551212", result);
            Assert.AreEqual(7, result.Length);
        }

        [TestMethod]
        public void GetPhoneDigits10_ReturnsOnlyDigits_WhenNonAlphasInTest_Test()
        {
            string result = PersonUtility.GetPhoneDigits10("!1~5#5$1%2^&1*2())");
            Assert.AreEqual("1551212", result);
            Assert.AreEqual(7, result.Length);
        }

        [TestMethod]
        public void GetPhoneDigits10_ConvertsKeypadAlphasToDigits_Test()
        {
            string result = PersonUtility.GetPhoneDigits10("1jk1a1a");
            Assert.AreEqual("1551212", result);
            Assert.AreEqual(7, result.Length);
        }

        [TestMethod]
        public void GetPhoneDigits_RemovesLeading1_Test()
        {
            string result = PersonUtility.GetPhoneDigits("13145551212", 10, false);
            Assert.AreEqual("3145551212", result);
            Assert.AreEqual(10, result.Length);
        }

        [TestMethod]
        public void GetPhoneDigits_Returns15Characters_WhenOver15Requested_Test()
        {
            string result = PersonUtility.GetPhoneDigits("01234567890123456789", 15, false);
            Assert.AreEqual("012345678901234", result);
            Assert.AreEqual(15, result.Length);
        }

    }
}
