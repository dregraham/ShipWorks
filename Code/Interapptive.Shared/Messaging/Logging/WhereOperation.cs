using Interapptive.Shared.Messaging.TrackedObservable;

namespace Interapptive.Shared.Messaging.Logging
{
    /// <summary>
    /// Data associated with a Where operation
    /// </summary>
    internal class WhereOperation : LogItem, IOperation
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WhereOperation(IMessageTracker tracker, object value, object listener, string callerName) :
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
        public object Listener { get; }

        /// <summary>
        /// Method of the operation (Select, Where, etc.)
        /// </summary>
        public string Method => "Where";

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