using System.Collections.Generic;

namespace ShipWorks.Shipping.UI.RatingPanel
{
#if DEBUG
    public class TestRatingPanelViewModel : RatingPanelViewModel
    {
        public TestRatingPanelViewModel() : base()
        {
            Rates = new List<RateResultDisplay>
            {
                new RateResultDisplay { Description = "Foo", Days = "1", Rate = "$5.60" },
                new RateResultDisplay { Description = "Bar", Days = "1-2", Rate = "$23.60",
                    Duties = "$1.23", Taxes = "$4.56", Shipping = "$9.12" },
                new RateResultDisplay { Description = "Baz", Days = "2-8", Rate = "$15.60" },
            };

            ShowShipping = true;
            ShowTaxes = true;
            ShowFootnotes = false;
        }
    }
#endif
}
