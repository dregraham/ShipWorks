using System;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// Base for all exceptions thrown by the OnTrac integration
    /// </summary>
    public class OnTracException : Exception
    {
        /// <summary>
        /// Exception
        /// </summary>
        public OnTracException()
        {
        }

        /// <summary>
        /// Exception
        /// </summary>
        public OnTracException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Exception
        /// </summary>
        public OnTracException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        /// <summary>
        /// Exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="doesNotServiceLocation">Was the exception a result of OnTrac's service area?</param>
        public OnTracException(string message, bool doesNotServiceLocation)
            : base(message)
        {
            DoesNotServiceLocation = doesNotServiceLocation;
        }

        /// <summary>
        /// Was the exception generated because OnTrac does not service an address?
        /// </summary>
        public virtual bool DoesNotServiceLocation { get; protected set; }
    }
}