using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Base class for footnotes
    /// </summary>
    public partial class RateFootnoteControl : UserControl
    {
        public event EventHandler RateCriteriaChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public RateFootnoteControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Raise the RateCriteriaChanged event
        /// </summary>
        protected void RaiseRateCriteriaChanged()
        {
            if (RateCriteriaChanged != null)
            {
                RateCriteriaChanged(this, EventArgs.Empty);
            }
        }
    }
}
