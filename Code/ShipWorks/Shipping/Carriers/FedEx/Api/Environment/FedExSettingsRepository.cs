using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Environment
{
    /// <summary>
    /// A FedEx implementation of the ICarrierSettingsRepository interface. This communicates with external
    /// dependencies/data stores such as the ShipWorks database and the Windows registry.
    /// </summary>
    [Component(RegistrationType.Self)]
    [KeyedComponent(typeof(ICarrierSettingsRepository), ShipmentTypeCode.FedEx)]
    public class FedExSettingsRepository : ICarrierSettingsRepository
    {
        /// <summary>
        /// Gets or sets a value indicating whether [use test server] based on a registry setting.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use test server]; otherwise, <c>false</c>.
        /// </value>
        public bool UseTestServer
        {
            get { return InterapptiveOnly.Registry.GetValue("FedExTestServer", false); }
            set { InterapptiveOnly.Registry.SetValue("FedExTestServer", value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to [use list rates] based on a registry setting. Indicates if LIST rates are in
        /// effect, instead of the standard ACCOUNT rates
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use list rates]; otherwise, <c>false</c>.
        /// </value>
        public bool UseListRates
        {
            get { return InterapptiveOnly.Registry.GetValue("FedExListRates", false); }
            set { InterapptiveOnly.Registry.SetValue("FedExListRates", value); }
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
        /// Gets the FedEx account associated with the given shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A FedExAccountEntity object.</returns>
        public virtual IEntity2 GetAccount(ShipmentEntity shipment)
        {
            if (shipment == null || shipment.FedEx == null)
            {
                throw new FedExException("ShipWorks cannot not find the FedEx account associated with a null shipment.");
            }

            return FedExAccountManager.GetAccount(shipment.FedEx.FedExAccountID);
        }


        /// <summary>
        /// Gets all of the active FedEx accounts that have been created in ShipWorks.
        /// </summary>
        /// <returns>
        /// A collection of FedExAccountEntity object. (The return type may need to be changed to an IEntity2 object
        /// if this interface is used outside of FedEx in the future.)
        /// </returns>
        public virtual IEnumerable<IEntity2> GetAccounts()
        {
            return FedExAccountManager.Accounts;
        }

        /// <summary>
        /// Indicates whether the current user is an Interapptive user.
        /// </summary>
        /// <value><c>true</c> if this user is an Interapptive user; otherwise, <c>false</c>.</value>
        public bool IsInterapptiveUser
        {
            get { return InterapptiveOnly.IsInterapptiveUser; }
        }
    }
}
