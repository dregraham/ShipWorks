using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Represents a group of rates retrieve for a shipment
    /// </summary>
    public class RateGroup
    {
        bool outOfDate = false;
        private readonly List<IRateFootnoteFactory> footnoteFactories;

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
        public List<RateResult> Rates { get; private set; }

        /// <summary>
        /// Indicates if the rates are out of date due to a change in shipment values
        /// </summary>
        public bool OutOfDate
        {
            get { return outOfDate; }
            set { outOfDate = value; }
        }

        /// <summary>
        /// Gets the footnote factories.
        /// </summary>
        public IEnumerable<IRateFootnoteFactory> FootnoteFactories
        {
            get { return footnoteFactories; }
        }

        /// <summary>
        /// Adds a footnote factory to the FootnoteFactories collection.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public void AddFootnoteFactory(IRateFootnoteFactory factory)
        {
            footnoteFactories.Add(factory);
        }

        /// <summary>
        /// Gets or sets the carrier.
        /// </summary>
        public ShipmentTypeCode Carrier { get; set; }

    }
}
