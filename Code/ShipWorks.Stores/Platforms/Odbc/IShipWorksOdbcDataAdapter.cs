using System;
using System.Data.Odbc;

namespace ShipWorks.Stores.Platforms.Odbc
{
    public interface IShipWorksOdbcDataAdapter : IDisposable
    {
        OdbcDataAdapter Adapter { get; }
    }
}