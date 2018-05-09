using System.Collections.Generic;
using System.ComponentModel;
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
        [Description("Enterprise")]
        Enterprise = 1804890536,

        [Description("Enterprise Core Based Licensing")]
        EnterpriseEditionCoreBasedLicensing = 1872460670,

        [Description("Enterprise Evaluation")]
        EnterpriseEvaluation = 610778273,

        [Description("Business Intelligence")]
        BusinessIntelligence = 284895786,

        [Description("Developer")]
        Developer = -2117995310,

        [Description("Express")]
        Express = -1592396055,

        [Description("Express With Advanced Services")]
        ExpressWithAdvancedServices = -133711905,

        [Description("Standard")]
        Standard = -1534726760,

        [Description("Web")]
        Web = 1293598313,

        [Description("Sql Database Or Sql Data Warehouse")]
        SqlDatabaseOrSqlDataWarehouse = 1674378470,

        [Description("Workgroup Edition")]
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
            return value == SqlServerEditionIdType.Enterprise ||
                   value == SqlServerEditionIdType.EnterpriseEditionCoreBasedLicensing ||
                   value == SqlServerEditionIdType.EnterpriseEvaluation ||
                   value == SqlServerEditionIdType.Standard ||
                   value == SqlServerEditionIdType.BusinessIntelligence ||
                   value == SqlServerEditionIdType.Developer;
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
