using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate.Fake
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

        public List<RateResult> GetBestRates(ShipmentEntity shipment, Action<BrokerException> exceptionHandler)
        {
            foreach (BrokerException brokerException in brokerExceptionsToThrow)
            {
                // We just want call the exception handler
                exceptionHandler(brokerException);
            }

            return new List<RateResult>();
        }

        public bool HasAccounts
        {
            get { return true; }
        }
    }
}
