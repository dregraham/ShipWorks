﻿using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Promotion;
using ShipWorks.Shipping.Carriers.UPS.Promo.API;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.RateFootnotes
{
    /// <summary>
    /// An IRateFootnoteFactory for creating UPS promotion footnotes.
    /// </summary>
    /// <seealso cref="ShipWorks.Shipping.Editing.Rating.IRateFootnoteFactory" />
    public class UpsPromoFootnoteFactory : IRateFootnoteFactory
    {
        private readonly IUpsPromo promo;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsPromoFootnoteFactory(IUpsPromo promo, UpsAccountEntity account)
        {
            this.promo = promo;
            ShipmentTypeCode = account.ShipmentType;
        }

        /// <summary>
        /// Gets the corresponding shipment type for the factory.
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode { get; }

        /// <summary>
        /// Creates a UpsPromoFootnote
        /// </summary>
        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new UpsPromoFootnote(parameters, promo);
        }

        /// <summary>
        /// Get a view model that represents this footnote
        /// </summary>
        public object CreateViewModel(ICarrierShipmentAdapter shipmentAdapter)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IUpsPromoFootnoteViewModel viewModel = lifetimeScope.Resolve<IUpsPromoFootnoteViewModel>();
                viewModel.UpsPromo = promo;
                viewModel.ShipmentAdapter = shipmentAdapter;
                return viewModel;
            }
        }

        /// <summary>
        /// Not for Best Rate
        /// </summary>
        public bool AllowedForBestRate => false;
    }
}
