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
        /// <returns>Task of Unit, where Unit is just a placeholder to let us treat this method
        /// as a Func instead of an Action for easier composition.</returns>
        Task<Unit> Archive(DateTime cutoffDate);
    }
}
