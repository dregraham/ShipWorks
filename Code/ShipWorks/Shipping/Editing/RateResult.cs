using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ShipWorks.Shipping.Editing.Enums;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Rate object used by the RatesControl, use for universal rate display system
    /// </summary>
    public class RateResult
    {
        bool selectable;
        string days;
        decimal amount;

        Image amountFootnote;
        object tag;

        string carrierDescription;

        /// <summary>
        /// Constructor for tests
        /// </summary>
        protected RateResult()
        {
            
        }

        /// <summary>
        /// Constructor, for when an entry is not a selectable rate, but used more as a heading
        /// </summary>
        public RateResult(string description, string days)
        {
            this.Description = description;
            this.days = days;

            this.selectable = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RateResult(string description, string days, decimal amount, object tag)
        {
            this.Description = description;
            this.days = days;
            this.amount = amount;
            this.tag = tag;

            this.selectable = true;
        }

        /// <summary>
        /// Gets or sets the service level.
        /// </summary>
        public ServiceLevelType ServiceLevel { get; set; }

        /// <summary>
        /// A description of the rate service class, like "USPS Priority"
        /// </summary>
        public string Description { get; set; }

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



    }
}
