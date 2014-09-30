using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    public class UspsShipmentType : StampsShipmentType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsShipmentType"/> class.
        /// </summary>
        public UspsShipmentType()
        {
            ShouldRetrieveExpress1Rates = false;

            // Use the "live" versions by default
            AccountRepository = new StampsAccountRepository();
            LogEntryFactory = new LogEntryFactory();
        }

        /// <summary>
        /// The ShipmentTypeCode enumeration value
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get { return ShipmentTypeCode.Usps; }
        }

        /// <summary>
        /// Create the settings control for stamps.com
        /// </summary>
        public override SettingsControlBase CreateSettingsControl()
        {
            return new StampsSettingsControl(StampsResellerType.StampsExpedited);
        }

        /// <summary>
        /// Get postal rates for the given shipment
        /// </summary>
        /// <param name="shipment">Shipment for which to retrieve rates</param>
        protected override RateGroup GetRatesFromApi(ShipmentEntity shipment)
        {
            List<RateResult> stampsRates = new StampsApiSession(AccountRepository, LogEntryFactory, CertificateInspector).GetRates(shipment);
            
            return new RateGroup(stampsRates);
        }
    }
}
