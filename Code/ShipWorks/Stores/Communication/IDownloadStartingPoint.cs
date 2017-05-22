using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Get download starting points
    /// </summary>
    public interface IDownloadStartingPoint
    {
        /// <summary>
        /// Obtains the most recent order date.  If there is none, and the store has an InitialDaysBack policy, it
        /// will be used to calculate the initial number of days back to.
        /// </summary>
        DateTime? OnlineLastModified(IStoreEntity store);

        /// <summary>
        /// Obtains the most recent order date.  If there is none, and the store has an InitialDaysBack policy, it
        /// will be used to calculate the initial number of days back to.
        /// </summary>
        DateTime? OrderDate(IStoreEntity store);
    }
}
