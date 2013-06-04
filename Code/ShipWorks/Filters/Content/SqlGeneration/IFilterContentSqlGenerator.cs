using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Filters.Content.SqlGeneration
{
    /// <summary>
    /// Interface that provides the SQL to be used for a FilterNodeContent
    /// </summary>
    public interface IFilterContentSqlGenerator
    {
        FilterSqlResult GenerateSql(long countID);
    }
}
