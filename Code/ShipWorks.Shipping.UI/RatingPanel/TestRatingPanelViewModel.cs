using System.Collections.Generic;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.UI.RatingPanel
{
#if DEBUG
    public class TestRatingPanelViewModel : RatingPanelViewModel
    {
        public TestRatingPanelViewModel()
        {
            Rates = new List<RateResult>
            {
                new RateResult("Foo", "1", 5.6M, null),
                new RateResult("Bar", "1-2", 23.605M, new RateAmountComponents(1.23M, 4.56M, 9.12M), null),
                new RateResult("Baz", "2-8", 15.6M, null),
            };

            ShowShipping = true;
            ShowTaxes = true;
            ShowFootnotes = false;
        }
    }
#endif
}
