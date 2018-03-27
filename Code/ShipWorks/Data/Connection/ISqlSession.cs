using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Windows.Forms;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Class for managing the login session and connectivity to SQL Server
    /// </summary>
    public interface ISqlSession
    {
        /// <summary>
        /// The current configuration of the session
        /// </summary>
        SqlSessionConfiguration Configuration { get; }

        /// <summary>
        /// Returns DatabaseGuid of database.
        /// </summary>
        Guid DatabaseIdentifier { get; }

        /// <summary>
        /// Returns a flag indicating if a connection can be made to SQL Server.
        /// </summary>
        bool CanConnect();

        /// <summary>
        /// Checks to see if the user on the current connection has the minimum permissions required to run ShipWorks
        /// </summary>
        bool CheckPermissions(SqlSessionPermissionSet permissionSet, IWin32Window owner);

        /// <summary>
        /// Gets the list of any missing permissions minimally required to run ShipWorks
        /// </summary>
        List<string> DetermineMissingPermissions(SqlSessionPermissionSet permissionSet);

        /// <summary>
        /// Get the machine name of the server that sql server is installed
        /// </summary>
        string GetServerMachineName();

        /// <summary>
        /// Gets the running version of the SQL Server database engine
        /// </summary>
        Version GetServerVersion();

        /// <summary>
        /// Indicates if we are connected to a 64bit instance of SQL Server
        /// </summary>
        bool Is64Bit();

        /// <summary>
        /// Checks to see if CLR is enabled on the server
        /// </summary>
        bool IsClrEnabled();

        /// <summary>
        /// Determines if this session is a connection to a server instance on the local machine
        /// </summary>
        bool IsLocalServer();

        /// <summary>
        /// Indicates if we are connected to an instance of SQL Server 2008 or better
        /// </summary>
        bool IsSqlServer2008OrLater();

        /// <summary>
        /// Open a connection using the current properties of the SqlSession
        /// </summary>
        DbConnection OpenConnection();

        /// <summary>
        /// Open a connection using the current properties of the SqlSession, but with
        /// a timeout based on timeoutInSeconds
        /// </summary>
        DbConnection OpenConnection(int timeoutInSeconds);

        /// <summary>
        /// Saves the state of the SqlSession object and sets it as the current sql session.
        /// </summary>
        void SaveAsCurrent();

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