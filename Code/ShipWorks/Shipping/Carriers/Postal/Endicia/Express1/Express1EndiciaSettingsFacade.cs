using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Postal.Express1;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// An implementation of the IExpress1SettingsFacade that defines the way for interacting
    /// with the Express1 for Endicia settings.
    /// </summary>
    public class Express1EndiciaSettingsFacade : IExpress1SettingsFacade
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Express1EndiciaSettingsFacade"/> class.
        /// </summary>
        /// <param name="settings">The shipping settings being used as the data source for the facade.</param>
        public Express1EndiciaSettingsFacade(IShippingSettingsEntity settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            UseExpress1 = settings.EndiciaAutomaticExpress1;
            Express1Account = settings.EndiciaAutomaticExpress1Account;
        }

        /// <summary>
        /// Gets and sets whether Express1 should be used if possible
        /// </summary>
        /// <exception cref="System.NotImplementedException">
        /// </exception>
        public bool UseExpress1 { get; set; }

        /// <summary>
        /// Gets and sets the ID of the Express1 account to use
        /// </summary>
        public long Express1Account { get; set; }

        /// <summary>
        /// Gets and sets the shipment type associated with this account type
        /// </summary>
        public ShipmentType ShipmentType
        {
            get { return new Express1EndiciaShipmentType(); }
        }

        /// <summary>
        /// Gets a list of account descriptions and IDs
        /// </summary>
        public ICollection<KeyValuePair<string, long>> Express1Accounts
        {
            get
            {
                return EndiciaAccountManager.GetAccounts(EndiciaReseller.Express1)
                           .Select(a => new KeyValuePair<string, long>(a.Description, a.EndiciaAccountID))
                           .ToList();
            }
        }

        /// <summary>
        /// Save the Express1 settings into the actual settings entity
        /// </summary>
        /// <param name="settings">Settings entity into which the settings should be saved</param>
        /// <exception cref="System.ArgumentNullException">settings</exception>
        public void SaveSettings(ShippingSettingsEntity settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.EndiciaAutomaticExpress1 = UseExpress1;
            settings.EndiciaAutomaticExpress1Account = Express1Account;
        }

        /// <summary>
        /// Gets a person to use as the default for new Express1 accounts
        /// </summary>
        public PersonAdapter DefaultAccountPerson
        {
            get
            {
                List<EndiciaAccountEntity> accounts = EndiciaAccountManager.GetAccounts(EndiciaReseller.None);
                return accounts.Count == 1 ? new PersonAdapter(accounts.Single(), "") : null;
            }
        }
    }
}
