using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Interapptive.Shared.Tests.Utility
{
    /// <summary>
    /// SqlException utility class
    /// </summary>
    public static class SqlExceptionUtility
    {
        /// <summary>
        /// Create a SqlException for testing.
        /// 
        /// http://blog.gauffin.org/2014/08/how-to-create-a-sqlexception/
        /// </summary>
        public static SqlException CreateSqlException(int number, int secondErrorNumber)
        {
            var collectionConstructor = typeof(SqlErrorCollection)
              .GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, //visibility
              null, //binder
              new Type[0],
              null);

            var addMethod = typeof(SqlErrorCollection).GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance);

            var errorCollection = (SqlErrorCollection)collectionConstructor.Invoke(null);

            var errorConstructor = typeof(SqlError).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null,
              new[]
              {
                  typeof (int), typeof (byte), typeof (byte), typeof (string), typeof(string), typeof (string),
                  typeof (int), typeof (uint)
              }, null);

            var error =
              errorConstructor.Invoke(new object[] { number, (byte)0, (byte)0, "server", "errMsg", "proccedure", 100, (uint)0 });
            var error2 =
              errorConstructor.Invoke(new object[] { secondErrorNumber, (byte)0, (byte)0, "server", "errMsg", "proccedure", 100, (uint)0 });

            addMethod.Invoke(errorCollection, new[] { error });
            addMethod.Invoke(errorCollection, new[] { error2 });

            var constructor = typeof(SqlException)
              .GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, //visibility
                null, //binder
                new[] { typeof(string), typeof(SqlErrorCollection), typeof(Exception), typeof(Guid) },
                null); //param modifiers

            return (SqlException)constructor.Invoke(new object[] { "Error message", errorCollection, new DataException(), Guid.NewGuid() });
        }
    }
}
