using System;

namespace ShipWorks.Shipping.ShipSense
{
    /// <summary>
    /// A DTO of customs information that will be serialized in an entry to the ShipSense knowledge base.
    /// </summary>
    [Serializable]
    public class KnowledgebaseCustomsItem
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        public double Quantity { get; set; }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Gets or sets the unit value.
        /// </summary>
        public decimal UnitValue { get; set; }

        /// <summary>
        /// Gets or sets the country of origin.
        /// </summary>
        public string CountryOfOrigin { get; set; }

        /// <summary>
        /// Gets or sets the harmonized code.
        /// </summary>
        public string HarmonizedCode { get; set; }

        /// <summary>
        /// Gets or sets the number of pieces.
        /// </summary>
        public int NumberOfPieces { get; set; }

        /// <summary>
        /// Gets or sets the unit price amount.
        /// </summary>
        public decimal UnitPriceAmount { get; set; }
    }
}
