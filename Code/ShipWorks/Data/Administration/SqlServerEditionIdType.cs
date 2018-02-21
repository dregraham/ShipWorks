using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// SQL Server edition IDs
    /// </summary>
    [Obfuscation(Exclude = true, StripAfterObfuscation = false, ApplyToMembers = true)]
    public enum SqlServerEditionIdType
    {
        Enterprise = 1804890536,
        EnterpriseEditionCoreBasedLicensing = 1872460670,
        EnterpriseEvaluation = 610778273,
        BusinessIntelligence = 284895786,
        Developer = -2117995310,
        Express = -1592396055,
        ExpressWithAdvancedServices = -133711905,
        Standard = -1534726760,
        Web = 1293598313,
        SqlDatabaseOrSqlDataWarehouse = 1674378470,
        WorkgroupEdition = 1333529388
    }

    /// <summary>
    /// Extension methods for SqlServerEditionType
    /// </summary>
    public static class SqlServerEditionTypeExtensions
    {
        /// <summary>
        /// Returns true if the edition supports compression
        /// </summary>
        public static bool SupportsCompression(this SqlServerEditionIdType value)
        {
            return value != SqlServerEditionIdType.Express &&
                   value != SqlServerEditionIdType.ExpressWithAdvancedServices &&
                   value != SqlServerEditionIdType.Web;
        }

        /// <summary>
        /// Parse a string and return it's converted SqlServerEditionIdType value or it it is not valid
        /// return defaultValue
        /// </summary>
        public static SqlServerEditionIdType Parse(this SqlServerEditionIdType value, string stringValue, SqlServerEditionIdType defaultValue)
        {
            if (!int.TryParse(stringValue, out int intValue))
            {
                return defaultValue;
            }

            IEnumerable<EnumEntry<SqlServerEditionIdType>> enumEntries = 
                EnumHelper.GetEnumList<SqlServerEditionIdType>()
                          .Where(e => intValue == (int) e.Value);

            return enumEntries.Any() ? enumEntries.First().Value : defaultValue;
        }
    }
}
