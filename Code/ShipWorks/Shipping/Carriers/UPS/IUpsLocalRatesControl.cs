using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// Represents a Ups Local Rates Control
    /// </summary>
    public interface IUpsLocalRatesControl
    {
        /// <summary>
        /// The DataContext of the control
        /// </summary>
        object DataContet { get; set; }
    }
}
