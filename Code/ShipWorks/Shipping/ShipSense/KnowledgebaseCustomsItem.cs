﻿using System;
using System.Security.Cryptography;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipSense.Hashing;

namespace ShipWorks.Shipping.ShipSense
{
    /// <summary>
    /// A DTO of customs information that will be serialized in an entry to the ShipSense knowledge base.
    /// </summary>
    [Serializable]
    public class KnowledgebaseCustomsItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KnowledgebaseCustomsItem"/> class.
        /// </summary>
        public KnowledgebaseCustomsItem()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnowledgebaseCustomsItem"/> class
        /// using the values of the entity provided..
        /// </summary>
        /// <param name="customsItemEntity">The customs item entity.</param>
        public KnowledgebaseCustomsItem(ShipmentCustomsItemEntity customsItemEntity)
        {
            Description = customsItemEntity.Description;
            Quantity = customsItemEntity.Quantity;
            Weight = customsItemEntity.Weight;
            UnitValue = customsItemEntity.UnitValue;
            CountryOfOrigin = customsItemEntity.CountryOfOrigin;
            HarmonizedCode = customsItemEntity.HarmonizedCode;
            NumberOfPieces = customsItemEntity.NumberOfPieces;
            UnitPriceAmount = customsItemEntity.UnitPriceAmount;
        }

        /// <summary>
        /// Gets a hash that uniquely identifies a customs item based on the
        /// other property values of the KnowledgebaseCustomsItem.
        /// </summary>
        public string Hash
        {
            get
            {
                string valueForHashing = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}",
                                                       Description, Quantity, Weight, UnitValue, CountryOfOrigin, HarmonizedCode,
                                                       NumberOfPieces, UnitPriceAmount);

                // Since Description is being used in the hash value, 
                // use SHA256 value to reduce the length of the hash value
                StringHash hashingAlgorithm = new StringHash();
                return hashingAlgorithm.Hash(valueForHashing, string.Empty);
            }
        }

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
