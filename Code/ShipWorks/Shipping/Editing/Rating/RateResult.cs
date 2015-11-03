using System;
using System.Collections.Generic;
using System.Drawing;
using Interapptive.Shared;
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
        bool selectable;
        string days;
        decimal amount;
        decimal? taxes;
        decimal? duties;
        decimal? shipping;

        Image amountFootnote;
        object tag;

        string carrierDescription;

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
        public RateResult(string description, string days) : this()
        {
            this.Description = description;
            this.days = days;

            this.selectable = false;
            this.IsCounterRate = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RateResult(string description, string days, decimal amount, object tag) : this()
        {
            this.Description = description;
            this.days = days;
            this.amount = amount;
            this.tag = tag;

            this.selectable = true;
            this.IsCounterRate = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public RateResult(string description, string days, decimal amount, decimal? duties, decimal? taxes, decimal? shipping, object tag) : 
            this(description, days, amount, tag)
        {
            this.duties = duties;
            this.taxes = taxes;
            this.shipping = shipping;
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
                return Tag is BestRateResultTag ? ((BestRateResultTag)Tag).OriginalTag : Tag;
            }
        }

        /// <summary>
        /// Transit time description could be "2" or "2 - 3 standard" or whatever
        /// </summary>
        public string Days
        {
            get { return days; }
            set { days = value; }
        }

        /// <summary>
        /// The amount the rate will cost the shipper. Only valid if Selectable is true.
        /// </summary>
        public decimal Amount
        {
            get { return amount; }
        }

        /// <summary>
        /// Returns the amount formatted as currency. If there is a half cent, 1/2 is added to the end.
        /// </summary>
        public string FormattedAmount
        {
            get
            {
                return StringUtility.FormatFriendlyCurrency(Amount);
            }
        }

        /// <summary>
        /// The amount of taxes included in the rate
        /// </summary>
        public decimal? Taxes
        {
            get { return taxes; }
        }

        /// <summary>
        /// The amount of duties included in the rate
        /// </summary>
        public decimal? Duties
        {
            get { return duties; }
        }

        /// <summary>
        /// The portion of the amount that goes for shipping
        /// </summary>
        public decimal? Shipping
        {
            get { return shipping; }
        }

        /// <summary>
        /// The image, if any, that will be displayed next to the amount
        /// </summary>
        public Image AmountFootnote
        {
            get { return amountFootnote; }
            set { amountFootnote = value; }
        }

        /// <summary>
        /// Gets or sets the type of the shipment.
        /// </summary>
        /// <value>
        /// The type of the shipment.
        /// </value>
        public ShipmentTypeCode ShipmentType { get; set; }

        /// <summary>
        /// Tag used by the specific service type to store a strongly-typed object representing the rate.  Only valid if Selectable is true.
        /// </summary>
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        /// <summary>
        /// Indicates if this rate should be selected by the user.
        /// </summary>
        public bool Selectable
        {
            get { return selectable; }
            set { selectable = value; }
        }

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
        public string CarrierDescription
        {
            get { return carrierDescription; }
            set { carrierDescription = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this [is a counter rate].
        /// </summary>
        /// <value>
        ///   <c>true</c> if this [is a counter rate]; otherwise, <c>false</c>.
        /// </value>
        public bool IsCounterRate { get; set; }

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
            RateResult copiedRate = new RateResult(Description, days, amount, duties, taxes, shipping, tag)
            {
                AmountFootnote = amountFootnote,
                CarrierDescription = carrierDescription,
                ExpectedDeliveryDate = ExpectedDeliveryDate,
                IsCounterRate = IsCounterRate,
                Selectable = selectable,
                RateID = RateID,
                ServiceLevel = ServiceLevel,
                ShipmentType = ShipmentType,
                ProviderLogo = ProviderLogo
            };

            return copiedRate;
        }
    }
}
