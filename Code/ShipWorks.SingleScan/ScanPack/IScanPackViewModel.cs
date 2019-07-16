using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.SingleScan.ScanPack
{
    /// <summary>
    /// Viewmodel for ScanPackView
    /// </summary>
    public interface IScanPackViewModel
    {
        /// <summary>
        /// Load an order
        /// </summary>
        void Load(string orderNumber);
    }
}
