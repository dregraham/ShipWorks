using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Base class for footnotes
    /// </summary>
    public partial class RateFootnoteControl : UserControl
    {
        public event EventHandler RateCriteriaChanged;
        public string CarrierName = "";

        /// <summary>
        /// Constructor
        /// </summary>
        public RateFootnoteControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RateFootnoteControl(string carrierName)
        {
            this.CarrierName = carrierName;

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

        /// <summary>
        /// Adds the carrier name text.
        /// </summary>
        /// <param name="label">The main message text.</param>
        /// <param name="linkLabel">The link label.</param>
        protected void AddCarrierNameText(Label label, LinkControl linkLabel)
        {
            if (!String.IsNullOrEmpty(CarrierName))
            {
                label.Text = "(" + CarrierName + ") " + label.Text;
                // Resize the label to fit the text
                label.AutoSize = true;

                if (linkLabel != null)
                {
                    // Move the link to accomodate for bigger label text
                    linkLabel.Location = new Point(label.Location.X + label.Size.Width, label.Location.Y);
                }
            }
        }
    }
}
