using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Search;

namespace Interapptive.Shared.Tests.Filters
{
    public static class FilterTestingHelper
    {
        /// <summary>
        /// Create a filter node based on given definition
        /// </summary>
        public static FilterNodeEntity CreateFilterNode(FilterDefinition definition)
        {
            FilterNodeEntity filterNode;

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                FilterNodeContentEntity filterNodeContent = FilterContentManager.CreateNewFilterContent(new SearchSqlGenerator(definition), adapter);

                FilterEntity filter = new FilterEntity
                {
                    Name = "Processed Shipments",
                    FilterTarget = (int)FilterTarget.Shipments,
                    IsFolder = false,
                    Definition = definition.GetXml(),
                    State = 1
                };
                adapter.SaveAndRefetch(filter);

                FilterSequenceEntity sequence = new FilterSequenceEntity
                {
                    ParentFilterID = -26,
                    Filter = filter,
                    Position = 2
                };
                adapter.SaveAndRefetch(sequence);

                filterNode = new FilterNodeEntity
                {
                    ParentFilterNodeID = -26,
                    Created = DateTime.Now,
                    Purpose = (int)FilterNodePurpose.Standard,
                    FilterSequence = sequence,
                    FilterNodeContentID = filterNodeContent.FilterNodeContentID
                };
                adapter.SaveAndRefetch(filterNode);

                adapter.Commit();
            }

            return filterNode;
        }

        /// <summary>
        /// Calculate inital filter counts.  It keeps calling the sproc until it completes.
        /// </summary>
        public static void CalculateInitialCounts()
        {
            using (SqlConnection connection = new SqlConnection(SqlAdapter.Default.ConnectionString))
            {
                connection.Open();

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.CalculateInitialFilterCounts";
                    cmd.Parameters.Add("@nodesUpdated", SqlDbType.Int).Direction = ParameterDirection.Output;

                    int count = 100;

                    while (count != 0)
                    {
                        cmd.ExecuteNonQuery();

                        count = Convert.ToInt32(cmd.Parameters["@nodesUpdated"].Value);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the number of FilterNodeContentDetail rows for the specified filterNodeContentID
        /// </summary>
        /// <param name="filterNodeContentID"></param>
        /// <returns></returns>
        public static int GetFilterNodeContentCount(long filterNodeContentID)
        {
            using (SqlConnection connection = new SqlConnection(SqlAdapter.Default.ConnectionString))
            {
                connection.Open();

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = $"select count(*) from FilterNodeContentDetail where FilterNodeContentID = {filterNodeContentID} ";

                    return int.Parse(cmd.ExecuteScalar().ToString());
                }
            }
        }

        /// <summary>
        /// Query the FilterNodeContentDirty table to go to no entries so we know that filter counts are done
        /// </summary>
        public static void WaitForFilterCountsToFinish()
        {
            int count = 100;
            while (count != 0)
            {
                using (SqlConnection connection = new SqlConnection(SqlAdapter.Default.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = $"select count(*) from FilterNodeContentDirty";

                        count = int.Parse(cmd.ExecuteScalar().ToString());
                    }
                }
            }
        }
    }
}
