using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.Collections;
using Xunit.Sdk;
using Syncfusion.XlsIO;

namespace ShipWorks.Tests
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class ExcelDataAttribute : DataAttribute
    {
        public ExcelDataAttribute(string path, string worksheet = "") 
        {
            Filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);

            if (!File.Exists(Filepath))
            {
                path = $@"..\..\{path}";
                Filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }

            Worksheet = worksheet;
        }

        public string Filepath { get; }

        public string Worksheet { get; }


        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            ParameterInfo[] pars = testMethod.GetParameters();
            return DataSource();
        }

        private IEnumerable<object[]> DataSource()
        {

            using (Stream fileStream = File.OpenRead(Filepath))
            {
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IWorkbook workbook = excelEngine.Excel.Workbooks.Open(fileStream);
                    var worksheet = workbook.Worksheets.FirstOrDefault(w => w.Name == Worksheet);
                    if (worksheet == null)
                    {
                        worksheet = workbook.Worksheets[0];
                    }

                    var dataTable = worksheet.ExportDataTable(worksheet.UsedRange, ExcelExportDataTableOptions.ColumnNames | ExcelExportDataTableOptions.PreserveOleDate);

                    foreach (DataRow row in dataTable.Select().Where(x => x.ItemArray.Any(y => y != null && y != DBNull.Value)))
                        yield return new[] { row };
                }
            }
        }
    }
}
