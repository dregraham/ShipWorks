using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Default manipulator of shipment date
    /// </summary>
    [Component(RegistrationType.Self)]
    public class DefaultShipmentDateManipulator : IShipmentDateManipulator
    {
        private readonly IDateTimeProvider dateTimeProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public DefaultShipmentDateManipulator(IDateTimeProvider dateTimeProvider)
        {
            this.dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// Manipulate the date of the given shipment
        /// </summary>
        public void Manipulate(ShipmentEntity shipment)
        {
            var now = dateTimeProvider.Now;

            if (!shipment.Processed && shipment.ShipDate.Date < now.Date)
            {
                shipment.ShipDate = now.Date.AddHours(12);
            }
        }
    }
}
