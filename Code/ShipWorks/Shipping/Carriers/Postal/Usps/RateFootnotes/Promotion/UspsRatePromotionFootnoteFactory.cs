using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Promotion
{
    /// <summary>
    /// An IRateFootnoteFactory for creating USPS promotion footnotes.
    /// </summary>
    public class UspsRatePromotionFootnoteFactory : IRateFootnoteFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsRatePromotionFootnoteFactory" /> class.
        /// </summary>
        /// <param name="shipmentType">Type of the shipment.</param>
        /// <param name="shipment">The shipment.</param>
        /// <param name="showSingleAccountDialog">if set to <c>true</c> [show single account dialog].</param>
        public UspsRatePromotionFootnoteFactory(ShipmentType shipmentType, ShipmentEntity shipment, bool showSingleAccountDialog) :
            this(shipmentType.ShipmentTypeCode, shipment, showSingleAccountDialog)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsRatePromotionFootnoteFactory" /> class.
        /// </summary>
        /// <param name="shipmentTypeCode">Type of the shipment.</param>
        /// <param name="shipment">The shipment.</param>
        /// <param name="showSingleAccountDialog">if set to <c>true</c> [show single account dialog].</param>
        public UspsRatePromotionFootnoteFactory(ShipmentTypeCode shipmentTypeCode, ShipmentEntity shipment, bool showSingleAccountDialog)
        {
            ShipmentTypeCode = shipmentTypeCode;
            Shipment = shipment;
            ShowSingleAccountDialog = showSingleAccountDialog;
        }

        /// <summary>
        /// Gets the corresponding shipment type for the factory.
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode { get; private set; }

        /// <summary>
        /// Gets or sets the shipment.
        /// </summary>
        private ShipmentEntity Shipment { get; set; }

        /// <summary>
        /// Notes that this factory should not be used in BestRate
        /// </summary>
        public bool AllowedForBestRate
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether to [show single account dialog].
        /// </summary>
        public bool ShowSingleAccountDialog { get; private set; }

        /// <summary>
        /// Creates a footnote control.
        /// </summary>
        /// <param name="parameters">Parameters that allow footnotes to interact with the rates grid</param>
        /// <returns>A instance of a RateFoonoteControl.</returns>
        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new UspsRatePromotionFootnote(Shipment, ShowSingleAccountDialog);
        }

        /// <summary>
        /// Get a view model that represents this footnote
        /// </summary>
        public object CreateViewModel(ICarrierShipmentAdapter shipmentAdapter)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IUspsRatePromotionFootnoteViewModel viewModel = lifetimeScope.Resolve<IUspsRatePromotionFootnoteViewModel>();
                viewModel.ShipmentAdapter = shipmentAdapter;
                viewModel.ShowSingleAccountDialog = ShowSingleAccountDialog;
                return viewModel;
            }
        }
    }
}
