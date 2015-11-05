using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    public class AmazonNotLinkedFootnoteFactory : IRateFootnoteFactory
    {
        private readonly string accountTypeToDisplay;

        public AmazonNotLinkedFootnoteFactory(string accountTypeToDisplay)
        {
            this.accountTypeToDisplay = accountTypeToDisplay;
        }

        public ShipmentTypeCode ShipmentTypeCode=> ShipmentTypeCode.Amazon;


        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new AmazonUspsNotLinkedFootnoteControl(accountTypeToDisplay);
        }

        /// <summary>
        /// Notes that this factory should or should not be used in BestRate
        /// For example, when using BestRate, we do not want Usps promo footnotes to display, so this will be set to false.
        /// </summary>
        public bool AllowedForBestRate => true;
    }
    
}
