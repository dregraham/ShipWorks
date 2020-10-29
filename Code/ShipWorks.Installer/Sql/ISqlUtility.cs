using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace ShipWorks.Installer.Sql
{
    public interface ISqlUtility
    {
        string DefaultInstanceName { get; }
        string ShipWorksSaPassword { get; }

        SqlSessionConfiguration DetermineCredentials(string instance, SqlSessionConfiguration firstTry = null);
        Task<IEnumerable<string>> GetDatabaseDetails(DbConnection con);
        bool ValidateOpenConnection(DbConnection con);
    }
}