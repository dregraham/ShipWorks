using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.UI.Carriers.Ups
{
    /// <summary>
    /// Interaction logic for UpsLocalRatesControl.xaml
    /// </summary>
    [Component]
    public partial class UpsLocalRatingControl : IUpsLocalRatingControl
    {
        public UpsLocalRatingControl()
        {
            InitializeComponent();
        }
    }
}
