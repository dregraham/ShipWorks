using System;
using Newtonsoft.Json;

namespace Interapptive.Shared.Messaging
{
    /// <summary>
    /// Defines a message usable with the Messenger
    /// </summary>
    /// <remarks>Right now this is just a marker interface</remarks>
    [JsonObject(MemberSerialization.OptOut)]
    public interface IShipWorksMessage
    {
        /// <summary>
        /// Id of the message used for tracking purposes
        /// </summary>
        Guid MessageId { get; }

        /// <summary>
        /// Source of the message
        /// </summary>
        [JsonIgnore]
        object Sender { get; }
    }
}