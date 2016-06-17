using System;
using System.Windows.Forms;

namespace ShipWorks.Shipping.Editing.Rating
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
            RateCriteriaChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
