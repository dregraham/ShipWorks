using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.UI.RatingPanel
{
    /// <summary>
    /// Generic control that will display the WPF version of the footnote controls in Windoes Forms
    /// </summary>
    [KeyedComponent(typeof(RateFootnoteControl), AmazonSameDayNotAvailableFootnoteFactory.ControlKey)]
    public partial class GenericRateFootnoteControl : RateFootnoteControl
    {
        /// <summary>
        /// Constructor for the VS designer
        /// </summary>
        protected GenericRateFootnoteControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericRateFootnoteControl(object footnoteViewModel) : this()
        {
            elementHost1.Child = new RateFootnoteControlContainer
            {
                DataContext = footnoteViewModel
            };
        }
    }
}
