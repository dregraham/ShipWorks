using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Core.Shipping;

namespace ShipWorks.Shipping.Configuration
{
    /// <summary>
    /// Tracked duration event for carrier
    /// </summary>
    [Component(RegistrationType.SpecificService, Service = typeof(ICarrierSettingsTrackedDurationEvent))]
    public class CarrierSettingsTrackedDurationEvent : TrackedDurationEvent, ICarrierSettingsTrackedDurationEvent
    {
        private readonly string formattedName;
        private string shipmentTypeCode;

        /// <summary>
        /// Constructor
        /// </summary>
        public CarrierSettingsTrackedDurationEvent() : base(string.Empty)
        {
            this.formattedName = "Carrier.{0}.Setup";
        }

        /// <summary>
        /// Record configuration for a carrier
        /// </summary>
        public void RecordConfiguration(ShipmentTypeCode carrier) =>
            shipmentTypeCode = EnumHelper.GetDescription(carrier);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            string shipmentType = string.IsNullOrWhiteSpace(shipmentTypeCode) ? "Unknown" : shipmentTypeCode;
            string eventName = string.Format(formattedName, shipmentType);

            ChangeName(eventName);
            base.Dispose();
        }
    }
}
