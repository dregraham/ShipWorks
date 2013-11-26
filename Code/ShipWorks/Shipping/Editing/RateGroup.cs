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
        
        /// <summary>
        /// Constructor
        /// </summary>
        public RateGroup(IEnumerable<RateResult> rates)
        {
            this.Rates = rates.ToList();
            FootnoteCreators = new List<Func<RateFootnoteControl>>();
        }

        /// <summary>
        /// Get the rates
        /// </summary>
        public List<RateResult> Rates { get; set; }

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
        public List<Func<RateFootnoteControl>> FootnoteCreators { get; set; }
    }
}
