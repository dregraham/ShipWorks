using System;
using System.Drawing;
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

        /// <summary>
        /// Constructor
        /// </summary>
        public RateFootnoteControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets a value indicating whether [associated with amount footer].
        /// </summary>
        public virtual bool AssociatedWithAmountFooter
        {
            get { return false; }
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
        /// Adds the carrier name text to the text of the control
        /// </summary>
        public virtual void SetCarrierName(string carrierName)
        {
            
        }

        /// <summary>
        /// Adds the carrier name text.
        /// </summary>
        /// <param name="carrierName"></param>
        /// <param name="label">The main message text.</param>
        /// <param name="linkLabel">The link label.</param>
        protected static void AddCarrierNameText(string carrierName, Label label, LinkControl linkLabel)
        {
            if (!String.IsNullOrEmpty(carrierName))
            {
                label.Text = "(" + carrierName + ") " + label.Text;
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
