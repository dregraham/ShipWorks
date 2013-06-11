﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Tests.Shipping
{
    [TestClass]
    public class CustomsManagerTest
    {
        private ShipmentEntity shipment;

        [TestInitialize]
        public void Initialize()
        {
            shipment = new ShipmentEntity
            {
                OriginCountryCode = "US",
                ShipCountryCode = "US",
                ShipPostalCode = "63102",
                ShipmentType = (int)ShipmentTypeCode.FedEx
            };
        }

        // These were written to regression test a private method that was changed in the CustomsManager to use the 
        // ShipmentType.IsDomestic method. To test this, you'll need make the IsCustomsRequiredByShipment method public.

        //[TestMethod]
        //public void IsCustomsRequiredByShipment_FromUnitedStatesToUnitedStates_Test()
        //{
        //    Assert.IsFalse(CustomsManager.IsCustomsRequiredByShipment(shipment));
        //}

        //[TestMethod]
        //public void IsCustomsRequiredByShipment_FromUnitedStatesToUSTerritory96910_Test()
        //{
        //    shipment.ShipPostalCode = "96910";
        //    Assert.IsTrue(CustomsManager.IsCustomsRequiredByShipment(shipment));
        //}

        //[TestMethod]
        //public void IsCustomsRequiredByShipment_FromUnitedStatesToUSTerritory96950_Test()
        //{
        //    shipment.ShipPostalCode = "96950";
        //    Assert.IsTrue(CustomsManager.IsCustomsRequiredByShipment(shipment));
        //}

        //[TestMethod]
        //public void IsCustomsRequiredByShipment_FromUnitedStatesToUSTerritory96960_Test()
        //{
        //    shipment.ShipPostalCode = "96960";
        //    Assert.IsTrue(CustomsManager.IsCustomsRequiredByShipment(shipment));
        //}

        //[TestMethod]
        //public void IsCustomsRequiredByShipment_FromUnitedStatesToUSTerritory96970_Test()
        //{
        //    shipment.ShipPostalCode = "96970";
        //    Assert.IsTrue(CustomsManager.IsCustomsRequiredByShipment(shipment));
        //}

        //[TestMethod]
        //public void IsCustomsRequiredByShipment_FromUnitedStatesToUSTerritory96799_Test()
        //{
        //    shipment.ShipPostalCode = "96799";
        //    Assert.IsTrue(CustomsManager.IsCustomsRequiredByShipment(shipment));
        //}

        //[TestMethod]
        //public void IsCustomsRequiredByShipment_FromUnitedStatesToMilitaryState_Test()
        //{
        //    shipment.ShipStateProvCode = "AA";
        //    Assert.IsTrue(CustomsManager.IsCustomsRequiredByShipment(shipment));
        //}

        //[TestMethod]
        //public void IsCustomsRequiredByShipment_FromUnitedStatesToMilitaryState_WhenUsingEndicia_Test()
        //{
        //    shipment.ShipStateProvCode = "AA";
        //    shipment.ShipmentType = (int) ShipmentTypeCode.Endicia;

        //    Assert.IsTrue(CustomsManager.IsCustomsRequiredByShipment(shipment));
        //}

        //[TestMethod]
        //public void IsCustomsRequiredByShipment_FromUnitedStatesToNonUSCountry_Test()
        //{
        //    shipment.ShipCountryCode = "CA";
        //    Assert.IsTrue(CustomsManager.IsCustomsRequiredByShipment(shipment));
        //}

        //[TestMethod]
        //public void IsCustomsRequiredByShipment_FromUnitedStatesToGuam_WithPostalShipment_Test()
        //{
        //    shipment.ShipCountryCode = "GU";
        //    shipment.ShipmentType = (int)ShipmentTypeCode.Endicia;

        //    Assert.IsTrue(CustomsManager.IsCustomsRequiredByShipment(shipment));
        //}

        //[TestMethod]
        //public void IsCustomsRequiredByShipment_FromUnitedStatesToUSDomesticCountryCode_WithPostalShipment_Test()
        //{
        //    // Ship to Puerto Rico
        //    shipment.ShipCountryCode = "PR";
        //    shipment.ShipmentType = (int)ShipmentTypeCode.Endicia;

        //    Assert.IsFalse(CustomsManager.IsCustomsRequiredByShipment(shipment));
        //}


        //[TestMethod]
        //public void IsCustomsRequiredByShipment_FromCanadaToUSDomesticCountryCode_Test()
        //{
        //    // Ship to Puerto Rico
        //    shipment.OriginCountryCode = "CA";
        //    shipment.ShipCountryCode = "PR";
        //    shipment.ShipmentType = (int)ShipmentTypeCode.FedEx;

        //    Assert.IsTrue(CustomsManager.IsCustomsRequiredByShipment(shipment));
        //}

        //[TestMethod]
        //public void IsCustomsRequiredByShipment_FromCanadaToCanada_Test()
        //{
        //    shipment.OriginCountryCode = "CA";
        //    shipment.ShipCountryCode = "CA";
        //    shipment.ShipmentType = (int)ShipmentTypeCode.FedEx;

        //    Assert.IsFalse(CustomsManager.IsCustomsRequiredByShipment(shipment));
        //}

        //[TestMethod]
        //public void IsCustomsRequiredByShipment_FromCanadaToCanada_WithPostalShipment_Test()
        //{
        //    shipment.OriginCountryCode = "CA";
        //    shipment.ShipCountryCode = "CA";
        //    shipment.ShipmentType = (int)ShipmentTypeCode.Endicia;

        //    Assert.IsFalse(CustomsManager.IsCustomsRequiredByShipment(shipment));
        //}

        //[TestMethod]
        //public void IsCustomsRequiredByShipment_FromCanadaToUSDomesticCountryCode_WithPostalShipment_Test()
        //{
        //    // Ship to Puerto Rico
        //    shipment.OriginCountryCode = "CA";
        //    shipment.ShipCountryCode = "PR";
        //    shipment.ShipmentType = (int)ShipmentTypeCode.Endicia;

        //    Assert.IsTrue(CustomsManager.IsCustomsRequiredByShipment(shipment));
        //}


        //[TestMethod]
        //public void IsCustomsRequiredByShipment_FromPuertoRicoToCanada_Test()
        //{
        //    // Ship from Puerto Rico
        //    shipment.OriginCountryCode = "PR";
        //    shipment.ShipCountryCode = "CA";
        //    shipment.ShipmentType = (int)ShipmentTypeCode.FedEx;

        //    Assert.IsTrue(CustomsManager.IsCustomsRequiredByShipment(shipment));
        //}

        //[TestMethod]
        //public void IsCustomsRequiredByShipment_FromUnitedStatesToCanada_Test()
        //{
        //    shipment.OriginCountryCode = "US";
        //    shipment.ShipCountryCode = "CA";
        //    shipment.ShipmentType = (int)ShipmentTypeCode.FedEx;

        //    Assert.IsTrue(CustomsManager.IsCustomsRequiredByShipment(shipment));
        //}

        //[TestMethod]
        //public void IsCustomsRequiredByShipment_FromMilitaryStateToCanada_Test()
        //{

        //    shipment.OriginCountryCode = "US";
        //    shipment.OriginStateProvCode = "AA";

        //    shipment.ShipCountryCode = "CA";
        //    shipment.ShipmentType = (int)ShipmentTypeCode.FedEx;

        //    Assert.IsTrue(CustomsManager.IsCustomsRequiredByShipment(shipment));
        //}
    }
}
