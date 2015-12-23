using System;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;

namespace ShipWorks.Shipping.Tests.Integration
{
    public interface IEntityBuilder<T> where T : EntityBase2, new()
    {
        T Save(SqlAdapter adapter);

        IEntityBuilder<T> WithDefaults();

        IEntityBuilder<T> Configure(Action<T> configuration);
    }
}