using Newtonsoft.Json;

namespace Interapptive.Shared.Messaging.Logging
{
    /// <summary>
    /// Data associated with logging an operation
    /// </summary>
    internal interface IOperation : ILogItem
    {
        /// <summary>
        /// Object that is listening for the message
        /// </summary>
        [JsonConverter(typeof(ReferenceConverter))]
        object Listener { get; }

        /// <summary>
        /// Method of the operation (Select, Where, etc.)
        /// </summary>
        string Method { get; }

        /// <summary>
        /// Value that was returned from the operation, if any
        /// </summary>
        object Value { get; }

        /// <summary>
        /// Name of the calling method in the listening object
        /// </summary>
        string CallerName { get; }
    }
}