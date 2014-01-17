using System.Collections.Generic;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.BestRate.Footnote
{
    /// <summary>
    /// An implementation of the IRateFootnoteFactory that creates instances of a 
    /// BrokerExceptionsRateFootnoteControl.
    /// </summary>
    public class BrokerExceptionsRateFootnoteFactory : IRateFootnoteFactory
    {
        private readonly IEnumerable<BrokerException> brokerExceptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrokerExceptionsRateFootnoteFactory"/> class.
        /// </summary>
        /// <param name="brokerExceptions">The broker exceptions.</param>
        public BrokerExceptionsRateFootnoteFactory(IEnumerable<BrokerException> brokerExceptions)
        {
            this.brokerExceptions = brokerExceptions;
        }

        /// <summary>
        /// Creates a footnote control.
        /// </summary>
        /// <returns>A BrokerExceptionsRateFootnoteControl object.</returns>
        public RateFootnoteControl CreateFootnote()
        {
            return new BrokerExceptionsRateFootnoteControl(brokerExceptions);
        }
    }
}
