using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.UPS.ServiceManager
{
    public interface IUpsServiceMapping
    {
        /// <summary>
        /// Gets or sets the type of the ups service.
        /// </summary>
        /// <value>
        /// The type of the ups service.
        /// </value>
        UpsServiceType UpsServiceType { get; set; }

        /// <summary>
        /// Gets or sets the ship service code.
        /// </summary>
        /// <value>
        /// The ship service code.
        /// </value>
        string ShipServiceCode { get; set; }

        /// <summary>
        /// Gets or sets the rate service code.
        /// </summary>
        /// <value>
        /// The rate service code.
        /// </value>
        string RateServiceCode { get; set; }

        /// <summary>
        /// Gets or sets the transit service code.
        /// </summary>
        /// <value>
        /// The transit service code.
        /// </value>
        string TransitServiceCode { get; set; }

        /// <summary>
        /// Gets or sets the world ship code.
        /// </summary>
        /// <value>
        /// The world ship code.
        /// </value>
        string WorldShipCode { get; set; }

        /// <summary>
        /// Gets or sets the world ship description.
        /// </summary>
        /// <value>
        /// The world ship description.
        /// </value>
        string WorldShipDescription { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is SurePost.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is SurePost; otherwise, <c>false</c>.
        /// </value>
        bool IsSurePost { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is mail innovations.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is mail innovations; otherwise, <c>false</c>.
        /// </value>
        bool IsMailInnovations { get; set; }

        /// <summary>
        /// Gets or sets the destination country code.
        /// </summary>
        /// <value>
        /// The destination country code.
        /// </value>
        string DestinationCountryCode { get; set; }
    }
}