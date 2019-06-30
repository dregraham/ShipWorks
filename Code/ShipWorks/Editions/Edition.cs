﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using Autofac;
using Interapptive.Shared;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Editions
{
    /// <summary>
    /// Base class for each possible edition of ShipWorks.  Also represents the Standard Edition.
    /// </summary>
    public class Edition : IEdition
    {
        StoreEntity store;
        EditionSharedOptions sharedOptions = new EditionSharedOptions();

        List<EditionRestriction> restrictions = new List<EditionRestriction>();
        bool restrictionsFinalized = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public Edition(StoreEntity store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            // Initialize to an empty shipment type restriction set
            ShipmentTypeFunctionality = ShipmentTypeFunctionality.Deserialize(store.StoreID, (XElement) null);

            this.store = store;
        }

        /// <summary>
        /// The store that is associated with this edition
        /// </summary>
        public StoreEntity Store
        {
            get { return store; }
        }

        /// <summary>
        /// Encapsulates all options that could be shared amongst multiple editions;
        /// </summary>
        public EditionSharedOptions SharedOptions
        {
            get { return sharedOptions; }
        }

        /// <summary>
        /// Gets or sets the shipment type functionality that can be configured.
        /// </summary>
        public ShipmentTypeFunctionality ShipmentTypeFunctionality { get; set; }

        /// <summary>
        /// Add a restriction to the edition
        /// </summary>
        protected void AddRestriction(EditionFeature feature, EditionRestrictionLevel level)
        {
            AddRestriction(feature, null, level);
        }

        /// <summary>
        /// Add a restriction to the edition
        /// </summary>
        protected void AddRestriction(EditionFeature feature, object data, EditionRestrictionLevel level)
        {
            if (restrictionsFinalized)
            {
                throw new InvalidOperationException("The restrictions for this edition have already been generated and finalized.");
            }

            restrictions.Add(new EditionRestriction(this, feature, data, level));

            // If limiting filters, we also have to make sure they can't get around it by creating "My Filters"
            if (feature == EditionFeature.FilterLimit)
            {
                restrictions.Add(new EditionRestriction(this, EditionFeature.MyFilters, level));
            }
        }

        /// <summary>
        /// Removes any restrictions related to the given feature
        /// </summary>
        protected void RemoveRestriction(EditionFeature feature)
        {
            if (restrictionsFinalized)
            {
                throw new InvalidOperationException("The restrictions for this edition have already been generated and finalized.");
            }

            // Copy the list, excluding the ones that are for this feature
            restrictions = restrictions.Where(r => r.Feature != feature).ToList();
        }

        /// <summary>
        /// Get the restrictions imposed by this edition
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethodAttribute]
        public virtual IEnumerable<EditionRestriction> GetRestrictions()
        {
            // Stamps DHL
            if (!sharedOptions.StampsDhlEnabled)
            {
                AddRestriction(EditionFeature.StampsDhl, EditionRestrictionLevel.Hidden);
            }

            // Endicia DHL
            if (!sharedOptions.EndiciaDhlEnabled)
            {
                AddRestriction(EditionFeature.EndiciaDhl, EditionRestrictionLevel.Hidden);
            }

            // Endicia Insurance
            if (!sharedOptions.EndiciaInsuranceEnabled)
            {
                AddRestriction(EditionFeature.EndiciaInsurance, EditionRestrictionLevel.Hidden);
            }

            // UPS SurePost
            if (!sharedOptions.UpsSurePostEnabled && !UpsUtility.HasSurePostShipments)
            {
                AddRestriction(EditionFeature.UpsSurePost, EditionRestrictionLevel.Hidden);
            }

            // Endicia Consolidation
            if (!sharedOptions.EndiciaConsolidatorEnabled)
            {
                AddRestriction(EditionFeature.EndiciaConsolidator, EditionRestrictionLevel.Hidden);
            }

            // Endicia Scan Based Returns
            if (!sharedOptions.EndiciaScanBasedReturnEnabled)
            {
                AddRestriction(EditionFeature.EndiciaScanBasedReturns, EditionRestrictionLevel.Hidden);
            }

            // Stamps Insurance
            if (!sharedOptions.StampsInsuranceEnabled)
            {
                AddRestriction(EditionFeature.StampsInsurance, EditionRestrictionLevel.Hidden);
            }

            // Warehouse
            if (!sharedOptions.WarehouseEnabled)
            {
                AddRestriction(EditionFeature.Warehouse, EditionRestrictionLevel.Hidden);
            }

            // Adds any Stamps consolidator restrictions, if required
            AddStampsConsolidatorRestrictions();

            // Load the shipment type functionality into the restriction set
            foreach (ShipmentTypeCode typeCode in Enum.GetValues(typeof(ShipmentTypeCode)))
            {
                List<ShipmentTypeRestrictionType> restrictionTypes = ShipmentTypeFunctionality[typeCode].ToList();

                if (restrictionTypes.Any(r => r == ShipmentTypeRestrictionType.Disabled))
                {
                    AddRestriction(EditionFeature.ShipmentType, typeCode, EditionRestrictionLevel.Hidden);
                }

                if (restrictionTypes.Any(r => r == ShipmentTypeRestrictionType.AccountRegistration))
                {
                    AddRestriction(EditionFeature.ShipmentTypeRegistration, typeCode, EditionRestrictionLevel.Hidden);

                    // If account registration is not allowed and there are no accounts in the db for this shipment type,
                    // it is effectively a disabled shipment type, so go ahead and add that restriction as well.
                    ShipmentType shipmentType = ShipmentTypeManager.GetType(typeCode);
                    if (!shipmentType.HasAccounts)
                    {
                        AddRestriction(EditionFeature.ShipmentType, typeCode, EditionRestrictionLevel.Hidden);
                    }
                }

                if (restrictionTypes.Any(r => r == ShipmentTypeRestrictionType.Processing))
                {
                    AddRestriction(EditionFeature.ProcessShipment, typeCode, EditionRestrictionLevel.Forbidden);
                }

                if (restrictionTypes.Any(r => r == ShipmentTypeRestrictionType.Purchasing))
                {
                    AddRestriction(EditionFeature.PurchasePostage, typeCode, EditionRestrictionLevel.Forbidden);
                }

                if (restrictionTypes.Any(r => r == ShipmentTypeRestrictionType.RateDiscountMessaging))
                {
                    AddRestriction(EditionFeature.RateDiscountMessaging, typeCode, EditionRestrictionLevel.Forbidden);
                }

                if (restrictionTypes.Any(r => r == ShipmentTypeRestrictionType.ShippingAccountConversion))
                {
                    AddRestriction(EditionFeature.ShippingAccountConversion, typeCode, EditionRestrictionLevel.Forbidden);
                }
            }

            restrictionsFinalized = true;

            return restrictions;
        }

        /// <summary>
        /// Adds any Stamps consolidator restrictions, if required
        /// </summary>
        private void AddStampsConsolidatorRestrictions()
        {
            // Ascendia
            if (!sharedOptions.StampsAscendiaEnabled)
            {
                AddRestriction(EditionFeature.StampsAscendiaConsolidator, EditionRestrictionLevel.Hidden);
            }

            // DHL
            if (!sharedOptions.StampsDhlConsolidatorEnabled)
            {
                AddRestriction(EditionFeature.StampsDhlConsolidator, EditionRestrictionLevel.Hidden);
            }

            // Globegistics
            if (!sharedOptions.StampsGlobegisticsEnabled)
            {
                AddRestriction(EditionFeature.StampsGlobegisticsConsolidator, EditionRestrictionLevel.Hidden);
            }

            // IBC
            if (!sharedOptions.StampsIbcEnabled)
            {
                AddRestriction(EditionFeature.StampsIbcConsolidator, EditionRestrictionLevel.Hidden);
            }

            // RR Donnelley
            if (!sharedOptions.StampsRrDonnelleyEnabled)
            {
                AddRestriction(EditionFeature.StampsRrDonnelleyConsolidator, EditionRestrictionLevel.Hidden);
            }
        }

        /// <summary>
        /// Must be implemented by derived classes that add 'RequiresUpgrade' restrictions.  Return true to indicate the upgrade was successful.
        /// </summary>
        public virtual bool PromptForUpgrade(IWin32Window owner, EditionRestrictionIssue issue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a serialized version of this edition
        /// </summary>
        public string Serialize() => EditionSerializer.Serialize(this);

        /// <summary>
        /// The ShipmentType that should be the initial default for the edition, or null if it doesn't matter.
        /// </summary>
        public virtual ShipmentTypeCode? DefaultShipmentType
        {
            get
            {
                try
                {
                    // Just use the defer to the shipping settings. This will be USPS in the case that
                    // the USPS account was added during activation otherwise it will be None.
                    ShippingSettingsEntity shippingSettings = ShippingSettings.Fetch();
                    ShipmentTypeCode defaultShipmentType = (ShipmentTypeCode) shippingSettings.DefaultType;

                    return defaultShipmentType;
                }
                catch (Exception)
                {
                    return ShipmentTypeCode.None;
                }
            }
        }
    }
}
