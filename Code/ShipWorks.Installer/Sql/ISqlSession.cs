using System;
using System.Data.Common;

namespace ShipWorks.Installer.Sql
{
    /// <summary>
    /// Interface for SqlSession
    /// </summary>
    public interface ISqlSession
    {
        /// <summary>
        /// The configuration for this session
        /// </summary>
        SqlSessionConfiguration Configuration { get; set; }

        /// <summary>
        /// Returns a flag indicating if a connection can be made to SQL Server.
        /// </summary>
        bool CanConnect();

        /// <summary>
        /// Open a connection using the current properties of the SqlSession
        /// </summary>
        DbConnection OpenConnection();

        /// <summary>
        /// Tries to connect to SQL Server.  Throws an exception on failure.
        /// </summary>
        bool TestConnection();

        /// <summary>
        /// Tries to connect to SQL Server.  Throws an exception on failure.
        /// </summary>
        bool TestConnection(TimeSpan timeout);
    }
}