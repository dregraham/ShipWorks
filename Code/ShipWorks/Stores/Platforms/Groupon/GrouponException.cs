using System;

namespace ShipWorks.Stores.Platforms.Groupon
{
    /// <summary>
    /// Handleable exception thrown by Generic store and derivatives
    /// </summary>
    public class GrouponException : Exception
    {
        public GrouponException()
        {

        }

        public GrouponException(string message)
            : base(message)
        {

        }

        public GrouponException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
