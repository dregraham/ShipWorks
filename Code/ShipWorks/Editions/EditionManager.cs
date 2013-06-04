using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Editions.Brown;
using ShipWorks.Editions.Freemium;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using System.Diagnostics;
using System.Configuration;
using log4net;
using ShipWorks.Stores;
using ShipWorks.Data.Model.EntityClasses;
using System.IO;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.ApplicationCore.Licensing;
using Interapptive.Shared.Utility;

namespace ShipWorks.Editions
{
    /// <summary>
    /// Primary controller for edition management
    /// </summary>
    public static class EditionManager
    {
        static readonly ILog log = LogManager.GetLogger(typeof(EditionManager));

        // We default to having no restrictions
        static EditionRestrictionSet _restrictionSet = new EditionRestrictionSet();
        static object restrictionLock = new object();

        /// <summary>
        /// Raised when the edition has changed
        /// </summary>
        public static event EventHandler RestrictionsChanged;

        /// <summary>
        /// One-time application initialization
        /// </summary>
        public static void Initialize()
        {
            UpdateRestrictions(new List<StoreEntity>());
        }

        /// <summary>
        /// Get the edition that the app.config represents is the current edition
        /// </summary>
        public static EditionInstalledType InstalledEditionType
        {
            get
            {
                string editionValue = ConfigurationManager.AppSettings["edition"] ?? "standard";

                log.InfoFormat("Insalled edition: {0}", editionValue);

                foreach (var value in EnumHelper.GetEnumList<EditionInstalledType>())
                {
                    if (value.Description == editionValue)
                    {
                        return value.Value;
                    }
                }

                return EditionInstalledType.Standard;
            }
        }

        /// <summary>
        /// Update the active edition based on the edition information found in each of the stores
        /// </summary>
        public static void UpdateRestrictions()
        {
            UpdateRestrictions(StoreManager.GetAllStores());
        }

        /// <summary>
        /// Update the current edition restriction set based on the given store list
        /// </summary>
        private static void UpdateRestrictions(IEnumerable<StoreEntity> stores)
        {
            List<EditionRestriction> restrictions = new List<EditionRestriction>();            

            foreach (StoreEntity store in stores)
            {
                Edition edition = EditionSerializer.Restore(store);

                restrictions.AddRange(edition.GetRestrictions());
            }

            ActiveRestrictions = new EditionRestrictionSet(restrictions);
        }

        /// <summary>
        /// The current set of edition restrictions 
        /// </summary>
        public static EditionRestrictionSet ActiveRestrictions
        {
            get 
            {
                lock (restrictionLock)
                {
                    return _restrictionSet;
                }
            }
            private set
            {
                lock (restrictionLock)
                {
                    if (object.Equals(_restrictionSet, value))
                    {
                        return;
                    }

                    _restrictionSet = value;
                }

                if (RestrictionsChanged != null)
                {
                    RestrictionsChanged(null, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Update the given store to make sure it's set with the given edition data.  The store is not edited directly - any change is saved directly to the database.
        /// </summary>
        public static bool UpdateStoreEdition(StoreEntity store, Edition edition)
        {
            // See if the edition changed
            string updatedEdition = EditionSerializer.Serialize(edition);
            if (store.Edition != updatedEdition)
            {
                StoreEntity prototype = new StoreEntity(store.StoreID) { IsNew = false };
                prototype.Edition = updatedEdition;

                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.SaveAndRefetch(prototype);
                }

                // Get our in memory stores set up-to-date
                StoreManager.CheckForChanges();
                UpdateRestrictions();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Return true if unrestricted on exit, or false if restricted. Shows the edition upgrade window if applicable.  
        /// </summary>
        public static bool HandleRestrictionIssue(IWin32Window owner, EditionRestrictionIssue issue)
        {
            if (issue.Level == EditionRestrictionLevel.None)
            {
                return true;
            }
            else if (issue.Level == EditionRestrictionLevel.Hidden)
            {
                Debug.Fail("Shouldn't be able to get to this point on a hidden GUI element.");

                return false;
            }
            else if (issue.Level == EditionRestrictionLevel.Forbidden)
            {
                MessageHelper.ShowError(owner, issue.GetDescription());

                return false;
            }
            else
            {
                Edition edition = issue.Edition;

                if (edition.PromptForUpgrade(owner, issue))
                {
                    // Update the stores and restriction set based on the now upgraded edition
                    StoreManager.CheckForChanges();
                    UpdateRestrictions();

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
