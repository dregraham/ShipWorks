using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// An interface intended to provide a layer of abstraction between the best rate shipment type
    /// and the actual carriers. Implementations of this interface will handle the details for taking
    /// the criteria of a best rate shipment and building the corresponding carrier specific
    /// shipment. The broker nomenclature stems from a ticket broker metaphor where you would specify
    /// the general parameters of what you need, and the broker will work on your behalf to find the
    /// best tickets given that criteria. Tying the metaphor back to shipping with the best rate, the
    /// criteria would be things like dimensions, weight, etc.
    /// </summary>
    public interface IBestRateShippingBroker
    {
        /// <summary>
        /// Gets a value indicating whether there any accounts available to a broker.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the broker [has accounts]; otherwise, <c>false</c>.
        /// </value>
        bool HasAccounts { get; }

        /// <summary>
        /// Gets a value indicating whether [is counter rate].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is counter rate]; otherwise, <c>false</c>.
        /// </value>
        bool IsCounterRate { get; }

        /// <summary>
        /// Gets the rates for each of the accounts of a specific shipping provider based
        /// on the configuration of the best rate shipment data.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="brokerExceptions"></param>
        /// <returns>A list of RateResults for each account of a specific shipping provider (i.e. if
        /// two accounts are registered for a single provider, the list of rates would have two entries
        /// if both accounts returned rates).</returns>
        RateGroup GetBestRates(ShipmentEntity shipment, List<BrokerException> brokerExceptions);

        /// <summary>
        /// Gets the insurance provider for the carrier.
        /// </summary>
        InsuranceProvider GetInsuranceProvider(IShippingSettingsEntity settings);

        /// <summary>
        /// Determines whether customs forms are required for the given shipment for any accounts of a broker.
        /// A value of false will only be returned if all of the carrier accounts do not require customs forms.
        /// </summary>
        bool IsCustomsRequired(ShipmentEntity shipment);

        /// <summary>
        /// Configures the broker using the given settings.
        /// </summary>
        /// <param name="brokerSettings">The broker settings.</param>
        void Configure(IBestRateBrokerSettings brokerSettings);
    }
}
