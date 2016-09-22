using System.Data;
using System.Data.Common;

namespace Interapptive.Shared.Data
{
    /// <summary>
    /// Extensions for DbCommands
    /// </summary>
    public static class DbCommandExtensions
    {
        /// <summary>
        /// Add a parameter to the collection with the given name and value
        /// </summary>
        public static DbParameter AddParameterWithValue(this DbCommand command, string name, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            command.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Add a parameter to the collection with the given name and type
        /// </summary>
        public static DbParameter AddParameter(this DbCommand command, string name, DbType type)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.DbType = type;
            command.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Add a parameter to the collection with the given name and type
        /// </summary>
        public static DbParameter AddParameter(this DbCommand command, string name, DbType type, int size)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.DbType = type;
            parameter.Size = size;
            command.Parameters.Add(parameter);
            return parameter;
        }
    }
}
