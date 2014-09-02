using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping;
using ShipWorks.Stores;
using Interapptive.Shared.Utility;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;

namespace ShipWorks.Editions
{
    /// <summary>
    /// Base class for each possible edition of ShipWorks.  Also represents the Standard Edition.
    /// </summary>
    public class Edition
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
        public virtual IEnumerable<EditionRestriction> GetRestrictions()
        {
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

            restrictionsFinalized = true;

            return restrictions;
        }

        /// <summary>
        /// Must be implemented by derived classes that add 'RequiresUpgrade' restrictions.  Return true to indicate the upgrade was successful.
        /// </summary>
        public virtual bool PromptForUpgrade(IWin32Window owner, EditionRestrictionIssue issue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The ShipmentType that should be the initial default for the edition, or null if it doesnt matter.
        /// </summary>
        public virtual ShipmentTypeCode? DefaultShipmentType
        {
            get { return ShipmentTypeCode.None; }
        }
    }
}
