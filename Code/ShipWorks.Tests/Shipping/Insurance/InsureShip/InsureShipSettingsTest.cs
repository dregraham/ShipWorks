﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Insurance.InsureShip;

namespace ShipWorks.Tests.Shipping.Insurance.InsureShip
{
    [TestClass]
    public class InsureShipSettingsTest
    {
        InsureShipSettings testObject = new InsureShipSettings();

        [TestMethod]
        public void UseTestServer_SavesAsTrue()
        {
            testObject.UseTestServer = false;
            testObject.UseTestServer = true;

            Assert.IsTrue(testObject.UseTestServer);
        }

        [TestMethod]
        public void UseTestServer_SavesAsFalse()
        {
            testObject.UseTestServer = true;
            testObject.UseTestServer = false;

            Assert.IsFalse(testObject.UseTestServer);
        }

        [TestMethod]
        public void Username_IsCorrect_WhenUsingTestServer()
        {
            testObject.UseTestServer = true;

            Assert.AreEqual("test2", testObject.Username);
        }

        [TestMethod]
        public void Username_IsCorrect_WhenUsingProductionServer()
        {
            testObject.UseTestServer = false;

            Assert.AreEqual("shipworks", testObject.Username);
        }

        [TestMethod]
        public void Password_IsCorrect_WhenUsingTestServer()
        {
            testObject.UseTestServer = true;

            Assert.AreEqual("password", testObject.Password);
        }

        [TestMethod]
        public void Password_IsCorrect_WhenUsingProductionServer()
        {
            testObject.UseTestServer = false;

            Assert.AreEqual("624c55cb00f588f0fe1a79", testObject.Password);
        }

        [TestMethod]
        public void DistributorID_IsCorrect_WhenUsingTestServer()
        {
            testObject.UseTestServer = true;

            Assert.AreEqual("D00002", testObject.DistributorID);
        }

        [TestMethod]
        public void DistributorID_IsCorrect_WhenUsingProductionServer()
        {
            testObject.UseTestServer = false;

            Assert.AreEqual("D00050", testObject.DistributorID);
        }

        [TestMethod]
        public void Url_IsCorrect_WhenUsingTestServer()
        {
            testObject.UseTestServer = true;

            Assert.AreEqual("https://int.insureship.com/api/", testObject.Url.AbsoluteUri);
        }

        [TestMethod]
        public void Url_IsCorrect_WhenUsingProductionServer()
        {
            testObject.UseTestServer = false;

            Assert.AreEqual("https://api2.insureship.com/api/", testObject.Url.AbsoluteUri);
        }
    }
}
