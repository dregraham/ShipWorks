using System.Data.Common;

namespace Interapptive.Shared.Data
{
    /// <summary>
    /// Extensions for DbConnection
    /// </summary>
    public static class DbConnectionExtensions
    {
        /// <summary>
        /// Create a command with the given text
        /// </summary>
        public static DbCommand CreateCommand(this DbConnection connection, string commandText)
        {
            var command = connection.CreateCommand();
            command.CommandText = commandText;
            return command;
        }
    }
}
