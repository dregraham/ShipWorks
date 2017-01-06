using Interapptive.Shared.Messaging.TrackedObservable;
using Newtonsoft.Json;

namespace Interapptive.Shared.Messaging.Logging
{
    /// <summary>
    /// Data that represents a Dump operation
    /// </summary>
    internal class DumpOperation : LogItem, IOperation
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DumpOperation(IMessageTracker tracker, object value, object listener, string callerName, long startingTimestamp) :
            base(tracker)
        {
            Value = value;
            Listener = listener;
            CallerName = callerName;
            Endpoint = "operation";
            TimestampStart = startingTimestamp;
        }

        /// <summary>
        /// Object that is listening for the message
        /// </summary>
        [JsonConverter(typeof(ReferenceConverter))]
        public object Listener { get; }

        /// <summary>
        /// Method of the operation (Do, Where, etc.)
        /// </summary>
        public string Method => "Dump";

        /// <summary>
        /// Value that was returned from the operation, if any
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Name of the calling method in the listening object
        /// </summary>
        public string CallerName { get; }

        /// <summary>
        /// Timestamp at which the operation started
        /// </summary>
        public long TimestampStart { get; }
    }
}