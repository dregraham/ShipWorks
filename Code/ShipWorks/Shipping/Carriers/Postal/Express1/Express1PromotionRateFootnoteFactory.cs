﻿using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Creates Express1 Rate Promotion footnote controls
    /// </summary>
    public class Express1PromotionRateFootnoteFactory : IRateFootnoteFactory
    {
        private readonly IExpress1SettingsFacade express1Settings;

        /// <summary>
        /// Construct a new Express1PromotionRateFootnoteFactory object
        /// </summary>
        /// <param name="shipmentType">Type of shipment that instantiated this factory</param>
        /// <param name="express1Settings">Settings that will be used when creating the footnote control</param>
        public Express1PromotionRateFootnoteFactory(ShipmentTypeCode shipmentTypeCode, IExpress1SettingsFacade express1Settings)
        {
            ShipmentTypeCode = shipmentTypeCode;
            this.express1Settings = express1Settings;
        }

        /// <summary>
        /// Gets the carrier to which this footnote is associated
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode { get; private set; }

        /// <summary>
        /// Notes that this factory should not be used in BestRate
        /// </summary>
        public bool AllowedForBestRate
        {
            get { return false; }
        }

        /// <summary>
        /// Create an Express1 rate promotion control
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public RateFootnoteControl CreateFootnote(IFootnoteParameters parameters)
        {
            return new Express1RatePromotionFootnote(express1Settings);
        }

        /// <summary>
        /// Get a view model that represents this footnote
        /// </summary>
        public object CreateViewModel(ICarrierShipmentAdapter shipmentAdapter)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IExpress1RatePromotionFootnoteViewModel viewModel = lifetimeScope.Resolve<IExpress1RatePromotionFootnoteViewModel>();
                viewModel.Settings = express1Settings;
                viewModel.ShipmentAdapter = shipmentAdapter;
                return viewModel;
            }
        }
    }
}
