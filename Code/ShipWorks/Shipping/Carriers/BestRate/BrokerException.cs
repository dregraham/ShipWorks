
namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// A ShippingException that is intended to be thrown from IBestRateShippingBroker implementations.
    /// </summary>
    public class BrokerException : ShippingException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BrokerException" /> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="severityLevel">The severity level.</param>
        /// <param name="shipmentType">TThe shipment type.</param>
        public BrokerException(ShippingException innerException, BrokerExceptionSeverityLevel severityLevel, ShipmentType shipmentType)
            : base(string.Empty, innerException)
        {
            SeverityLevel = severityLevel;
            ShipmentType = shipmentType;
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public override string Message
        {
            get { return InnerException.Message; }
        }

        /// <summary>
        /// Gets the severity level of the exception.
        /// </summary>
        public BrokerExceptionSeverityLevel SeverityLevel { get; private set; }

        /// <summary>
        /// Gets the shipment type.
        /// </summary>
        public ShipmentType ShipmentType { get; private set; }
    }
}
