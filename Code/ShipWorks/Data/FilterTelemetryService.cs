using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Interapptive.Shared.Data;
using Interapptive.Shared.Extensions;
using ShipWorks.Data.Connection;
using ShipWorks.Filters;

namespace ShipWorks.Data
{
    /// <summary>
    /// Static class for getting startup telemetry on filters
    /// </summary>
    public static class FilterTelemetryService
    {
        private static int quickFilterCount;
        private static int searchFilterCount;
        private static int enabledFilterCount;
        private static int disabledFilterCount;
        private static readonly List<int> filterCounts = new List<int>();
        private static readonly List<int> filterCosts = new List<int>();
        
        /// <summary>
        /// Get filter telemetry data
        /// </summary>
        internal static IEnumerable<KeyValuePair<string, string>> GetTelemetryData()
        {
            GetFilterMetrics();
            
            List<KeyValuePair<string, string>> telemetryData = new List<KeyValuePair<string, string>>(new []
            {
                new KeyValuePair<string, string>("Filters.Content.Count.Average", filterCounts.Average().ToString()),
                new KeyValuePair<string, string>("Filters.Content.Count.Max", filterCounts.Max().ToString()),
                new KeyValuePair<string, string>("Filters.Content.Count.Min", filterCounts.Min().ToString()),
                new KeyValuePair<string, string>("Filters.Content.Count.StdDev", filterCounts.StandardDeviation().ToString()),
                new KeyValuePair<string, string>("Filters.Content.Cost.Average", filterCosts.Average().ToString()),
                new KeyValuePair<string, string>("Filters.Content.Cost.Max", filterCosts.Max().ToString()),
                new KeyValuePair<string, string>("Filters.Content.Cost.Min", filterCosts.Min().ToString()),
                new KeyValuePair<string, string>("Filters.Content.Cost.StdDev", filterCosts.StandardDeviation().ToString()),
                new KeyValuePair<string, string>("Filters.Enabled.Count", enabledFilterCount.ToString()),
                new KeyValuePair<string, string>("Filters.Disabled.Count", disabledFilterCount.ToString()),
                new KeyValuePair<string, string>("Filters.Quick.Count", quickFilterCount.ToString()),
                new KeyValuePair<string, string>("Filters.Search.Count", searchFilterCount.ToString())
            });

            return telemetryData;
        }

        /// <summary>
        /// Get the necessary filter data from the database
        /// </summary>
        private static void GetFilterMetrics()
        {
            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                using (DbCommand cmd = DbCommandProvider.Create(con))
                {
                    cmd.CommandText = @"
                    SELECT fnode.Purpose as FilterType, f.State as Enabled, fcont.Count, fcont.Cost
                        FROM [FilterNode] fnode 
                        INNER JOIN [FilterSequence] fseq ON fnode.FilterSequenceID = fseq.FilterSequenceID 
                        INNER JOIN [Filter] f ON f.FilterID = fseq.FilterID 
                        INNER JOIN [FilterNodeContent] fcont ON fnode.FilterNodeContentID = fcont.FilterNodeContentID
                        WHERE fnode.FilterNodeID > 0";

                    using (DbDataReader reader = DbCommandProvider.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            FilterNodePurpose filterType = (FilterNodePurpose) reader["FilterType"];
                            
                            if (filterType == FilterNodePurpose.Quick)
                            {
                                quickFilterCount++;
                            }
                            else if(filterType == FilterNodePurpose.Search)
                            {
                                searchFilterCount++;
                            }
                            
                            if ((byte) reader["Enabled"] == 0)
                            {
                                disabledFilterCount++;
                            }
                            else
                            {
                                enabledFilterCount++;
                            }
                            
                            filterCounts.Add((int) reader["Count"]);
                            filterCosts.Add((int) reader["Cost"]);
                        }
                    }
                }
            }
        }
    }
}