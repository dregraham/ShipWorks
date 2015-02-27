using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Common.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.XmlDiffPatch;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Policies;

namespace ShipWorks.Tests.Editions
{
    [TestClass]
    public class EditionManagerTest
    {
        private List<EditionRestriction> restrictions;
        private List<StoreEntity> stores;
 
        private EditionRestriction endiciaRegistrationRestricted1;
        private EditionRestriction endiciaRegistrationRestricted2;

        private StoreEntity enabledStore1;
        private StoreEntity enabledStore2;
        private StoreEntity disabledStore1;
        private StoreEntity disabledStore2;
        private StoreEntity trialStore;

        [TestInitialize]
        public void Initialize()
        {
            enabledStore1 = new StoreEntity() { Enabled = true };
            enabledStore2 = new StoreEntity() { Enabled = true };

            disabledStore1 = new StoreEntity() { Enabled = false };
            disabledStore2 = new StoreEntity() { Enabled = false };

            trialStore = new StoreEntity() { Enabled = true };

            endiciaRegistrationRestricted1 = new EditionRestriction(new Edition(enabledStore1), EditionFeature.ShipmentTypeRegistration, ShipmentTypeCode.Endicia, EditionRestrictionLevel.Hidden);
            endiciaRegistrationRestricted2 = new EditionRestriction(new Edition(enabledStore2), EditionFeature.ShipmentTypeRegistration, ShipmentTypeCode.Endicia, EditionRestrictionLevel.Hidden);
        }

        [TestMethod]
        public void NoEndiciaRegistrationRestriction_WhenTrialStoreIsOnlyRestriction_Test()
        {
            endiciaRegistrationRestricted1 = new EditionRestriction(new Edition(trialStore), EditionFeature.ShipmentTypeRegistration, ShipmentTypeCode.Endicia, EditionRestrictionLevel.Hidden);
            restrictions = new List<EditionRestriction>()
            {
                endiciaRegistrationRestricted1
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2,
                disabledStore1,
                disabledStore2,
                trialStore
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveShipmentTypeRegistrationIfNeeded(EditionFeature.ShipmentTypeRegistration, ShipmentTypeCode.Endicia, restrictions, stores);

            Assert.IsFalse(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.ShipmentTypeRegistration && (ShipmentTypeCode)r.Data == ShipmentTypeCode.Endicia));
        }

        [TestMethod]
        public void NoEndiciaRegistrationRestriction_WhenHalfOfTheStoresAreRestricted_Test()
        {
            restrictions = new List<EditionRestriction>()
            {
                endiciaRegistrationRestricted1
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2,
                disabledStore1,
                disabledStore2
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveShipmentTypeRegistrationIfNeeded(EditionFeature.ShipmentTypeRegistration, ShipmentTypeCode.Endicia, restrictions, stores);

            Assert.IsFalse(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.ShipmentTypeRegistration && (ShipmentTypeCode)r.Data == ShipmentTypeCode.Endicia));
        }

        [TestMethod]
        public void EndiciaRegistrationRestriction_WhenTrialStoreIsTheOnlyStore_Test()
        {
            endiciaRegistrationRestricted1 = new EditionRestriction(new Edition(trialStore), EditionFeature.ShipmentTypeRegistration, ShipmentTypeCode.Endicia, EditionRestrictionLevel.Hidden);
            restrictions = new List<EditionRestriction>()
            {
                endiciaRegistrationRestricted1
            };

            stores = new List<StoreEntity>()
            {
                trialStore
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveShipmentTypeRegistrationIfNeeded(EditionFeature.ShipmentTypeRegistration, ShipmentTypeCode.Endicia, restrictions, stores);

            Assert.IsTrue(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.ShipmentTypeRegistration && (ShipmentTypeCode)r.Data == ShipmentTypeCode.Endicia));
        }

        [TestMethod]
        public void EndiciaRegistrationRestriction_WhenAllStoresRestricted_Test()
        {
            restrictions = new List<EditionRestriction>()
            {
                endiciaRegistrationRestricted1,
                endiciaRegistrationRestricted2
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveShipmentTypeRegistrationIfNeeded(EditionFeature.ShipmentTypeRegistration, ShipmentTypeCode.Endicia, restrictions, stores);

            Assert.IsTrue(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.ShipmentTypeRegistration && (ShipmentTypeCode)r.Data == ShipmentTypeCode.Endicia));
        }

        [TestMethod]
        public void EndiciaRegistrationRestriction_WhenEnabledStoresAreRestricted_AndDisabledStoresAreNot_Test()
        {
            restrictions = new List<EditionRestriction>()
            {
                endiciaRegistrationRestricted1,
                endiciaRegistrationRestricted2
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2,
                disabledStore1,
                disabledStore2
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveShipmentTypeRegistrationIfNeeded(EditionFeature.ShipmentTypeRegistration, ShipmentTypeCode.Endicia, restrictions, stores);

            Assert.IsTrue(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.ShipmentTypeRegistration && (ShipmentTypeCode)r.Data == ShipmentTypeCode.Endicia));
        }


    }
}
