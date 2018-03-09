using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShipWorks.Archiving
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
        Task<Unit> Archive(DateTime cutoffDate);
    }
}
