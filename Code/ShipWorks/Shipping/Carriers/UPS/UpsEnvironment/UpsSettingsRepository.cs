using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.UPS.UpsEnvironment
{
    /// <summary>
    /// A UPS implementation of the ICarrierSettingsRepository interface. This communicates with external
    /// dependencies/data stores such as the ShipWorks database and the Windows registry.
    /// </summary>
    public class UpsSettingsRepository : ICarrierSettingsRepository
    {
        /// <summary>
        /// Gets or sets a value indicating whether [use test server] based on a registry setting.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use test server]; otherwise, <c>false</c>.
        /// </value>
        public bool UseTestServer
        {
            get { return UpsWebClient.UseTestServer; }
            set { UpsWebClient.UseTestServer = value; }
        }

        /// <summary>
        /// Gets the shipping settings.
        /// </summary>
        /// <returns>
        /// The ShippingSettingsEntity.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual ShippingSettingsEntity GetShippingSettings()
        {
            return ShippingSettings.Fetch();
        }

        /// <summary>
        /// Saves the shipping settings to the data source.
        /// </summary>
        /// <param name="shippingSettings">The shipping settings.</param>
        public void SaveShippingSettings(ShippingSettingsEntity shippingSettings)
        {
            ShippingSettings.Save(shippingSettings);
        }

        /// <summary>
        /// Gets the UPS account associated with the given shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A UpsOpenAccountAccountEntity object.</returns>
        public IEntity2 GetAccount(ShipmentEntity shipment)
        {
            if (shipment == null || shipment.Ups == null)
            {
                throw new UpsException("ShipWorks cannot not find the UPS account associated with a null shipment.");
            }

            return UpsAccountManager.GetAccount(shipment.Ups.UpsAccountID);
        }

        /// <summary>
        /// Gets all of the active UpsOpenAccount accounts that have been created in ShipWorks.
        /// </summary>
        /// <returns>
        /// A collection of UpsOpenAccountAccountEntity object. (The return type may need to be changed to an IEntity2 object
        /// if this interface is used outside of UpsOpenAccount in the future.)
        /// </returns>
        public IEnumerable<IEntity2> GetAccounts()
        {
            return UpsAccountManager.Accounts;
        }

        /// <summary>
        /// Indicates whether the current user is an Interapptive user.
        /// </summary>
        /// <value><c>true</c> if this user is an Interapptive user; otherwise, <c>false</c>.</value>
        public bool IsInterapptiveUser
        {
            get { return InterapptiveOnly.IsInterapptiveUser; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to [use list rates]. Indicates if LIST rates are in
        /// effect, instead of the standard ACCOUNT rates
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use list rates]; otherwise, <c>false</c>.
        /// </value>
        /// <exception cref="System.NotImplementedException">
        /// </exception>
        public bool UseListRates
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
