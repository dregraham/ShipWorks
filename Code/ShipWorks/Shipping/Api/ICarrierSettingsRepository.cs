using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Api
{
    /// <summary>
    /// An interface for retrieving carrier settings that could very across environments (testing, production, etc.).
    /// </summary>
    public interface ICarrierSettingsRepository
    {
        /// <summary>
        /// Gets or sets a value indicating whether to [use test server].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use test server]; otherwise, <c>false</c>.
        /// </value>
        bool UseTestServer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to [use list rates]. Indicates if LIST rates are in 
        /// effect, instead of the standard ACCOUNT rates
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use list rates]; otherwise, <c>false</c>.
        /// </value>
        bool UseListRates { get; set; }

        /// <summary>
        /// Gets the shipping settings.
        /// </summary>
        /// <returns>The ShippingSettingsEntity.</returns>
        ShippingSettingsEntity GetShippingSettings();

        /// <summary>
        /// Saves the shipping settings to the data source.
        /// </summary>
        /// <param name="shippingSettings">The shipping settings.</param>
        void SaveShippingSettings(ShippingSettingsEntity shippingSettings);

        /// <summary>
        /// Gets the carrier account associated with the given shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A carrier account entity object. </returns>
        IEntity2 GetAccount(ShipmentEntity shipment);

        /// <summary>
        /// Gets all of the carrier accounts that have been created in ShipWorks.
        /// </summary>
        /// <returns>A collection of IEntity2 carrier accounts object.</returns>
        IEnumerable<IEntity2> GetAccounts();

        /// <summary>
        /// Indicates whether the current user is an Interapptive user.
        /// </summary>
        /// <value><c>true</c> if this user is an Interapptive user; otherwise, <c>false</c>.</value>
        bool IsInterapptiveUser { get; }
    }
}
