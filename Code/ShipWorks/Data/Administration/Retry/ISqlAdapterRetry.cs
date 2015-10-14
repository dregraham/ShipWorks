using System;
namespace ShipWorks.Data.Administration.Retry
{
    public interface ISqlAdapterRetry
    {
        void ExecuteWithRetry(Action method);
        void ExecuteWithRetry(Action<ShipWorks.Data.Connection.SqlAdapter> method);
    }
}
