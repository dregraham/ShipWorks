using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace ShipWorks.Tests
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class ExcelDataAttribute : OdbcDataAttribute
    {
        public ExcelDataAttribute(string path, string worksheet) :
            base($"Provider=Microsoft.ACE.OLEDB.12.0; Data Source={path}; Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';", 
                $"select * from [{worksheet}$]")
        {
            Filepath = path;
            Worksheet = worksheet;
        }

        public string Filepath { get; }

        public string Worksheet { get; }
    }
    
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class CsvDataAttribute : OdbcDataAttribute
    {
        public CsvDataAttribute(string path, string fileName) :
            base($"Provider=Microsoft.ACE.OLEDB.12.0; Data Source={path}; Extended Properties='Text;HDR=YES;FORMAT=Delimited;IMEX=1;';", 
                $"select * from [{fileName}#csv]")
        {

        }
    }
    
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class OdbcDataAttribute : DataAttribute
    {
        public OdbcDataAttribute(string connectionString, string queryString)
        {
            ConnectionString = connectionString;
            QueryString = queryString;
        }

        public string ConnectionString { get; private set; }

        public string QueryString { get; private set; }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod == null)
                throw new ArgumentNullException("testMethod");

            ParameterInfo[] pars = testMethod.GetParameters();
            return DataSource(QueryString, pars.Select(par => par.ParameterType).ToArray());
        }

        IEnumerable<object[]> DataSource(string selectString, Type[] parameterTypes)
        {
            IDataAdapter adapter = new OleDbDataAdapter(selectString, ConnectionString);
            DataSet dataSet = new DataSet();

            try
            {
                adapter.Fill(dataSet);

                foreach (DataRow row in dataSet.Tables[0].Rows)
                    yield return new[] { row };
            }
            finally
            {
                IDisposable disposable = adapter as IDisposable;
                disposable.Dispose();
            }
        }

        static string GetFullFilename(string filename)
        {
            string executable = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            return Path.GetFullPath(Path.Combine(Path.GetDirectoryName(executable), filename));
        }

        static object[] ConvertParameters(object[] values, Type[] parameterTypes)
        {
            object[] result = new object[values.Length];

            for (int idx = 0; idx < values.Length; idx++)
                result[idx] = ConvertParameter(values[idx], idx >= parameterTypes.Length ? null : parameterTypes[idx]);

            return result;
        }

        /// <summary>
        /// Converts a parameter to its destination parameter type, if necessary.
        /// </summary>
        /// <param name="parameter">The parameter value</param>
        /// <param name="parameterType">The destination parameter type (null if not known)</param>
        /// <returns>The converted parameter value</returns>
        static object ConvertParameter(object parameter, Type parameterType)
        {
            if ((parameter is double || parameter is float) &&
                (parameterType == typeof(int) || parameterType == typeof(int?)))
            {
                int intValue;
                string floatValueAsString = parameter.ToString();

                if (Int32.TryParse(floatValueAsString, out intValue))
                    return intValue;
            }

            return parameter;
        }
    }
}
