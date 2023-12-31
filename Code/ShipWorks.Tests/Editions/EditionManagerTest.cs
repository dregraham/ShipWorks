﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Common.Logging.Configuration;
using Xunit;
using Microsoft.XmlDiffPatch;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Policies;

namespace ShipWorks.Tests.Editions
{
    public class EditionManagerTest
    {
        private List<EditionRestriction> restrictions;
        private List<StoreEntity> stores;
 
        private EditionRestriction endiciaRegistrationRestricted1;
        private EditionRestriction endiciaRegistrationRestricted2;

        private EditionRestriction stampsAscendiaRestricted1;
        private EditionRestriction stampsAscendiaRestricted2;
        private EditionRestriction stampsDhlRestricted1;
        private EditionRestriction stampsDhlRestricted2;
        private EditionRestriction stampsGlobegisticsRestricted1;
        private EditionRestriction stampsGlobegisticsRestricted2;
        private EditionRestriction stampsIbcRestricted1;
        private EditionRestriction stampsIbcRestricted2;
        private EditionRestriction stampsRrDonnelleyRestricted1;
        private EditionRestriction stampsRrDonnelleyRestricted2;
        private EditionRestriction dhlEcommerceDhlMaxRestricted1;
        private EditionRestriction dhlEcommerceDhlMaxRestricted2;

        private StoreEntity enabledStore1;
        private StoreEntity enabledStore2;
        private StoreEntity disabledStore1;
        private StoreEntity disabledStore2;
        private StoreEntity trialStore;

        public EditionManagerTest()
        {
            // Make sure we're starting with a fresh cache each time
            ShippingPolicies.ClearCache();

            enabledStore1 = new StoreEntity() { Enabled = true };
            enabledStore2 = new StoreEntity() { Enabled = true };

            disabledStore1 = new StoreEntity() { Enabled = false };
            disabledStore2 = new StoreEntity() { Enabled = false };

            trialStore = new StoreEntity() { Enabled = true };

            endiciaRegistrationRestricted1 = new EditionRestriction(new Edition(enabledStore1), EditionFeature.ShipmentTypeRegistration, ShipmentTypeCode.Endicia, EditionRestrictionLevel.Hidden);
            endiciaRegistrationRestricted2 = new EditionRestriction(new Edition(enabledStore2), EditionFeature.ShipmentTypeRegistration, ShipmentTypeCode.Endicia, EditionRestrictionLevel.Hidden);

            stampsAscendiaRestricted1 = new EditionRestriction(new Edition(enabledStore1), EditionFeature.StampsAscendiaConsolidator, EditionRestrictionLevel.Hidden);
            stampsAscendiaRestricted2 = new EditionRestriction(new Edition(enabledStore2), EditionFeature.StampsAscendiaConsolidator, EditionRestrictionLevel.Hidden);
            stampsDhlRestricted1 = new EditionRestriction(new Edition(enabledStore1), EditionFeature.StampsDhlConsolidator, EditionRestrictionLevel.Hidden);
            stampsDhlRestricted2 = new EditionRestriction(new Edition(enabledStore2), EditionFeature.StampsDhlConsolidator, EditionRestrictionLevel.Hidden);
            stampsGlobegisticsRestricted1 = new EditionRestriction(new Edition(enabledStore1), EditionFeature.StampsGlobegisticsConsolidator, EditionRestrictionLevel.Hidden);
            stampsGlobegisticsRestricted2 = new EditionRestriction(new Edition(enabledStore2), EditionFeature.StampsGlobegisticsConsolidator, EditionRestrictionLevel.Hidden);
            stampsIbcRestricted1 = new EditionRestriction(new Edition(enabledStore1), EditionFeature.StampsIbcConsolidator, EditionRestrictionLevel.Hidden);
            stampsIbcRestricted2 = new EditionRestriction(new Edition(enabledStore2), EditionFeature.StampsIbcConsolidator, EditionRestrictionLevel.Hidden);
            stampsRrDonnelleyRestricted1 = new EditionRestriction(new Edition(enabledStore1), EditionFeature.StampsRrDonnelleyConsolidator, EditionRestrictionLevel.Hidden);
            stampsRrDonnelleyRestricted2 = new EditionRestriction(new Edition(enabledStore2), EditionFeature.StampsRrDonnelleyConsolidator, EditionRestrictionLevel.Hidden);

            dhlEcommerceDhlMaxRestricted1 = new EditionRestriction(new Edition(enabledStore1), EditionFeature.DhlEcommerceMax, EditionRestrictionLevel.Hidden);
            dhlEcommerceDhlMaxRestricted2 = new EditionRestriction(new Edition(enabledStore2), EditionFeature.DhlEcommerceMax, EditionRestrictionLevel.Hidden);
        }

        [Fact]
        public void NoEndiciaRegistrationRestriction_WhenTrialStoreIsOnlyRestriction()
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

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.ShipmentTypeRegistration, ShipmentTypeCode.Endicia, restrictions, stores);

            Assert.False(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.ShipmentTypeRegistration && (ShipmentTypeCode)r.Data == ShipmentTypeCode.Endicia));
        }

        [Fact]
        public void NoEndiciaRegistrationRestriction_WhenHalfOfTheStoresAreRestricted()
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

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.ShipmentTypeRegistration, ShipmentTypeCode.Endicia, restrictions, stores);

            Assert.False(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.ShipmentTypeRegistration && (ShipmentTypeCode)r.Data == ShipmentTypeCode.Endicia));
        }

        [Fact]
        public void EndiciaRegistrationRestriction_WhenTrialStoreIsTheOnlyStore()
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

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.ShipmentTypeRegistration, ShipmentTypeCode.Endicia, restrictions, stores);

            Assert.True(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.ShipmentTypeRegistration && (ShipmentTypeCode)r.Data == ShipmentTypeCode.Endicia));
        }

        [Fact]
        public void EndiciaRegistrationRestriction_WhenAllStoresRestricted()
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

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.ShipmentTypeRegistration, ShipmentTypeCode.Endicia, restrictions, stores);

            Assert.True(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.ShipmentTypeRegistration && (ShipmentTypeCode)r.Data == ShipmentTypeCode.Endicia));
        }

        [Fact]
        public void EndiciaRegistrationRestriction_WhenEnabledStoresAreRestricted_AndDisabledStoresAreNot()
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

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.ShipmentTypeRegistration, ShipmentTypeCode.Endicia, restrictions, stores);

            Assert.True(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.ShipmentTypeRegistration && (ShipmentTypeCode)r.Data == ShipmentTypeCode.Endicia));
        }

        [Fact]
        public void NoStampsAscendiaRestriction_WhenTrialStoreIsOnlyRestriction()
        {
            stampsAscendiaRestricted1 = new EditionRestriction(new Edition(trialStore), EditionFeature.StampsAscendiaConsolidator, EditionRestrictionLevel.Hidden);
            restrictions = new List<EditionRestriction>()
            {
                stampsAscendiaRestricted1
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2,
                disabledStore1,
                disabledStore2,
                trialStore
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.StampsAscendiaConsolidator, null, restrictions, stores);

            Assert.False(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.StampsAscendiaConsolidator));
        }

        [Fact]
        public void NoStampsAscendiaRestriction_WhenHalfOfTheStoresAreRestricted()
        {
            restrictions = new List<EditionRestriction>()
            {
                stampsAscendiaRestricted1
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2,
                disabledStore1,
                disabledStore2
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.StampsAscendiaConsolidator, null, restrictions, stores);

            Assert.False(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.StampsAscendiaConsolidator));
        }

        [Fact]
        public void StampsAscendiaRestriction_WhenTrialStoreIsTheOnlyStore()
        {
            stampsAscendiaRestricted1 = new EditionRestriction(new Edition(trialStore), EditionFeature.StampsAscendiaConsolidator, null, EditionRestrictionLevel.Hidden);
            restrictions = new List<EditionRestriction>()
            {
                stampsAscendiaRestricted1
            };

            stores = new List<StoreEntity>()
            {
                trialStore
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.StampsAscendiaConsolidator, null, restrictions, stores);

            Assert.True(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.StampsAscendiaConsolidator));
        }

        [Fact]
        public void StampsAscendiaRestriction_WhenAllStoresRestricted()
        {
            restrictions = new List<EditionRestriction>()
            {
                stampsAscendiaRestricted1,
                stampsAscendiaRestricted2
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.StampsAscendiaConsolidator, null, restrictions, stores);

            Assert.True(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.StampsAscendiaConsolidator));
        }

        [Fact]
        public void StampsAscendiaRestriction_WhenEnabledStoresAreRestricted_AndDisabledStoresAreNot()
        {
            restrictions = new List<EditionRestriction>()
            {
                stampsAscendiaRestricted1,
                stampsAscendiaRestricted2
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2,
                disabledStore1,
                disabledStore2
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.StampsAscendiaConsolidator, null, restrictions, stores);

            Assert.True(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.StampsAscendiaConsolidator));
        }

        [Fact]
        public void NoStampsDhlRestriction_WhenTrialStoreIsOnlyRestriction()
        {
            stampsDhlRestricted1 = new EditionRestriction(new Edition(trialStore), EditionFeature.StampsDhlConsolidator, EditionRestrictionLevel.Hidden);
            restrictions = new List<EditionRestriction>()
            {
                stampsDhlRestricted1
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2,
                disabledStore1,
                disabledStore2,
                trialStore
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.StampsDhlConsolidator, null, restrictions, stores);

            Assert.False(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.StampsDhlConsolidator));
        }

        [Fact]
        public void NoStampsDhlRestriction_WhenHalfOfTheStoresAreRestricted()
        {
            restrictions = new List<EditionRestriction>()
            {
                stampsDhlRestricted1
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2,
                disabledStore1,
                disabledStore2
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.StampsDhlConsolidator, null, restrictions, stores);

            Assert.False(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.StampsDhlConsolidator));
        }

        [Fact]
        public void StampsDhlRestriction_WhenTrialStoreIsTheOnlyStore()
        {
            stampsDhlRestricted1 = new EditionRestriction(new Edition(trialStore), EditionFeature.StampsDhlConsolidator, null, EditionRestrictionLevel.Hidden);
            restrictions = new List<EditionRestriction>()
            {
                stampsDhlRestricted1
            };

            stores = new List<StoreEntity>()
            {
                trialStore
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.StampsDhlConsolidator, null, restrictions, stores);

            Assert.True(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.StampsDhlConsolidator));
        }

        [Fact]
        public void StampsDhlRestriction_WhenAllStoresRestricted()
        {
            restrictions = new List<EditionRestriction>()
            {
                stampsDhlRestricted1,
                stampsDhlRestricted2
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.StampsDhlConsolidator, null, restrictions, stores);

            Assert.True(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.StampsDhlConsolidator));
        }

        [Fact]
        public void StampsDhlRestriction_WhenEnabledStoresAreRestricted_AndDisabledStoresAreNot()
        {
            restrictions = new List<EditionRestriction>()
            {
                stampsDhlRestricted1,
                stampsDhlRestricted2
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2,
                disabledStore1,
                disabledStore2
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.StampsDhlConsolidator, null, restrictions, stores);

            Assert.True(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.StampsDhlConsolidator));
        }

        [Fact]
        public void NoStampsGlobegisticsRestriction_WhenTrialStoreIsOnlyRestriction()
        {
            stampsGlobegisticsRestricted1 = new EditionRestriction(new Edition(trialStore), EditionFeature.StampsGlobegisticsConsolidator, EditionRestrictionLevel.Hidden);
            restrictions = new List<EditionRestriction>()
            {
                stampsGlobegisticsRestricted1
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2,
                disabledStore1,
                disabledStore2,
                trialStore
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.StampsGlobegisticsConsolidator, null, restrictions, stores);

            Assert.False(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.StampsGlobegisticsConsolidator));
        }

        [Fact]
        public void NoStampsGlobegisticsRestriction_WhenHalfOfTheStoresAreRestricted()
        {
            restrictions = new List<EditionRestriction>()
            {
                stampsGlobegisticsRestricted1
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2,
                disabledStore1,
                disabledStore2
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.StampsGlobegisticsConsolidator, null, restrictions, stores);

            Assert.False(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.StampsGlobegisticsConsolidator));
        }

        [Fact]
        public void StampsGlobegisticsRestriction_WhenTrialStoreIsTheOnlyStore()
        {
            stampsGlobegisticsRestricted1 = new EditionRestriction(new Edition(trialStore), EditionFeature.StampsGlobegisticsConsolidator, null, EditionRestrictionLevel.Hidden);
            restrictions = new List<EditionRestriction>()
            {
                stampsGlobegisticsRestricted1
            };

            stores = new List<StoreEntity>()
            {
                trialStore
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.StampsGlobegisticsConsolidator, null, restrictions, stores);

            Assert.True(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.StampsGlobegisticsConsolidator));
        }

        [Fact]
        public void StampsGlobegisticsRestriction_WhenAllStoresRestricted()
        {
            restrictions = new List<EditionRestriction>()
            {
                stampsGlobegisticsRestricted1,
                stampsGlobegisticsRestricted2
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.StampsGlobegisticsConsolidator, null, restrictions, stores);

            Assert.True(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.StampsGlobegisticsConsolidator));
        }

        [Fact]
        public void StampsGlobegisticsRestriction_WhenEnabledStoresAreRestricted_AndDisabledStoresAreNot()
        {
            restrictions = new List<EditionRestriction>()
            {
                stampsGlobegisticsRestricted1,
                stampsGlobegisticsRestricted2
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2,
                disabledStore1,
                disabledStore2
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.StampsGlobegisticsConsolidator, null, restrictions, stores);

            Assert.True(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.StampsGlobegisticsConsolidator));
        }

        [Fact]
        public void NoStampsIbcRestriction_WhenTrialStoreIsOnlyRestriction()
        {
            stampsIbcRestricted1 = new EditionRestriction(new Edition(trialStore), EditionFeature.StampsIbcConsolidator, EditionRestrictionLevel.Hidden);
            restrictions = new List<EditionRestriction>()
            {
                stampsIbcRestricted1
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2,
                disabledStore1,
                disabledStore2,
                trialStore
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.StampsIbcConsolidator, null, restrictions, stores);

            Assert.False(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.StampsIbcConsolidator));
        }

        [Fact]
        public void NoStampsIbcRestriction_WhenHalfOfTheStoresAreRestricted()
        {
            restrictions = new List<EditionRestriction>()
            {
                stampsIbcRestricted1
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2,
                disabledStore1,
                disabledStore2
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.StampsIbcConsolidator, null, restrictions, stores);

            Assert.False(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.StampsIbcConsolidator));
        }

        [Fact]
        public void StampsIbcRestriction_WhenTrialStoreIsTheOnlyStore()
        {
            stampsIbcRestricted1 = new EditionRestriction(new Edition(trialStore), EditionFeature.StampsIbcConsolidator, null, EditionRestrictionLevel.Hidden);
            restrictions = new List<EditionRestriction>()
            {
                stampsIbcRestricted1
            };

            stores = new List<StoreEntity>()
            {
                trialStore
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.StampsIbcConsolidator, null, restrictions, stores);

            Assert.True(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.StampsIbcConsolidator));
        }

        [Fact]
        public void StampsIbcRestriction_WhenAllStoresRestricted()
        {
            restrictions = new List<EditionRestriction>()
            {
                stampsIbcRestricted1,
                stampsIbcRestricted2
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.StampsIbcConsolidator, null, restrictions, stores);

            Assert.True(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.StampsIbcConsolidator));
        }

        [Fact]
        public void StampsIbcRestriction_WhenEnabledStoresAreRestricted_AndDisabledStoresAreNot()
        {
            restrictions = new List<EditionRestriction>()
            {
                stampsIbcRestricted1,
                stampsIbcRestricted2
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2,
                disabledStore1,
                disabledStore2
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.StampsIbcConsolidator, null, restrictions, stores);

            Assert.True(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.StampsIbcConsolidator));
        }

        [Fact]
        public void NoStampsRrDonnelleyRestriction_WhenTrialStoreIsOnlyRestriction()
        {
            stampsRrDonnelleyRestricted1 = new EditionRestriction(new Edition(trialStore), EditionFeature.StampsRrDonnelleyConsolidator, EditionRestrictionLevel.Hidden);
            restrictions = new List<EditionRestriction>()
            {
                stampsRrDonnelleyRestricted1
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2,
                disabledStore1,
                disabledStore2,
                trialStore
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.StampsRrDonnelleyConsolidator, null, restrictions, stores);

            Assert.False(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.StampsRrDonnelleyConsolidator));
        }

        [Fact]
        public void NoStampsRrDonnelleyRestriction_WhenHalfOfTheStoresAreRestricted()
        {
            restrictions = new List<EditionRestriction>()
            {
                stampsRrDonnelleyRestricted1
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2,
                disabledStore1,
                disabledStore2
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.StampsRrDonnelleyConsolidator, null, restrictions, stores);

            Assert.False(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.StampsRrDonnelleyConsolidator));
        }

        [Fact]
        public void StampsRrDonnelleyRestriction_WhenTrialStoreIsTheOnlyStore()
        {
            stampsRrDonnelleyRestricted1 = new EditionRestriction(new Edition(trialStore), EditionFeature.StampsRrDonnelleyConsolidator, null, EditionRestrictionLevel.Hidden);
            restrictions = new List<EditionRestriction>()
            {
                stampsRrDonnelleyRestricted1
            };

            stores = new List<StoreEntity>()
            {
                trialStore
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.StampsRrDonnelleyConsolidator, null, restrictions, stores);

            Assert.True(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.StampsRrDonnelleyConsolidator));
        }

        [Fact]
        public void StampsRrDonnelleyRestriction_WhenAllStoresRestricted()
        {
            restrictions = new List<EditionRestriction>()
            {
                stampsRrDonnelleyRestricted1,
                stampsRrDonnelleyRestricted2
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.StampsRrDonnelleyConsolidator, null, restrictions, stores);

            Assert.True(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.StampsRrDonnelleyConsolidator));
        }

        [Fact]
        public void StampsRrDonnelleyRestriction_WhenEnabledStoresAreRestricted_AndDisabledStoresAreNot()
        {
            restrictions = new List<EditionRestriction>()
            {
                stampsRrDonnelleyRestricted1,
                stampsRrDonnelleyRestricted2
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2,
                disabledStore1,
                disabledStore2
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.StampsRrDonnelleyConsolidator, null, restrictions, stores);

            Assert.True(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.StampsRrDonnelleyConsolidator));
        }
        
        [Fact]
        public void NoDhlEcommerceDhlRestriction_WhenTrialStoreIsOnlyRestriction()
        {
            dhlEcommerceDhlMaxRestricted1 = new EditionRestriction(new Edition(trialStore), EditionFeature.DhlEcommerceMax, EditionRestrictionLevel.Hidden);
            restrictions = new List<EditionRestriction>()
            {
                dhlEcommerceDhlMaxRestricted1
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2,
                disabledStore1,
                disabledStore2,
                trialStore
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.DhlEcommerceMax, null, restrictions, stores);

            Assert.False(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.DhlEcommerceMax));
        }

        [Fact]
        public void NoDhlEcommerceDhlRestriction_WhenHalfOfTheStoresAreRestricted()
        {
            restrictions = new List<EditionRestriction>()
            {
                dhlEcommerceDhlMaxRestricted1
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2,
                disabledStore1,
                disabledStore2
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.DhlEcommerceMax, null, restrictions, stores);

            Assert.False(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.DhlEcommerceMax));
        }

        [Fact]
        public void DhlEcommerceDhlRestriction_WhenTrialStoreIsTheOnlyStore()
        {
            dhlEcommerceDhlMaxRestricted1 = new EditionRestriction(new Edition(trialStore), EditionFeature.DhlEcommerceMax, null, EditionRestrictionLevel.Hidden);
            restrictions = new List<EditionRestriction>()
            {
                dhlEcommerceDhlMaxRestricted1
            };

            stores = new List<StoreEntity>()
            {
                trialStore
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.DhlEcommerceMax, null, restrictions, stores);

            Assert.True(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.DhlEcommerceMax));
        }

        [Fact]
        public void DhlEcommerceDhlRestriction_WhenAllStoresRestricted()
        {
            restrictions = new List<EditionRestriction>()
            {
                dhlEcommerceDhlMaxRestricted1,
                dhlEcommerceDhlMaxRestricted2
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.DhlEcommerceMax, null, restrictions, stores);

            Assert.True(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.DhlEcommerceMax));
        }

        [Fact]
        public void DhlEcommerceDhlRestriction_WhenEnabledStoresAreRestricted_AndDisabledStoresAreNot()
        {
            restrictions = new List<EditionRestriction>()
            {
                dhlEcommerceDhlMaxRestricted1,
                dhlEcommerceDhlMaxRestricted2
            };

            stores = new List<StoreEntity>()
            {
                enabledStore1,
                enabledStore2,
                disabledStore1,
                disabledStore2
            };

            List<EditionRestriction> effectiveEditionRestrictions = EditionManager.RemoveRestrictionIfNeeded(EditionFeature.DhlEcommerceMax, null, restrictions, stores);

            Assert.True(effectiveEditionRestrictions.Any(r => r.Feature == EditionFeature.DhlEcommerceMax));
        }
    }
}
