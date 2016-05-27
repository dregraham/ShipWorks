using Interapptive.Shared.Messaging.TrackedObservable;
using Newtonsoft.Json;

namespace Interapptive.Shared.Messaging.Logging
{
    /// <summary>
    /// Data that represents a Subscribe operation
    /// </summary>
    internal class SubscribeOperation : LogItem, IOperation
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SubscribeOperation(IMessageTracker tracker, object listener, string callerName) : base(tracker)
        {
            Listener = listener;
            CallerName = callerName;
            Endpoint = "operation";
        }

        /// <summary>
        /// Name of the calling method in the listening object
        /// </summary>
        public string CallerName { get; }

        /// <summary>
        /// Object that is listening for the message
        /// </summary>
        [JsonConverter(typeof(ReferenceConverter))]
        public object Listener { get; }

        /// <summary>
        /// Method of the operation (Select, Where, etc.)
        /// </summary>
        public string Method => "Subscribe";

        /// <summary>
        /// Value that was returned from the operation, if any
        /// </summary>
        public object Value => null;
    }
}