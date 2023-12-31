﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Shared.System.ComponentModel.DataAnnotations;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl
{
    /// <summary>
    /// View model for use by ShipmentControl
    /// </summary>
    public partial class OtherShipmentViewModel
    {
        private string carrierName;
        private string service;
        private decimal cost;
        private string trackingNumber;

        /// <summary>
        /// Name of the carrier used
        /// </summary>
        [Obfuscation(Exclude = true)]
        [StringLength(50, ErrorMessage = @"Carrier may not be longer than 50 characters.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = @"Carrier is required.")]
        public string CarrierName
        {
            get { return carrierName; }
            set { handler.Set(nameof(CarrierName), ref carrierName, value); }
        }

        /// <summary>
        /// Name of the service used
        /// </summary>
        [Obfuscation(Exclude = true)]
        [StringLength(50, ErrorMessage = @"Service may not be longer than 50 characters.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = @"Service is required.")]
        public string Service
        {
            get { return service; }
            set { handler.Set(nameof(Service), ref service, value); }
        }

        /// <summary>
        /// Cost of the shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        [DecimalCompare(0, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Cost must be greater than or equal to $0.00.")]
        [Range(0, 999999, ErrorMessage = @"Please enter a valid cost.")]
        [Required]
        public decimal Cost
        {
            get { return cost; }
            set { handler.Set(nameof(Cost), ref cost, value); }
        }

        /// <summary>
        /// Tracking number
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string TrackingNumber
        {
            get { return trackingNumber; }
            set { handler.Set(nameof(TrackingNumber), ref trackingNumber, value); }
        }
    }
}
