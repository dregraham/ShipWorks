using System;
using System.Data.Odbc;

namespace ShipWorks.Stores.Platforms.Odbc.DataAccess
{
    public interface IShipWorksOdbcDataAdapter : IDisposable
    {
        OdbcDataAdapter Adapter { get; }
    }
}