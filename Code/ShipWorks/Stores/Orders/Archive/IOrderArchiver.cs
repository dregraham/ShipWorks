using System;
using System.Reactive;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// Archive orders
    /// </summary>
    public interface IOrderArchiver
    {
        /// <summary>
        /// Perform the archive
        /// </summary>
        /// <param name="cutoffDate">Date before which orders will be archived</param>
        /// <param name="isManualArchive">True if this is a manual archive, false if automatic</param>
        /// <returns>Task of Unit, where Unit is just a placeholder to let us treat this method
        /// as a Func instead of an Action for easier composition.</returns>
        Task<IResult> Archive(DateTime cutoffDate, bool isManualArchive);
    }
}
