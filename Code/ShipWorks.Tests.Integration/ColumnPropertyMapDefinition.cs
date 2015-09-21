using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Tests.Integration.MSTest
{
    public class ColumnPropertyMapDefinition
    {
        public string SpreadsheetColumnName { get; set; }
        public string PropertyName { get; set; }
        public int SpreadsheetColumnIndex { get; set; }
    }
}
