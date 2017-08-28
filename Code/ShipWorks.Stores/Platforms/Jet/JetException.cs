using System;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// Jet specific exception
    /// </summary>
    public class JetException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public JetException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public JetException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public JetException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}