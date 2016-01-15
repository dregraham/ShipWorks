using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Tests.Carriers.BestRate
{
    /// <summary>
    /// A fake broker for the purpose of testing the best rate shipment type where the only thing it does 
    /// is call the exception handler provided to the best rates method for the broker exception(s) provided
    /// in the constructor.
    /// </summary>
    public class FakeExceptionHandlerBroker : IBestRateShippingBroker
    {
        private readonly List<BrokerException> brokerExceptionsToThrow;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeExceptionHandlerBroker"/> class.
        /// </summary>
        /// <param name="brokerExceptionToThrow">The broker exception to throw.</param>
        public FakeExceptionHandlerBroker(BrokerException brokerExceptionToThrow)
            : this (new List<BrokerException> { brokerExceptionToThrow })
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeExceptionHandlerBroker" /> class.
        /// </summary>
        /// <param name="brokerExceptionsToThrow">The broker exceptions to throw.</param>
        public FakeExceptionHandlerBroker(IEnumerable<BrokerException> brokerExceptionsToThrow)
        {
            this.brokerExceptionsToThrow = new List<BrokerException>(brokerExceptionsToThrow);
        }
        
        /// <summary>
        /// Gets a value indicating whether there any accounts available to a broker.
        /// </summary>
        /// <value>
        /// <c>true</c> if the broker [has accounts]; otherwise, <c>false</c>.
        /// </value>
        public bool HasAccounts
        {
            get { return true; }
        }
        
        /// <summary>
        /// Gets the rates for each of the accounts of a specific shipping provider based
        /// on the configuration of the best rate shipment data.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="brokerExceptions"></param>
        /// <returns>
        /// A list of RateResults for each account of a specific shipping provider (i.e. if
        /// two accounts are registered for a single provider, the list of rates would have two entries
        /// if both accounts returned rates).
        /// </returns>
        public RateGroup GetBestRates(ShipmentEntity shipment, List<BrokerException> brokerExceptions)
        {
            foreach (BrokerException brokerException in brokerExceptionsToThrow)
            {
                // We just want call the exception handler
                brokerExceptions.Add(brokerException);
            }

            return new RateGroup(new List<RateResult>());
        }

        /// <summary>
        /// Gets the insurance provider for the carrier.
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public InsuranceProvider GetInsuranceProvider(ShippingSettingsEntity settings)
        {
            return InsuranceProvider.Invalid;
        }

        /// <summary>
        /// Determines whether customs forms are required for the given shipment for any accounts of a broker.
        /// A value of false will only be returned if all of the carrier accounts do not require customs forms.
        /// </summary>
        public bool IsCustomsRequired(ShipmentEntity shipment)
        {
            return false;
        }
        
        /// <summary>
        /// Configures the broker using the given settings.
        /// </summary>
        /// <param name="brokerSettings">The broker settings.</param>
        public void Configure(IBestRateBrokerSettings brokerSettings)
        { }


        /// <summary>
        /// Gets a value indicating whether [is counter rate].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is counter rate]; otherwise, <c>false</c>.
        /// </value>
        public bool IsCounterRate
        {
            get
            {
                return false;
            }
        }
    }
}
