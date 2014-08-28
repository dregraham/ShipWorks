using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping;

namespace ShipWorks.Editions
{
    /// <summary>
    /// Encapsulates
    /// </summary>
    public class EditionSharedOptions
    {
        private List<ShipmentTypeCode> disabledShipmentTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditionSharedOptions"/> class.
        /// </summary>
        public EditionSharedOptions()
        {
            disabledShipmentTypes = new List<ShipmentTypeCode>();
        }

        /// <summary>
        /// Indicates if DHL is enabled and allowed for the edition
        /// </summary>
        public bool EndiciaDhlEnabled { get; set; }

        /// <summary>
        /// Indicates if insurance is enabled and allowed for the edition
        /// </summary>
        public bool EndiciaInsuranceEnabled { get; set; }

        /// <summary>
        /// Indicates whether UPS SurePost is enabled and allowed for the edition
        /// </summary>
        /// <value><c>true</c> if UPS SurePost is enabled; otherwise, <c>false</c>.</value>
        public bool UpsSurePostEnabled { get; set; }

        /// <summary>
        /// Indicates whether consolidator support is enabled for the customer's endicia account
        /// </summary>
        public bool EndiciaConsolidatorEnabled { get; set; }

        /// <summary>
        /// Indicates whether scan based payment returns support is enabled for the customer's endicia account
        /// </summary>
        public bool EndiciaScanBasedReturnEnabled { get; set; }

        /// <summary>
        /// Indicates shipment types that have been disabled in ShipWorks.
        /// </summary>
        IEnumerable<ShipmentTypeCode> DisabledShipmentTypes
        {
            get { return disabledShipmentTypes; }
        }
    }
}
