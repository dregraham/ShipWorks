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
        List<RateResult> rates;
        bool outOfDate = false;
        Func<RateFootnoteControl> footnoteCreator;

        /// <summary>
        /// Constructor
        /// </summary>
        public RateGroup(IEnumerable<RateResult> rates)
        {
            this.rates = rates.ToList();
        }

        /// <summary>
        /// Get the rates
        /// </summary>
        public ICollection<RateResult> Rates
        {
            get { return rates; }
        }

        /// <summary>
        /// Indicates if the rates are out of date due to a change in shipment values
        /// </summary>
        public bool OutOfDate
        {
            get { return outOfDate; }
            set { outOfDate = value; }
        }

        /// <summary>
        /// Callback to create a footnote control, if any
        /// </summary>
        public Func<RateFootnoteControl> FootnoteCreator
        {
            get { return footnoteCreator; }
            set { footnoteCreator = value; }
        }
    }
}
