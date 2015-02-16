using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1
{
    /// <summary>
    /// Defines a way of interacting with Express1/Stamps settings
    /// </summary>
    public class Express1StampsSettingsFacade : IExpress1SettingsFacade
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="settings"></param>
        public Express1StampsSettingsFacade(ShippingSettingsEntity settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            UseExpress1 = settings.UspsAutomaticExpress1;
            Express1Account = settings.UspsAutomaticExpress1Account;
        }

        /// <summary>
        /// Gets and sets whether Express1 should be used if possible
        /// </summary>
        public bool UseExpress1 { get; set; }

        /// <summary>
        /// Gets and sets the id of the Express1 account to use
        /// </summary>
        public long Express1Account { get; set; }

        /// <summary>
        /// Gets and sets the shipment type associated with this account type
        /// </summary>
        public ShipmentType ShipmentType
        {
            get
            {
                return new Express1StampsShipmentType();
            }
        }

        /// <summary>
        /// Gets a list of account descriptions and ids
        /// </summary>
        /// <returns></returns>
        public ICollection<KeyValuePair<string, long>> Express1Accounts
        {
            get
            {
                return StampsAccountManager.GetAccounts(UspsResellerType.Express1)
                           .Select(a => new KeyValuePair<string, long>(a.Description, a.UspsAccountID))
                           .ToList();
            }
        }

        /// <summary>
        /// Save the Express1/Stamps settings into the actual settings entity
        /// </summary>
        /// <param name="settings">Settings entity into which the settings should be saved</param>
        public void SaveSettings(ShippingSettingsEntity settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.UspsAutomaticExpress1 = UseExpress1;
            settings.UspsAutomaticExpress1Account = Express1Account;
        }

        /// <summary>
        /// Gets a person to use as the default for new Express1 accounts
        /// </summary>
        public PersonAdapter DefaultAccountPerson
        {
            get
            {
                List<UspsAccountEntity> accounts = StampsAccountManager.GetAccounts(UspsResellerType.None);

                return accounts.Count == 1 ? new PersonAdapter(accounts.Single(), "") : null;
            }
        }
    }
}
