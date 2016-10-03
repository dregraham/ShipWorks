using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// DTO for passing USPS label data around
    /// </summary>
    public class UspsLabelResponse
    {
        /// <summary>
        /// The shipment
        /// </summary>
        public ShipmentEntity Shipment;

        /// <summary>
        /// Byte array of image data
        /// </summary>
        public byte[][] ImageData;

        /// <summary>
        /// Label URL
        /// </summary>
        public string LabelUrl;
    }
}
