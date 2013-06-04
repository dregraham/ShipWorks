using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Editions.Freemium
{
    /// <summary>
    /// Type of endicia account used by freemium
    /// </summary>
    public enum FreemiumAccountType
    {
        /// <summary>
        /// No account has been set yet
        /// </summary>
        None = 0,

        /// <summary>
        /// New LabelServer account for use with ShipWorks
        /// </summary>
        LabelServer = 1,

        /// <summary>
        /// Existing DAZzle account
        /// </summary>
        DAZzle = 2
    }
}
