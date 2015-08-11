using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Excel;
using Xunit;


namespace ShipWorks.Tests.Integration.MSTest
{
    public class DataDrivenIntegrationTestBase
    {
        private TestContext testContextInstance;
        private bool isTranslationMapPopulated = false;

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
        
        /// <summary>
        /// Populates the test object based on the mapping of a spreadsheet columns to the test object's properties.
        /// </summary>
        /// <returns>Returns true when a row needs to be tests (i.e. the current row is not the headers); otherwise false.</returns>
        public bool PopulateTestObject<T>(T testObject, List<ColumnPropertyMapDefinition> columnPropertyMap)
        {
            DataRow testDataRow = TestContext.DataRow;

            int rowIndex = testDataRow.Table.Rows.IndexOf(testDataRow);

            if (string.IsNullOrWhiteSpace(testDataRow[0].ToString()))
            {
                return false;
            }

            if (!isTranslationMapPopulated)
            {
                PopulateTranslationMap(TestContext.DataConnection.Database, testDataRow.Table.TableName, columnPropertyMap);
            }

            if (rowIndex == 0 && !isTranslationMapPopulated)
            {
                // Build the translation map using the column headers in the first row
                PopulateTranslationMap(TestContext.DataConnection.Database, testDataRow.Table.TableName, columnPropertyMap);

                return false;
            }

            PopulateValues(testObject, testDataRow, columnPropertyMap);

            return true;
        }

        private void PopulateTranslationMap(string fileName, string tabName, List<ColumnPropertyMapDefinition> columnPropertyMap )
        {
            tabName = tabName.Replace("$", "");
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

            isTranslationMapPopulated = true;
        }

        public void GenerateColumnPropertyListCode()
        {
            StringBuilder populationCode = new StringBuilder();
            List<ColumnPropertyMapDefinition> columnPropertyMap = new List<ColumnPropertyMapDefinition>();

            PopulateTranslationMap(TestContext.DataConnection.Database, TestContext.DataRow.Table.TableName, columnPropertyMap);

            if (columnPropertyMap.Any(cpm => cpm.SpreadsheetColumnIndex == -1))
            {
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

        public void GetPropertyNames<T>(T testObject)
        {
            List<PropertyInfo> properties = (testObject.GetType().GetProperties().Where(c => c.DeclaringType.FullName.ToUpperInvariant().Contains("SHIPWORKS")
                                                                                             && c.Name.ToUpperInvariant() != "MagicKeysDown".ToUpperInvariant()
                                                                                             && c.Name.ToUpperInvariant() != "DebugKeysDown".ToUpperInvariant()
                                                                                             && c.Name.ToUpperInvariant() != "Mapping".ToUpperInvariant())).OrderBy(pi => pi.Name).ToList();
            
            StringBuilder propertyNames = new StringBuilder();
            properties.ForEach(pi => propertyNames.AppendLine(pi.Name));
        }

        /// <summary>
        /// Iterates through each property, in our ShipWorks MSTest objects, setting each to null
        /// </summary>
        private void PopulateValues<T>(T testObject, DataRow testDataRow, List<ColumnPropertyMapDefinition> columnPropertyMap)
        {
            PropertyInfo[] properties = (testObject.GetType().GetProperties().Where(c => c.DeclaringType.FullName.ToUpperInvariant().Contains("SHIPWORKS")
                                                                                         && c.Name.ToUpperInvariant() != "MagicKeysDown".ToUpperInvariant()
                                                                                         && c.Name.ToUpperInvariant() != "DebugKeysDown".ToUpperInvariant())).ToArray();
            foreach (PropertyInfo item in properties)
            {
                try
                {
                    ColumnPropertyMapDefinition cpm = columnPropertyMap.FirstOrDefault(x => x.PropertyName == item.Name);

                    if (cpm != null)
                    {
                        if (cpm.SpreadsheetColumnIndex == -1)
                        {
                            throw new Exception(cpm.SpreadsheetColumnName + " has an invalid column index.");
                        }

                        string value = testDataRow[cpm.SpreadsheetColumnIndex].ToString().Trim();
                        value = value.Replace("Each Package", "");
                        value = value.Replace("each package", "");
                        value = value.Replace("(Each Package)", "");
                        value = value.Trim();

                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            try
                            {
                                item.SetValue(testObject, Convert.ChangeType(value, item.PropertyType), null);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Error encountered while populating property {0}: {1}", item.Name, e.Message);
                                throw;
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }

        /// <summary>
        /// Iterates through each property, in our ShipWorks MSTest objects, verifying there's a matching column
        /// </summary>
        protected void DetermineMissingProperties<T>(T testObject, DataRow testDataRow, List<ColumnPropertyMapDefinition> columnPropertyMap)
        {
            PropertyInfo[] properties = (from c in testObject.GetType().GetProperties()
                                         where c.DeclaringType.FullName.ToUpperInvariant().Contains("SHIPWORKS")
                                            && c.Name.ToUpperInvariant() != "MagicKeysDown".ToUpperInvariant()
                                            && c.Name.ToUpperInvariant() != "DebugKeysDown".ToUpperInvariant()
                                         select c).ToArray();

            string missingProperties = string.Empty;
            //string format = "SpreadsheetColumn: {0},  Property: {1}";
            string format = "{0}\t{1}";

            Debug.WriteLine("ColumnPropertyMapDefinition with GOOD indexes");
            Debug.WriteLine("SpreadsheetColumnName\tPropertyName");
            columnPropertyMap.Where(x => x.SpreadsheetColumnIndex != -1).ToList().ForEach(x => Debug.WriteLine(format, x.SpreadsheetColumnName, x.PropertyName));

            Debug.WriteLine("ColumnPropertyMapDefinition with -1 indexes");
            Debug.WriteLine("SpreadsheetColumnName\tPropertyName");
            columnPropertyMap.Where(x => x.SpreadsheetColumnIndex == -1).ToList().ForEach(x => Debug.WriteLine(format, x.SpreadsheetColumnName, x.PropertyName));

            Debug.WriteLine("ColumnPropertyMapDefinition matches Properties");
            Debug.WriteLine("SpreadsheetColumnName\tPropertyName");
            var qry = from cpm in columnPropertyMap
                join prop in properties on cpm.PropertyName equals prop.Name
                select cpm;
            qry.ToList().ForEach(x => Debug.WriteLine(format, x.SpreadsheetColumnName, x.PropertyName));



            foreach (PropertyInfo item in properties)
            {
                try
                {
                    ColumnPropertyMapDefinition cpm = columnPropertyMap.FirstOrDefault(x => x.PropertyName == item.Name);

                    if (cpm != null)
                    {
                        if (cpm.SpreadsheetColumnIndex == -1)
                        {
                            missingProperties += string.Format("{0}{1}", cpm.SpreadsheetColumnName, System.Environment.NewLine);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    throw;
                }
            }

            Debug.Write(missingProperties);
        }
    }
}
