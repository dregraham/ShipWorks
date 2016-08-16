using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

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
        /// Initializes a new instance of the <see cref="BrokerExceptionsRateFootnoteFactory" /> class.
        /// </summary>
        /// <param name="shipmentType">The shipment type that the factory is being create for.</param>
        /// <param name="brokerExceptions">The broker exceptions.</param>
        /// <exception cref="System.InvalidOperationException">One or more broker exceptions must be provided.</exception>
        public BrokerExceptionsRateFootnoteFactory(ShipmentTypeCode shipmentTypeCode, IEnumerable<BrokerException> brokerExceptions)
        {
            IEnumerable<BrokerException> exceptions = brokerExceptions as IList<BrokerException> ?? brokerExceptions.ToList();

            if (!exceptions.Any())
            {
                throw new InvalidOperationException("One or more broker exceptions must be provided.");
            }

            ShipmentTypeCode = shipmentTypeCode;
            this.brokerExceptions = exceptions;
        }

        /// <summary>
        /// Gets the corresponding shipment type for the factory.
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode { get; private set; }

        /// <summary>
        /// Notes that this factory should be used in BestRate
        /// </summary>
        public bool AllowedForBestRate
        {
            get { return true; }
        }

        /// <summary>
        /// Creates a footnote control.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>A BrokerExceptionsRateFootnoteControl object.</returns>
        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new BrokerExceptionsRateFootnoteControl(brokerExceptions, GetSeverityLevel());
        }

        /// <summary>
        /// Get a view model that represents this footnote
        /// </summary>
        public object CreateViewModel(ICarrierShipmentAdapter shipmentAdapter)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IBrokerExceptionsRateFootnoteViewModel viewModel = lifetimeScope.Resolve<IBrokerExceptionsRateFootnoteViewModel>();
                viewModel.BrokerExceptions = brokerExceptions;
                viewModel.SeverityLevel = GetSeverityLevel();
                return viewModel;
            }
        }

        private BrokerExceptionSeverityLevel GetSeverityLevel() => brokerExceptions.FirstOrDefault().SeverityLevel;
    }
}
