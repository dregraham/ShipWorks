using System;
using Xunit;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Tests.Filters.Content.SqlGeneration
{
    public class FilterSqlResultTest
    {
        [Fact]
        public void CheckBitCounts()
        {
            typeof(FilterSqlResult).TypeInitializer.Invoke(null, null); 
        }
    }
}
