using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing.Enums;

namespace ShipWorks.Shipping.Editing.Rating
{
    /// <summary>
    /// Rate object used by the RatesControl, use for universal rate display system
    /// </summary>
    public class RateResult
    {
        //bool selectable;
        //string days;
        //decimal amount;
        //object tag;
        private readonly RateAmountComponents rateAmountComponents;

        /// <summary>
        /// Constructor for tests
        /// </summary>
        protected RateResult()
        {
            RateID = Guid.NewGuid();
        }

        /// <summary>
        /// Constructor, for when an entry is not a selectable rate, but used more as a heading
        /// </summary>
        public RateResult(string description, string days)
        {
            Description = description;
            Days = days;

            Selectable = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RateResult(string description, string days, decimal amount, CurrencyCode currencyCode, object tag)
            : this()
        {
            Description = description;
            Days = days;
            Amount = amount;
            CurrencyCode = currencyCode;
            Tag = tag;

            Selectable = true;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RateResult(string description, string days, decimal amount, object tag) :
            this(description, days, amount, CurrencyCode.USD, tag)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RateResult(string description, string days, decimal amount, RateAmountComponents rateAmountComponents,
            object tag) :
            this(description, days, amount, CurrencyCode.USD, tag)
        {
            this.rateAmountComponents = rateAmountComponents;
        }

        /// <summary>
        /// Value to uniquely identify the rate.
        /// </summary>
        public Guid RateID { get; set; }

        /// <summary>
        /// Gets or sets the service level.
        /// </summary>
        public ServiceLevelType ServiceLevel { get; set; }

        /// <summary>
        /// A description of the rate service class, like "USPS Priority"
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A helper property to get the original tag of a rate to handle cases where the
        /// rate is a cached rate best rate result.
        /// </summary>
        public object OriginalTag
        {
            get
            {
                // Account for the rate being a previously cached rate where the tag is already a best rate tag
                return Tag is BestRateResultTag ? ((BestRateResultTag) Tag).OriginalTag : Tag;
            }
        }

        /// <summary>
        /// Transit time description could be "2" or "2 - 3 standard" or whatever
        /// </summary>
        public string Days { get; set; }

        /// <summary>
        /// The amount the rate will cost the shipper. Only valid if Selectable is true.
        /// </summary>
        public decimal AmountOrDefault => Amount.GetValueOrDefault();

        /// <summary>
        /// The amount the rate will cost the shipper. Only valid if Selectable is true.
        /// </summary>
        public decimal? Amount { get; }

        /// <summary>
        /// The currency that the amount is in
        /// </summary>
        public CurrencyCode CurrencyCode { get; }

        /// <summary>
        /// Returns the amount formatted as currency. If there is a half cent, 1/2 is added to the end.
        /// </summary>
        public string FormattedAmount
        {
            get
            {
                string culture = EnumHelper.GetApiValue(CurrencyCode);
                CultureInfo cultureInfo = CultureInfo.CurrentCulture;

                try
                {
                    cultureInfo = CultureInfo.CreateSpecificCulture(culture);
                }
                catch (CultureNotFoundException)
                {
                    Debug.Fail("Unknown culture for currency");
                }

                return AmountOrDefault.FormatFriendlyCurrency(cultureInfo);
            }
        }


        /// <summary>
        /// The amount of taxes included in the rate
        /// </summary>
        public decimal? Taxes => rateAmountComponents.Taxes;

        /// <summary>
        /// The amount of duties included in the rate
        /// </summary>
        public decimal? Duties => rateAmountComponents.Duties;

        /// <summary>
        /// The portion of the amount that goes for shipping
        /// </summary>
        public decimal? Shipping => rateAmountComponents.Shipping;

        /// <summary>
        /// The image, if any, that will be displayed next to the amount
        /// </summary>
        public Image AmountFootnote { get; set; }

        /// <summary>
        /// Gets or sets the type of the shipment.
        /// </summary>
        public ShipmentTypeCode ShipmentType { get; set; }

        /// <summary>
        /// Tag used by the specific service type to store a strongly-typed object representing the rate.  Only valid if Selectable is true.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Indicates if this rate should be selected by the user.
        /// </summary>
        public bool Selectable { get; set; }

        /// <summary>
        /// Gets or sets the expected delivery date.
        /// </summary>
        public DateTime? ExpectedDeliveryDate { get; set; }

        /// <summary>
        /// Mask the description of the rate, if necessary
        /// </summary>
        /// <param name="rates">Collection of all rates, including this one</param>
        /// <remarks>For the base RateResult class, this method should do nothing</remarks>
        public virtual void MaskDescription(IEnumerable<RateResult> rates)
        {

        }

        /// <summary>
        /// Gets or sets the carrier description.
        /// </summary>
        /// <value>
        /// The carrier description.
        /// </value>
        public string CarrierDescription { get; set; }

        /// <summary>
        /// Gets or sets the provider logo.
        /// </summary>
        /// <value>
        /// The provider logo.
        /// </value>
        public Image ProviderLogo { get; set; }

        /// <summary>
        /// Copies this instance.
        /// </summary>
        public RateResult Copy()
        {
            //Description,days,amount, tag
            RateResult copiedRate = new RateResult(Description, Days, AmountOrDefault, rateAmountComponents, Tag)
            {
                AmountFootnote = AmountFootnote,
                CarrierDescription = CarrierDescription,
                ExpectedDeliveryDate = ExpectedDeliveryDate,
                Selectable = Selectable,
                RateID = RateID,
                ServiceLevel = ServiceLevel,
                ShipmentType = ShipmentType,
                ProviderLogo = ProviderLogo
            };

            return copiedRate;
        }
    }
}
