using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Adapter interface for ShipmentCustomsItemEntities
    /// </summary>
    public interface IShipmentCustomsItemAdapter : INotifyPropertyChanged
    {
        /// <summary>
        /// The backing ShipmentCustomsItemEntity
        /// </summary>
        [Obfuscation(Exclude = true)]
        ShipmentCustomsItemEntity ShipmentCustomsItemEntity { get; }

        /// <summary>
        /// The ShipmentCustomsItemID
        /// </summary>
        [Obfuscation(Exclude = true)]
        long ShipmentCustomsItemID { get; }

        /// <summary>
        /// The ShipmentEntity
        /// </summary>
        [Obfuscation(Exclude = true)]
        ShipmentEntity Shipment { get; }

        /// <summary>
        /// The Description
        /// </summary>
        [Obfuscation(Exclude = true)]
        string Description { get; set; }

        /// <summary>
        /// The Quantity
        /// </summary>
        [Obfuscation(Exclude = true)]
        double Quantity { get; set; }

        /// <summary>
        /// The weight
        /// </summary>
        [Obfuscation(Exclude = true)]
        double Weight { get; set; }

        /// <summary>
        /// The unit value
        /// </summary>
        [Obfuscation(Exclude = true)]
        decimal UnitValue { get; set; }

        /// <summary>
        /// The country of origin
        /// </summary>
        [Obfuscation(Exclude = true)]
        string CountryOfOrigin { get; set; }

        /// <summary>
        /// The harmonized code
        /// </summary>
        [Obfuscation(Exclude = true)]
        string HarmonizedCode { get; set; }

        /// <summary>
        /// The number of pices
        /// </summary>
        [Obfuscation(Exclude = true)]
        int NumberOfPieces { get; set; }

        /// <summary>
        /// The unit price amount
        /// </summary>
        [Obfuscation(Exclude = true)]
        decimal UnitPriceAmount { get; set; }
    }
}
