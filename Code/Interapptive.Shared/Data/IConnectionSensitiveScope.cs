using System;

namespace Interapptive.Shared.Data
{
    /// <summary>
    /// An instance of this class should be created before opening a window or doing an action that could change the database connection, restore, or backup the database.
    /// After the constructor finishes, Acquired must be checked to determine if the action can proceed.
    /// </summary>
    public interface IConnectionSensitiveScope : IDisposable
    {
        /// <summary>
        /// Indicates if the scope was successfully entered. If false, the window or action that could potentially affect the data connection
        /// must not proceed.
        /// </summary>
        bool Acquired { get; }

        /// <summary>
        /// Indicates if the database, or the database we are pointing to via SqlSession, has changed during this scope
        /// </summary>
        bool DatabaseChanged { get; }
    }
}