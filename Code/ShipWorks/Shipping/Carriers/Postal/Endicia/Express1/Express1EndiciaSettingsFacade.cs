using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Express1;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// Defines a way of interacting with Express1/Endicia settings
    /// </summary>
    public class Express1EndiciaSettingsFacade : IExpress1SettingsFacade
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="settings"></param>
        public Express1EndiciaSettingsFacade(ShippingSettingsEntity settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            UseExpress1 = settings.StampsAutomaticExpress1;
            Express1Account = settings.StampsAutomaticExpress1Account;
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
                return new Express1EndiciaShipmentType();
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
                return EndiciaAccountManager.GetAccounts(EndiciaReseller.Express1)
                           .Select(a => new KeyValuePair<string, long>(a.Description, a.EndiciaAccountID))
                           .ToList();
            }
        }

        /// <summary>
        /// Save the Express1/Endicia settings into the actual settings entity
        /// </summary>
        /// <param name="settings">Settings entity into which the settings should be saved</param>
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
                if (EndiciaAccountManager.GetAccounts(EndiciaReseller.None).Count == 1)
                {
                    return new PersonAdapter(EndiciaAccountManager.GetAccounts(EndiciaReseller.None)[0], "");
                }

                return null;
            }
        }
    }
}
