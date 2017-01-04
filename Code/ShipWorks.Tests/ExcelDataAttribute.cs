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

                foreach (DataRow row in dataSet.Tables[0].Rows.OfType<DataRow>().Where(x => x.ItemArray.Any(y => y != null && y != DBNull.Value)))
                    yield return new[] { row };
            }
            finally
            {
                IDisposable disposable = adapter as IDisposable;
                disposable.Dispose();
            }
        }
    }
}
