using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.UPS.ServiceManager
{
    /// <summary>
    /// DTO class that contains mappings between UpsServiceType and Ship, Rate, and WorldShip codes and other properties.
    /// </summary>
    public class UpsServiceMapping
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsServiceMapping"/> class.
        /// </summary>
        /// <param name="upsServiceType">Type of the ups service.</param>
        /// <param name="destinationCountryCode">The destination country code.</param>
        /// <param name="rateServiceCode">The rate service code.</param>
        /// <param name="shipServiceCode">The ship service code.</param>
        /// <param name="transitServiceCode">The transit service code.</param>
        /// <param name="worldShipCode">The world ship code.</param>
        /// <param name="worldShipDescription">The world ship description.</param>
        /// <param name="isMailInnovations">if set to <c>true</c> [is mail innovations].</param>
        /// <param name="isSurePost">if set to <c>true</c> [is sure post].</param>
        [NDependIgnoreTooManyParams]
        public UpsServiceMapping(UpsServiceType upsServiceType, string destinationCountryCode,
                                                   string rateServiceCode, string shipServiceCode, string transitServiceCode, string worldShipCode,
                                                   string worldShipDescription, bool isMailInnovations, bool isSurePost)
        {
            UpsServiceType = upsServiceType;
            DestinationCountryCode = destinationCountryCode;
            RateServiceCode = rateServiceCode;
            ShipServiceCode = shipServiceCode;
            TransitServiceCode = transitServiceCode;
            WorldShipCode = worldShipCode;
            WorldShipDescription = worldShipDescription;
            IsMailInnovations = isMailInnovations;
            IsSurePost = isSurePost;
        }

        /// <summary>
        /// Gets or sets the type of the ups service.
        /// </summary>
        /// <value>
        /// The type of the ups service.
        /// </value>
        public UpsServiceType UpsServiceType { get; set; }

        /// <summary>
        /// Gets or sets the ship service code.
        /// </summary>
        /// <value>
        /// The ship service code.
        /// </value>
        public string ShipServiceCode { get; set; }

        /// <summary>
        /// Gets or sets the rate service code.
        /// </summary>
        /// <value>
        /// The rate service code.
        /// </value>
        public string RateServiceCode { get; set; }

        /// <summary>
        /// Gets or sets the transit service code.
        /// </summary>
        /// <value>
        /// The transit service code.
        /// </value>
        public string TransitServiceCode { get; set; }

        /// <summary>
        /// Gets or sets the world ship code.
        /// </summary>
        /// <value>
        /// The world ship code.
        /// </value>
        public string WorldShipCode { get; set; }

        /// <summary>
        /// Gets or sets the world ship description.
        /// </summary>
        /// <value>
        /// The world ship description.
        /// </value>
        public string WorldShipDescription { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is SurePost.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is SurePost; otherwise, <c>false</c>.
        /// </value>
        public bool IsSurePost { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is mail innovations.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is mail innovations; otherwise, <c>false</c>.
        /// </value>
        public bool IsMailInnovations { get; set; }

        /// <summary>
        /// Gets or sets the destination country code.
        /// </summary>
        /// <value>
        /// The destination country code.
        /// </value>
        public string DestinationCountryCode { get; set; }
    }
}
