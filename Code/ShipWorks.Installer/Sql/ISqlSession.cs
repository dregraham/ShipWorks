using System;
using System.Data.Common;

namespace ShipWorks.Installer.Sql
{
    public interface ISqlSession
    {
        SqlSessionConfiguration Configuration { get; set; }

        bool CanConnect();
        DbConnection OpenConnection();
        bool TestConnection();
        bool TestConnection(TimeSpan timeout);
    }
}