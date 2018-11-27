using System.Collections.Generic;
using System.Data.Common;
using Interapptive.Shared.Data;
using ShipWorks.Data.Connection;

namespace ShipWorks.Data
{
    /// <summary>
    /// Static class for getting startup telemetry for products
    /// </summary>
    public static class ProductCatalogTelemetryService
    {
        /// <summary>
        /// Get product telemetry data
        /// </summary>
        internal static IEnumerable<KeyValuePair<string, string>> GetTelemetryData()
        {
            List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>();

            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                using (DbCommand cmd = DbCommandProvider.Create(con))
                {
                    cmd.CommandText = @"
                        SELECT 'ProductCatalog.Product.Count' AS [Key], count(*) as [Value] FROM Product
                        UNION
                        SELECT 'ProductCatalog.ProductBundle.Count' AS [Key], count(*) as [Value] FROM ProductBundle
                        UNION
                        SELECT 'ProductCatalog.ProductVariant.Count' AS [Key], count(*) as [Value] FROM ProductVariant
                        UNION
                        SELECT 'ProductCatalog.ProductVariantAlias.Count' AS [Key], count(*) as [Value] FROM ProductVariantAlias
                        UNION
                        SELECT 'ProductCatalog.ProductVariantTypeAndValueCount.Count' AS [Key], count(*) as [Value] FROM ProductVariantTypeAndValue
                        ";

                    using (DbDataReader reader = DbCommandProvider.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            data.Add(new KeyValuePair<string, string>(reader["Key"].ToString(), reader["Value"].ToString()));
                        }
                    }
                }
            }

            return data;
        }
    }
}