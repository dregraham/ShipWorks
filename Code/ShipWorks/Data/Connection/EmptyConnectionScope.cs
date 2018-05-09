using Interapptive.Shared.Data;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Empty connection scope that returns false for everything
    /// </summary>
    internal class EmptyConnectionScope : IConnectionSensitiveScope
    {
        /// <summary>
        /// Was the scope acquired
        /// </summary>
        public bool Acquired => false;

        /// <summary>
        /// Has the database changed
        /// </summary>
        public bool DatabaseChanged => false;

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {

        }
    }
}