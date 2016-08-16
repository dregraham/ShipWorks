using Interapptive.Shared.Messaging.TrackedObservable;
using Newtonsoft.Json;

namespace Interapptive.Shared.Messaging.Logging
{
    /// <summary>
    /// Data that represents a Select operation
    /// </summary>
    internal class SelectOperation : LogItem, IOperation
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SelectOperation(IMessageTracker tracker, object value, object listener, string callerName) :
            base(tracker)
        {
            Value = value;
            Listener = listener;
            CallerName = callerName;
            Endpoint = "operation";
        }

        /// <summary>
        /// Object that is listening for the message
        /// </summary>
        [JsonConverter(typeof(ReferenceConverter))]
        public object Listener { get; }

        /// <summary>
        /// Method of the operation (Select, Where, etc.)
        /// </summary>
        public string Method => "Select";

        /// <summary>
        /// Value that was returned from the operation, if any
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Name of the calling method in the listening object
        /// </summary>
        public string CallerName { get; }
    }
}