using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate.Fake
{
    public class FakeRateFootnoteControl : RateFootnoteControl
    {
        private bool associatedWithAmountFooter;

        public FakeRateFootnoteControl(bool associatedWithAmountFooter)
        {
            this.associatedWithAmountFooter = associatedWithAmountFooter;
        }

        public override bool AssociatedWithAmountFooter
        {
            get 
            {
                return associatedWithAmountFooter;
            }
        }
    }
}
