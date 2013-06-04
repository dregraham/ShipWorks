using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.GenericModule
{
    /// <summary>
    /// Exception for a misconfigured Generic Module
    /// </summary>
    public class GenericModuleConfigurationException : GenericStoreException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericModuleConfigurationException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericModuleConfigurationException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericModuleConfigurationException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
