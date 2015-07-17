using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Stores;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.Editions.Freemium
{
    /// <summary>
    /// Edition for the Endicia\eBay Freemium version of ShipWorks
    /// </summary>
    public class FreemiumFreeEdition : Edition
    {
        static readonly int maxActions = 3;
        static readonly int maxFilters = 10;
        static readonly int maxSelection = 5;

        string account;
        FreemiumAccountType accountType;

        /// <summary>
        /// Constructor
        /// </summary>
        public FreemiumFreeEdition(StoreEntity store, string account, FreemiumAccountType accountType) 
            : base(store)
        {
            this.account = account ?? "";
            this.accountType = accountType;

            foreach (ShipmentTypeCode shipmentType in Enum.GetValues(typeof(ShipmentTypeCode)))
            {
                if (shipmentType == ShipmentTypeCode.Endicia || shipmentType == ShipmentTypeCode.Express1Endicia || shipmentType == ShipmentTypeCode.BestRate)
                {
                    continue;
                }

                AddRestriction(EditionFeature.ShipmentType, shipmentType, 
                    shipmentType == ShipmentTypeCode.BestRate ? EditionRestrictionLevel.Hidden : EditionRestrictionLevel.RequiresUpgrade);
            }

            AddRestriction(EditionFeature.ActionLimit, maxActions, EditionRestrictionLevel.RequiresUpgrade);
            AddRestriction(EditionFeature.FilterLimit, maxFilters, EditionRestrictionLevel.RequiresUpgrade);
            AddRestriction(EditionFeature.SelectionLimit, maxSelection, EditionRestrictionLevel.RequiresUpgrade);

            AddRestriction(EditionFeature.AddOrderCustomer, EditionRestrictionLevel.RequiresUpgrade);
            AddRestriction(EditionFeature.EndiciaScanForm, EditionRestrictionLevel.RequiresUpgrade);

            AddRestriction(EditionFeature.SingleStore, StoreTypeCode.Ebay, EditionRestrictionLevel.RequiresUpgrade);

            // Add account count restriction
            AddRestriction(EditionFeature.EndiciaAccountLimit, 1, EditionRestrictionLevel.RequiresUpgrade);

            if (!string.IsNullOrWhiteSpace(account))
            {
                AddRestriction(EditionFeature.EndiciaAccountNumber, account, EditionRestrictionLevel.RequiresUpgrade);
            }
        }

        /// <summary>
        /// Indicates if 'Freemium' mode is active.  This will be true if there are no stores and Freemium version is installed, or if there
        /// is exactly one freemium store.
        /// </summary>
        public static bool IsActive
        {
            get
            {
                var stores = StoreManager.GetAllStores();

                if (stores.Count == 0)
                {
                    return EditionManager.InstalledEditionType == EditionInstalledType.EndiciaEbay;
                }
                else if (stores.Count == 1)
                {
                    return EditionSerializer.Restore(stores.Single()) is FreemiumFreeEdition;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Get the account number (if any yet) associated with the edition
        /// </summary>
        public string AccountNumber
        {
            get { return account; }
        }

        /// <summary>
        /// If AccountNumber is non-empty, indicates what typeof endicia account it is 
        /// </summary>
        public FreemiumAccountType AccountType
        {
            get { return accountType; }
        }

        /// <summary>
        /// Prompt the user to upgrade the edition.  Return true if the upgrade completes successfully.
        /// </summary>
        public override bool PromptForUpgrade(IWin32Window owner, EditionRestrictionIssue issue)
        {
            using (FreemiumUpgradeWizard dlg = new FreemiumUpgradeWizard(issue))
            {
                return dlg.ShowDialog(owner) == DialogResult.OK;
            }
        }

        /// <summary>
        /// Default shipment type is Endicia
        /// </summary>
        public override ShipmentTypeCode? DefaultShipmentType
        {
            get
            {
                return ShipmentTypeCode.Endicia;
            }
        }
    }
}
