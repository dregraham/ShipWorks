using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// A decorator of the RateGroup intended to be used within the rates panel. This version of a 
    /// rate group was created so we had access to the that the rates were 
    /// retrieved for. A separate class was created to keep the dependency of the
    /// ShipmentEntity out of the original RateGroup since this is only being used
    /// by the RatesPanel.
    /// </summary>
    public class ShipmentRateGroup : RateGroup
    {
        private readonly RateGroup rateGroup;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipmentRateGroup" /> class.
        /// </summary>
        /// <param name="rateGroup">The rate group.</param>
        /// <param name="shipment">The shipment.</param>
        public ShipmentRateGroup(RateGroup rateGroup, ShipmentEntity shipment)
            : base(rateGroup.Rates)
        {
            this.rateGroup = rateGroup;
            Shipment = shipment;
        }

        /// <summary>
        /// Gets or sets the carrier.
        /// </summary>
        public override ShipmentTypeCode Carrier
        {
            // Override the carrier to look at the shipment type of the 
            // shipment; otherwise the provider logo wasn't showing in the
            // rate control when using best-rate.
            get { return (ShipmentTypeCode)Shipment.ShipmentType; }
        }

        /// <summary>
        /// Get the rates
        /// </summary>
        public override List<RateResult> Rates
        {
            get
            {
                return rateGroup.Rates;
            }
        }

        /// <summary>
        /// Gets the footnote factories.
        /// </summary>
        public override IEnumerable<IRateFootnoteFactory> FootnoteFactories
        {
            get { return rateGroup.FootnoteFactories; }
        }

        /// <summary>
        /// Adds a footnote factory to the FootnoteFactories collection.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public override void AddFootnoteFactory(IRateFootnoteFactory factory)
        {
            rateGroup.AddFootnoteFactory(factory);
        }

        /// <summary>
        /// If this rate group should show a row in the grid for "show more rates", this is the rate result to do so.
        /// </summary>
        public override RateResult ShowMoreRateResult
        {
            get { return rateGroup.ShowMoreRateResult; }
            set { rateGroup.ShowMoreRateResult = value; }
        }

        /// <summary>
        /// Gets the shipment that the rate group was created for.
        /// </summary>
        public ShipmentEntity Shipment { get; private set; }
    }
}
