using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;

namespace ShipWorks.Shipping.Editing.Rating
{
    /// <summary>
    /// Represents a group of rates retrieve for a shipment
    /// </summary>
    public class RateGroup
    {
        private readonly List<IRateFootnoteFactory> footnoteFactories;

        /// <summary>
        /// Constructor used by mocks, etc.
        /// </summary>
        protected RateGroup()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RateGroup(IEnumerable<RateResult> rates)
        {
            this.Rates = rates.ToList();
            footnoteFactories = new List<IRateFootnoteFactory>();
        }

        /// <summary>
        /// Get the rates
        /// </summary>
        public virtual List<RateResult> Rates { get; private set; }

        /// <summary>
        /// Indicates if the rates are out of date due to a change in shipment values
        /// </summary>
        public virtual bool OutOfDate { get; set; }

        /// <summary>
        /// Gets the footnote factories.
        /// </summary>
        public virtual IEnumerable<IRateFootnoteFactory> FootnoteFactories
        {
            get { return footnoteFactories; }
        }


        /// <summary>
        /// Gets or sets the carrier.
        /// </summary>
        public virtual ShipmentTypeCode Carrier { get; set; }

        /// <summary>
        /// Adds a footnote factory to the FootnoteFactories collection.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public virtual void AddFootnoteFactory(IRateFootnoteFactory factory)
        {
            footnoteFactories.Add(factory);
        }

        /// <summary>
        /// Creates a new rate group by copying the current group settings and replacing the rates with the passed in rates
        /// </summary>
        public virtual RateGroup CopyWithRates(IEnumerable<RateResult> rates)
        {
            RateGroup newRateGroup = new RateGroup(rates)
            {
                Carrier = Carrier,
                OutOfDate = OutOfDate
            };

            foreach (IRateFootnoteFactory factory in FootnoteFactories)
            {
                newRateGroup.AddFootnoteFactory(factory);
            }

            return newRateGroup;
        }

        /// <summary>
        /// A factory method to create a rate group indicating that a shipping account is required.
        /// </summary>
        /// <returns>A RateGroup without any rate results and an informational footnote.</returns>
        public static RateGroup ShippingAccountRequiredRateGroup(ShipmentTypeCode shipmentTypeCode)
        {
            RateGroup group = new RateGroup(new List<RateResult>());
            group.AddFootnoteFactory(new ShippingAccountRequiredForRatingFootnoteFactory(shipmentTypeCode));

            return group;
        }
    }
}
