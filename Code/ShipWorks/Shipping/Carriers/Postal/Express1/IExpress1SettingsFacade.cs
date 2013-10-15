using System.Collections.Generic;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Defines a way of interacting with Express1 settings for various shipping types
    /// </summary>
    public interface IExpress1SettingsFacade
    {
        /// <summary>
        /// Gets and sets whether Express1 should be used if possible
        /// </summary>
        bool UseExpress1 { get; set; }

        /// <summary>
        /// Gets and sets the id of the Express1 account to use
        /// </summary>
        long Express1Account { get; set; }

        /// <summary>
        /// Gets and sets the shipment type associated with this account type
        /// </summary>
        ShipmentType ShipmentType { get; }

        /// <summary>
        /// Gets a list of account descriptions and ids
        /// </summary>
        /// <returns></returns>
        ICollection<KeyValuePair<string, long>> Express1Accounts { get; }

        /// <summary>
        /// Save the Express1 settings into the actual settings entity
        /// </summary>
        /// <param name="settings">Settings entity into which the settings should be saved</param>
        void SaveSettings(ShippingSettingsEntity settings);

        /// <summary>
        /// Gets a person to use as the default for new Express1 accounts
        /// </summary>
        PersonAdapter DefaultAccountPerson { get; }
    }
}
