using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Tests.Filters.Content.SqlGeneration
{
    [TestClass]
    public class FilterSqlResultTest
    {
        [TestMethod]
        public void CheckBitCounts()
        {
            FilterSqlResult.CheckBitCounts();
        }
    }
}
