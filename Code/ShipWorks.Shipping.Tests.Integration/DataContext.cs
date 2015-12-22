using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Autofac.Extras.Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;

namespace ShipWorks.Shipping.Tests.Integration
{
    /// <summary>
    /// Context that can be used to run a data driven test
    /// </summary>
    public class DataContext : IDisposable
    {
        private static readonly IDictionary<Type, object> defaultValues =
            new Dictionary<Type, object>
            {
                { typeof(byte), 0 },
                { typeof(char), 0 },
                { typeof(int), 0 },
                { typeof(long), 0L },
                { typeof(float), 0F },
                { typeof(double), 0D },
                { typeof(decimal), 0M },
                { typeof(bool), false },
                { typeof(DateTime), new DateTime(2015, 12, 22) },
                { typeof(string), string.Empty }
            };

        private readonly SqlConnection connection;
        private readonly SqlTransaction transaction;

        /// <summary>
        /// Constructor
        /// </summary>
        public DataContext(SqlConnection connection, AutoMock mock)
        {
            this.connection = connection;
            transaction = connection.BeginTransaction();

            mock.Provide<Func<bool, SqlAdapter>>(x => new SqlAdapter(connection, transaction));
        }

        /// <summary>
        /// Create an instance of an entity that has all its fields set to a non-null default
        /// </summary>
        public T CreateEntityWithDefaults<T>() where T : EntityBase2, new()
        {
            T entity = new T();

            foreach (IEntityField2 field in entity.Fields
                .OfType<IEntityField2>()
                .Where(x => !x.IsPrimaryKey)
                .Where(x => !x.IsReadOnly))
            {
                if (defaultValues.ContainsKey(field.DataType))
                {
                    field.CurrentValue = defaultValues[field.DataType];
                    field.IsChanged = true;
                }
            }

            entity.Fields.IsDirty = true;
            entity.IsDirty = true;

            return entity;
        }

        /// <summary>
        /// Create a SQL adapter that is hooked into the current test transaction
        /// </summary>
        public SqlAdapter CreateSqlAdapter() => new SqlAdapter(connection, transaction);

        /// <summary>
        /// Dispose of the context
        /// </summary>
        public void Dispose()
        {
            transaction.Dispose();
            connection.Dispose();
        }
    }
}
