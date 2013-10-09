using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Common.Logging;
using Common.Logging.Simple;
using Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using Moq;


namespace ShipWorks.Tests.Integration.MSTest
{
    [TestClass]
    public class IntegrationTestBase
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        public bool PopulatTestObject<T>(T testObject, List<ColumnPropertyMapDefinition> columnPropertyMap)
        {
            DataRow testDataRow = TestContext.DataRow;
            int rowIndex = testDataRow.Table.Rows.IndexOf(testDataRow);
            System.Diagnostics.Debug.WriteLine(rowIndex);

            if (rowIndex == 0)
            {
                PopulateTranslationMap(TestContext.DataConnection.Database, "US Grn Dom", columnPropertyMap);
                return false;
            }
            
            if (rowIndex == 1)
            {
                return false;
            }

            PopulateValues(testObject, testDataRow, columnPropertyMap);
            return true;
        }

        private void PopulateTranslationMap(string fileName, string tabName, List<ColumnPropertyMapDefinition> columnPropertyMap )
        {
            FileStream workBook = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(workBook);
            reader.IsFirstRowAsColumnNames = false;

            DataSet dataset = reader.AsDataSet();

            DataTable dataTable = dataset.Tables[tabName];

            foreach (DataColumn dataColumn in dataTable.Columns)
            {
                DataRow dataRow1 = dataTable.Rows[0];
                DataRow dataRow2 = dataTable.Rows[1];

                string columnName = dataRow1[dataColumn].ToString().Trim();
                if (!string.IsNullOrWhiteSpace(dataRow2[dataColumn].ToString().Trim()))
                {
                    if (!string.IsNullOrWhiteSpace(columnName))
                    {
                        columnName += "." + dataRow2[dataColumn].ToString().Trim();
                    }
                    else
                    {
                        columnName = dataRow2[dataColumn].ToString().Trim();    
                    }
                }
                else if (!string.IsNullOrWhiteSpace(dataRow2[dataColumn].ToString().Trim()))
                {
                    columnName = dataRow2[dataColumn].ToString().Trim();
                }

                if (string.IsNullOrWhiteSpace(columnName))
                {
                    continue;
                }

                ColumnPropertyMapDefinition columnPropertyMapDefinition = columnPropertyMap.FirstOrDefault(cpm => cpm.SpreadsheetColumnName.ToLower() == columnName.ToLower());

                if (columnPropertyMapDefinition != null)
                {
                    columnPropertyMapDefinition.SpreadsheetColumnIndex = dataTable.Columns.IndexOf(dataColumn);
                }
                else
                {
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition
                        {
                            SpreadsheetColumnName = columnName,
                            PropertyName = "",
                            SpreadsheetColumnIndex = -1
                        });
                }
            }
        }

        public void GenerateColumnPropertyListCode(string tabName, List<ColumnPropertyMapDefinition> columnPropertyMap)
        {
            StringBuilder populationCode = new StringBuilder();
            if (columnPropertyMap.Any(cpm => cpm.SpreadsheetColumnIndex == -1))
            {
                string populationCodeFormat = @"columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = {0},PropertyName = "",SpreadsheetColumnIndex = -1});";

                foreach (ColumnPropertyMapDefinition cpm in columnPropertyMap.Where(cpm => cpm.SpreadsheetColumnIndex == -1).OrderBy(cpm => cpm.SpreadsheetColumnName).ToList())
                {
                    populationCode.AppendLine("columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = \""
                        + cpm.SpreadsheetColumnName
                        + "\",PropertyName = \"\",SpreadsheetColumnIndex = -1});")
                    ;
                }
            }

            Debug.Write(populationCode.ToString());
        }

        /// <summary>
        /// Iterates through each property, in our ShipWorks MSTest objects, setting each to null
        /// </summary>
        private void PopulateValues<T>(T testObject, DataRow testDataRow, List<ColumnPropertyMapDefinition> columnPropertyMap)
        {
            PropertyInfo[] properties = (from c in testObject.GetType().GetProperties()
                                         where c.DeclaringType.FullName.ToUpperInvariant().Contains("SHIPWORKS")
                                            && c.Name.ToUpperInvariant() != "MagicKeysDown".ToUpperInvariant()
                                            && c.Name.ToUpperInvariant() != "DebugKeysDown".ToUpperInvariant()
                                         select c).ToArray();
            
            string missingProperties = string.Empty;

            foreach (PropertyInfo item in properties)
            {
                try
                {
                    ColumnPropertyMapDefinition cpm = columnPropertyMap.FirstOrDefault(x => x.PropertyName == item.Name);

                    if (cpm != null)
                    {
                        string value = testDataRow[cpm.SpreadsheetColumnIndex].ToString().Trim();
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            item.SetValue(testObject, Convert.ChangeType(value, item.PropertyType), null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    throw;
                }
            }
        }
    }
}
