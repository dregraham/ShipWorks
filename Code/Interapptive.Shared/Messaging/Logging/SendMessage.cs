using Newtonsoft.Json;

namespace Interapptive.Shared.Messaging.Logging
{
    /// <summary>
    /// Data that represents a call to send message
    /// </summary>
    public class SendMessage : LogItem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SendMessage(IShipWorksMessage message, string method) : base(message)
        {
            Sender = message.Sender;
            Payload = message;
            Message = message;
            Method = method;
            Endpoint = "sendmessage";
        }

        /// <summary>
        /// Sender of the message
        /// </summary>
        [JsonConverter(typeof(ReferenceConverter))]
        public object Sender { get; }

        /// <summary>
        /// Message that was sent
        /// </summary>
        [JsonConverter(typeof(TypeConverter))]
        public object Message { get; }

        /// <summary>
        /// Method that sent the message
        /// </summary>
        public string Method { get; }

        /// <summary>
        /// Payload of the message
        /// </summary>
        public object Payload { get; }
    }
}
