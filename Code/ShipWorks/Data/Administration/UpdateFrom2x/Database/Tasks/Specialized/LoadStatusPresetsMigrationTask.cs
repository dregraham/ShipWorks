using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using System.Data;
using Interapptive.Shared;
using ShipWorks.Stores;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.Specialized
{
    /// <summary>
    /// Combines V2 order and order item status strings into status presets
    /// </summary>
    public class LoadStatusPresetsMigrationTask : MigrationTaskBase
    {
        /// <summary>
        /// Factory Type Code
        /// </summary>
        public override MigrationTaskTypeCode TaskTypeCode
        {
            get { return MigrationTaskTypeCode.LoadStatusPresetsTask; }
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public LoadStatusPresetsMigrationTask()
            : base(WellKnownMigrationTaskIds.LoadStatusPresets, MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.RunOnce)
        {

        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        public LoadStatusPresetsMigrationTask(LoadStatusPresetsMigrationTask toCopy)
            : base (toCopy)
        {

        }

        /// <summary>
        /// Clone
        /// </summary>
        public override MigrationTaskBase Clone()
        {
            return new LoadStatusPresetsMigrationTask(this);
        }

        /// <summary>
        /// Execute the task
        /// </summary>
        /// <returns></returns>
        [NDependIgnoreLongMethod]
        protected override int Run()
        {
            Progress.Detail = "Configuring Order Status Presets...";
            List<Tuple<long, string, string>> v2Defaults = new List<Tuple<long, string, string>>();

            List<Tuple<long, List<string>, List<string>>> allStatuses = new List<Tuple<long, List<string>, List<string>>>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, SqlCommandProvider.DefaultTimeout))
            {
                using (SqlConnection con = OpenConnectionForTask(this))
                {
                    // load all of the status data into memory so we can work with it
                    using (SqlCommand cmd = SqlCommandProvider.Create(con))
                    {
                        cmd.CommandText = "SELECT StoreID, OrderStatusStrings, ItemStatusStrings FROM v2m_StoreStatusString";

                        using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(cmd))
                        {
                            while (reader.Read())
                            {
                                long storeID = (long)reader["StoreID"];
                                string orderStatusStrings = (string)reader["OrderStatusStrings"];
                                string itemStatusStrings = (string)reader["ItemStatusStrings"];

                                List<string> parsedOrderStatuses = new List<string>(orderStatusStrings.Split(';').Where(s => s.Length > 0));
                                List<string> parsedItemStatuses = new List<string>(itemStatusStrings.Split(';').Where(s => s.Length > 0));

                                allStatuses.Add(new Tuple<long, List<string>, List<string>>(storeID, parsedOrderStatuses, parsedItemStatuses));

                                // remember the store's default order and item statuses
                                Tuple<long, string, string> storeDefault = new Tuple<long, string, string>(storeID, parsedOrderStatuses.Count == 0 ? "" : parsedOrderStatuses.FirstOrDefault(),
                                                                                                                    parsedItemStatuses.Count == 0 ? "" : parsedItemStatuses.FirstOrDefault());
                                v2Defaults.Add(storeDefault);
                            }
                        }

                        // IEnumerable<List<string>>
                        List<List<string>> orderStatuses = allStatuses.Select(t => t.Item2).ToList();
                        List<List<string>> itemStatuses = allStatuses.Select(t => t.Item3).ToList();

                        // collate all of the common statuses for each
                        List<string> commonOrderStatuses = GetCommonItems(orderStatuses, orderStatuses.Count - 1).ToList();
                        List<string> commonItemStatuses = GetCommonItems(itemStatuses, itemStatuses.Count - 1).ToList();

                        // now go remove all common ones from each store's set
                        foreach (Tuple<long, List<string>, List<string>> storeSet in allStatuses)
                        {
                            // remove all common OrderStatuses first
                            storeSet.Item2.RemoveAll(s => commonOrderStatuses.Contains(s));

                            // remove all common ItemStatuses next
                            storeSet.Item3.RemoveAll(s => commonItemStatuses.Contains(s));
                        }

                        // first save all of the common items to the database
                        cmd.CommandText = "INSERT INTO dbo.StatusPreset (StoreID, StatusTarget, StatusText, IsDefault) VALUES (@StoreID, @StatusTarget, @StatusText, @IsDefault)";
                        cmd.Parameters.Add("@StoreID", SqlDbType.BigInt);
                        cmd.Parameters.Add("@StatusTarget", SqlDbType.Int);
                        cmd.Parameters.Add("@StatusText", SqlDbType.NVarChar, 300);
                        cmd.Parameters.Add("@IsDefault", SqlDbType.Bit);

                        // add common order statuses
                        commonOrderStatuses.ForEach(status =>
                        {
                            cmd.Parameters["@StoreID"].Value = DBNull.Value;
                            cmd.Parameters["@StatusTarget"].Value = (int)StatusPresetTarget.Order;
                            cmd.Parameters["@StatusText"].Value = status;
                            cmd.Parameters["@IsDefault"].Value = 0;

                            SqlCommandProvider.ExecuteNonQuery(cmd);
                        });

                        // add common order item statuses
                        commonItemStatuses.ForEach(status =>
                        {
                            cmd.Parameters["@StoreID"].Value = DBNull.Value;
                            cmd.Parameters["@StatusTarget"].Value = (int)StatusPresetTarget.OrderItem;
                            cmd.Parameters["@StatusText"].Value = status;
                            cmd.Parameters["@IsDefault"].Value = 0;

                            SqlCommandProvider.ExecuteNonQuery(cmd);
                        });

                        // add store-specific
                        allStatuses.ForEach(tuple => UpdateAllStatuses(tuple, v2Defaults, cmd));
                    }

                    // commit the transaction
                    scope.Complete();
                }
            }

            return 1;
        }

        /// <summary>
        /// Update all the statuses
        /// </summary>
        private static void UpdateAllStatuses(Tuple<long, List<string>, List<string>> tuple, List<Tuple<long, string, string>> v2Defaults, SqlCommand cmd)
        {
            long storeID = tuple.Item1;
            List<string> storeOrderStatuses = tuple.Item2;
            List<string> storeItemStatuses = tuple.Item3;

            string storeDefaultOrderStatus = v2Defaults.First(t => t.Item1 == storeID).Item2;
            bool storeDefaultOrderStatusAdded = storeOrderStatuses.Any(status => status == storeDefaultOrderStatus);

            string storeDefaultItemStatus = v2Defaults.First(t => t.Item1 == storeID).Item3;
            bool storeDefaultItemStatusAdded = storeItemStatuses.Any(status => status == storeDefaultItemStatus);

            storeOrderStatuses.ForEach(status =>
                UpdateStoreDetailStatuses(status, cmd, storeID, storeDefaultOrderStatus));

            storeItemStatuses.ForEach(status =>
                UpdateStoreDetailStatuses(status, cmd, storeID, storeDefaultItemStatus));

            // if a Store's V2 default statuses end up being V3 Shared statuses, they wouldn't have been added int he prior loops.  Add them now.
            if (!storeDefaultOrderStatusAdded)
            {
                cmd.Parameters["@StoreID"].Value = storeID;
                cmd.Parameters["@StatusTarget"].Value = (int)StatusPresetTarget.Order;
                cmd.Parameters["@StatusText"].Value = storeDefaultOrderStatus;
                cmd.Parameters["@IsDefault"].Value = true;

                SqlCommandProvider.ExecuteNonQuery(cmd);
            }

            if (!storeDefaultItemStatusAdded)
            {
                cmd.Parameters["@StoreID"].Value = storeID;
                cmd.Parameters["@StatusTarget"].Value = (int)StatusPresetTarget.OrderItem;
                cmd.Parameters["@StatusText"].Value = storeDefaultItemStatus;
                cmd.Parameters["@IsDefault"].Value = true;

                SqlCommandProvider.ExecuteNonQuery(cmd);
            }
        }

        /// <summary>
        /// Update statuses of store details
        /// </summary>
        private static void UpdateStoreDetailStatuses(string status, SqlCommand cmd, long storeID, string storeDefaultOrderStatus)
        {
            cmd.Parameters["@StoreID"].Value = storeID;
            cmd.Parameters["@StatusTarget"].Value = (int)StatusPresetTarget.Order;
            cmd.Parameters["@StatusText"].Value = status;
            cmd.Parameters["@IsDefault"].Value = (status == storeDefaultOrderStatus);

            SqlCommandProvider.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Recursively gets the intersection of all items in the lists
        /// </summary>
        private IEnumerable<string> GetCommonItems(List<List<string>> orderStatuses, int current)
        {
            if (current > 0)
            {
                return orderStatuses[current].Intersect(GetCommonItems(orderStatuses, current - 1));
            }
            else if (current == 0)
            {
                return orderStatuses[0];
            }
            else
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// Calculate an estimate
        /// </summary>
        protected override int RunEstimate(SqlConnection con)
        {
            // yeah, considering to be just a single step
            return 1;
        }
    }
}
