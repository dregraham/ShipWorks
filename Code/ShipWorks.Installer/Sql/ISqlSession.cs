using System;
using System.Data.Common;
using System.Threading.Tasks;

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
        Task<bool> CanConnect();

        /// <summary>
        /// Open a connection using the current properties of the SqlSession
        /// </summary>
        DbConnection OpenConnection();

        /// <summary>
        /// Tries to connect to SQL Server.  Throws an exception on failure.
        /// </summary>
        Task<bool> TestConnection();

        /// <summary>
        /// Tries to connect to SQL Server.  Throws an exception on failure.
        /// </summary>
        Task<bool> TestConnection(TimeSpan timeout);
    }
}