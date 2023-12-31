﻿using Autofac;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;
using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    /// <summary>
    /// Footnote factory for creating Amazon carrier terms and conditions footnote
    /// </summary>
    public class AmazonSFPCarrierTermsAndConditionsNotAcceptedFootnoteFactory : IRateFootnoteFactory
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSFPCarrierTermsAndConditionsNotAcceptedFootnoteFactory(IEnumerable<string> carrierNames)
        {
            MethodConditions.EnsureArgumentIsNotNull(carrierNames, nameof(carrierNames));

            CarrierNames = carrierNames;
        }

        /// <summary>
        /// Gets the corresponding shipment type for the factory.
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.AmazonSFP;

        /// <summary>
        /// Creates a footnote control.
        /// </summary>
        public RateFootnoteControl CreateFootnote(IFootnoteParameters parameters)
        {
            return new AmazonSFPCarrierTermsAndConditionsNotAcceptedFootnoteControl(CarrierNames);
        }

        /// <summary>
        /// Get a view model that represents this footnote
        /// </summary>
        public object CreateViewModel(ICarrierShipmentAdapter shipmentAdapter)
        {
            IAmazonSFPCarrierTermsAndConditionsNotAcceptedFootnoteViewModel viewModel =
                IoC.UnsafeGlobalLifetimeScope.Resolve<IAmazonSFPCarrierTermsAndConditionsNotAcceptedFootnoteViewModel>();

            viewModel.CarrierNames = CarrierNames;
            return viewModel;
        }

        /// <summary>
        /// Notes that this factory should or should not be used in BestRate
        /// For example, when using BestRate, we do not want Usps promo footnotes to display, so this will be set to false.
        /// </summary>
        public bool AllowedForBestRate => false;

        /// <summary>
        /// Carrier names for Terms and Conditions
        /// </summary>
        public IEnumerable<string> CarrierNames { get; }
    }
}
