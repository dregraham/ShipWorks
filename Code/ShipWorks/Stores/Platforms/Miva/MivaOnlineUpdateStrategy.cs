using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ShipWorks.Stores.Platforms.Miva
{
    /// <summary>
    /// Strategy for pushing online updates to Miva
    /// </summary>
    public enum MivaOnlineUpdateStrategy
    {
        /// <summary>
        /// No online update supported
        /// </summary>
        None = 0,

        /// <summary>
        /// Online update supported through sebenza ultimate order status
        /// </summary>
        Sebenza = 1,

        /// <summary>
        /// Miva "Womat" supports native Order statuses
        /// </summary>
        MivaNative = 2
    }
}
