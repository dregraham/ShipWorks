using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Defaults;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.Postal.Endicia;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// A setup wizard for the USPS (Stamps.com Expedited) shipment type.
    /// </summary>
    public class UspsSetupWizard : StampsSetupWizard
    {
        private readonly Dictionary<long, long> profileMap = new Dictionary<long, long>();

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsSetupWizard"/> class.
        /// </summary>
        /// <param name="promotion">The promotion.</param>
        /// <param name="allowRegisteringExistingAccount">if set to <c>true</c> [allow registering existing account].</param>
        public UspsSetupWizard(IRegistrationPromotion promotion, bool allowRegisteringExistingAccount)
            : base(promotion, allowRegisteringExistingAccount, ShipmentTypeCode.Usps)
        { }

        /// <summary>
        /// Gets or sets the initial account address that to use when adding an account.
        /// </summary>
        public PersonAdapter InitialAccountAddress { get; set; }
        
        /// <summary>
        /// Initialization
        /// </summary>
        protected override void OnLoad(object sender, System.EventArgs e)
        {
            base.OnLoad(sender, e);

            if (InitialAccountAddress != null)
            {
                // Pre-load the person control with our initial account address (in the event an account is being
                // created via the Activate Postage Discount dialog
                PersonControl.LoadEntity(InitialAccountAddress);
            }

        }
        /// <summary>
        /// Prepares the stamps account for save. Just sets the reseller type to expedited.
        /// </summary>
        protected override void PrepareStampsAccountForSave()
        {
            base.PrepareStampsAccountForSave();
            UspsAccount.UspsReseller = (int) StampsResellerType.StampsExpedited;
        }

        protected override void OnFormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            base.OnFormClosing(sender, e);

            if (DialogResult == DialogResult.OK)
            {
                ShippingSettingsEntity settings = ShippingSettings.Fetch();

                // We also need to exclude Endicia, Express1, and the original Stamps.com from the list 
                // of active providers since the customer agreed to use USPS (Stamps.com Expedited)
                ExcludeShipmentType(settings, ShipmentTypeCode.Endicia);
                ExcludeShipmentType(settings, ShipmentTypeCode.Express1Endicia);
                ExcludeShipmentType(settings, ShipmentTypeCode.Express1Stamps);
                ExcludeShipmentType(settings, ShipmentTypeCode.Stamps);
                ExcludeShipmentType(settings, ShipmentTypeCode.PostalWebTools);

                // There's a chance we came from Stamps.com shipment type, so make sure USPS is not excluded
                // before saving the settings
                List<int> excludedTypes = settings.ExcludedTypes.ToList();
                excludedTypes.Remove((int) ShipmentTypeCode.Usps);
                settings.ExcludedTypes = excludedTypes.ToArray();

                ShippingSettings.Save(settings);

                // Need to update any rules to swap out Endicia, Express1, and the original Stamps.com 
                // with USPS (Stamps.com Expedited) now that those types are not longer active
                UseUspsInDefaultShippingRulesFor(ShipmentTypeCode.Endicia);
                UseUspsInDefaultShippingRulesFor(ShipmentTypeCode.Express1Endicia);
                UseUspsInDefaultShippingRulesFor(ShipmentTypeCode.Express1Stamps);
                UseUspsInDefaultShippingRulesFor(ShipmentTypeCode.Stamps);
                UseUspsInDefaultShippingRulesFor(ShipmentTypeCode.PostalWebTools);

                // We may have came from USPS (Stamps.com), which would not have marked USPS as configured, so mark it now.
                ShippingSettings.MarkAsConfigured(ShipmentTypeCode.Usps);

                ShippingSettingsEventDispatcher.DispatchUspsAccountCreated(this, new ShippingSettingsEventArgs(ShipmentTypeCode.Usps));
            }
        }

        /// <summary>
        /// Excludes the given shipment type from the list of active shipping providers.
        /// </summary>
        /// <param name="settings">The settings being updated.</param>
        /// <param name="shipmentTypeCode">The shipment type code to be excluded.</param>
        private void ExcludeShipmentType(ShippingSettingsEntity settings, ShipmentTypeCode shipmentTypeCode)
        {
            if (!settings.ExcludedTypes.Any(t => t == (int)shipmentTypeCode))
            {
                List<int> excludedTypes = settings.ExcludedTypes.ToList();
                excludedTypes.Add((int)shipmentTypeCode);

                settings.ExcludedTypes = excludedTypes.ToArray();
            }
        }

        /// <summary>
        /// Uses the USPS (Stamps.com Expedited) as the shipping provider for any rules using the given shipment type code.
        /// </summary>
        /// <param name="shipmentTypeCode">The shipment type code to be replaced with USPS (Stamps.com Expedited) .</param>
        private void UseUspsInDefaultShippingRulesFor(ShipmentTypeCode shipmentTypeCode)
        {
            List<ShippingDefaultsRuleEntity> rules = ShippingDefaultsRuleManager.GetRules(shipmentTypeCode);
            foreach (ShippingDefaultsRuleEntity rule in rules)
            {
                ShippingDefaultsRuleEntity clonedRule = CreateCopy(rule);

                clonedRule.ShipmentType = (int)ShipmentTypeCode.Usps;
                clonedRule.ShippingProfileID = GetRuleProfile(rule.ShippingProfileID);

                ShippingDefaultsRuleManager.SaveRule(clonedRule);
            }
        }

        /// <summary>
        /// Get a copied profile for the specified id
        /// </summary>
        private long GetRuleProfile(long profileId)
        {
            if (profileMap.ContainsKey(profileId))
            {
                return profileMap[profileId];
            }

            long newProfileId = CreateProfileClone(profileId);

            profileMap.Add(profileId, newProfileId);

            return newProfileId;
        }

        /// <summary>
        /// Create a copy of the profile with the given id
        /// </summary>
        private static long CreateProfileClone(long profileId)
        {
            ShippingProfileEntity profile = ShippingProfileManager.GetProfile(profileId);

            // If we can't find the requested profile (or it's not set in the original rule), we'll just set the new rule to (none).
            if (profile == null)
            {
                return 0;
            }

            ShippingProfileEntity newProfile = CreateCopy(profile);

            newProfile.Name = string.Format("{0} (Copy)", profile.Name);
            newProfile.ShipmentType = (int)ShipmentTypeCode.Usps;
            newProfile.ShipmentTypePrimary = false;

            newProfile.Postal = CreateCopy(profile.Postal);
            newProfile.Postal.Usps = new UspsProfileEntity();

            EnsureUniqueName(newProfile, profile.Name);

            ShippingProfileManager.SaveProfile(newProfile);

            return newProfile.ShippingProfileID;
        }

        /// <summary>
        /// Attempt to ensure a unique name for the new profile
        /// </summary>
        private static void EnsureUniqueName(ShippingProfileEntity newProfile, string baseName)
        {
            int copyNumber = 1;
            while (ShippingProfileManager.DoesNameExist(newProfile))
            {
                newProfile.Name = string.Format("{0} (Copy {1})", baseName, copyNumber);
                copyNumber++;

                // If we can't find a unique name after 10 attempts, just give up...
                if (copyNumber == 10)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Create a copy of the specified object
        /// </summary>
        /// <returns>
        /// EntityUtility.CloneEntity would have worked, but this is specific to the needs of copying the profile. We don't want
        /// to copy any other postal profile data, Ups, FedEx, etc.
        /// </returns>
        private static T CreateCopy<T>(T copyFrom) where T : IEntity2, new()
        {
            T newObject = new T();

            foreach (IEntityField2 field in copyFrom.Fields.Cast<IEntityField2>().Where(field => !field.IsPrimaryKey && !field.IsReadOnly))
            {
                newObject.Fields[field.Name].CurrentValue = field.CurrentValue;
                newObject.Fields[field.Name].IsChanged = true;
            }

            newObject.Fields.IsDirty = true;

            return newObject;
        }
    }
}
