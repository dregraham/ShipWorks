using System;
using System.Data;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// Iterates through each property, in our ShipWorks MSTest objects, setting each to null
        /// </summary>
        protected void PopulateValues<T>(T testObject, DataRow testDataRow)
        {
            PropertyInfo[] properties = (from c in testObject.GetType().GetProperties()
                                         where c.DeclaringType.FullName.ToUpperInvariant().Contains("SHIPWORKS")
                                            && c.Name.ToUpperInvariant() != "MagicKeysDown".ToUpperInvariant()
                                            && c.Name.ToUpperInvariant() != "DebugKeysDown".ToUpperInvariant()
                                         select c).ToArray();
            string missingColumns = string.Empty;
            foreach (PropertyInfo item in properties)
            {
                try
                {
                    if (!testDataRow.Table.Columns.Contains(item.Name))
                    {
                        missingColumns += string.Format("{0}{1}", item.Name, System.Environment.NewLine);
                        continue;
                    }

                    string val = testDataRow[item.Name].ToString().Trim();
                    if (!string.IsNullOrWhiteSpace(val))
                    {
                        item.SetValue(testObject, Convert.ChangeType(val, item.PropertyType), null);
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
