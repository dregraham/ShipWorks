﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Interapptive.Shared;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.SqlServer.Filters.DirtyCounts;

namespace ShipWorks.Filters.Content.SqlGeneration
{
    /// <summary>
    /// The result of filter SQL generation
    /// </summary>
    public class FilterSqlResult
    {
        static readonly ILog log = LogManager.GetLogger(typeof(FilterSqlResult));

        string initialSql;
        string updateSql;
        string existsSql;
        byte[] columnMask;
        int joinMask;

        static Dictionary<EntityType, FilterNodeColumnMaskTable> columnMaskTableMap;

        /// <summary>
        /// Static constructor
        /// </summary>
        [NDependIgnoreLongMethod]
        static FilterSqlResult()
        {
            columnMaskTableMap = new Dictionary<EntityType, FilterNodeColumnMaskTable>();
            columnMaskTableMap[EntityType.CustomerEntity] = FilterNodeColumnMaskTable.Customer;
            columnMaskTableMap[EntityType.OrderEntity] = FilterNodeColumnMaskTable.Order;
            columnMaskTableMap[EntityType.OrderItemEntity] = FilterNodeColumnMaskTable.OrderItem;
            columnMaskTableMap[EntityType.OrderChargeEntity] = FilterNodeColumnMaskTable.OrderCharge;
            columnMaskTableMap[EntityType.NoteEntity] = FilterNodeColumnMaskTable.Note;
            columnMaskTableMap[EntityType.ShipmentEntity] = FilterNodeColumnMaskTable.Shipment;
            columnMaskTableMap[EntityType.PrintResultEntity] = FilterNodeColumnMaskTable.PrintResult;
            columnMaskTableMap[EntityType.EmailOutboundEntity] = FilterNodeColumnMaskTable.EmailOutbound;
            columnMaskTableMap[EntityType.EmailOutboundRelationEntity] = FilterNodeColumnMaskTable.EmailOutboundRelation;
            columnMaskTableMap[EntityType.OrderPaymentDetailEntity] = FilterNodeColumnMaskTable.OrderPaymentDetail;
            columnMaskTableMap[EntityType.DownloadDetailEntity] = FilterNodeColumnMaskTable.DownloadDetail;
            columnMaskTableMap[EntityType.AmazonOrderEntity] = FilterNodeColumnMaskTable.AmazonOrder;
            columnMaskTableMap[EntityType.ChannelAdvisorOrderEntity] = FilterNodeColumnMaskTable.ChannelAdvisorOrder;
            columnMaskTableMap[EntityType.ChannelAdvisorOrderItemEntity] = FilterNodeColumnMaskTable.ChannelAdvisorOrderItem;
            columnMaskTableMap[EntityType.EbayOrderEntity] = FilterNodeColumnMaskTable.EbayOrder;
            columnMaskTableMap[EntityType.EbayOrderItemEntity] = FilterNodeColumnMaskTable.EbayOrderItem;
            columnMaskTableMap[EntityType.MarketplaceAdvisorOrderEntity] = FilterNodeColumnMaskTable.MarketplaceAdvisorOrder;
            columnMaskTableMap[EntityType.OrderMotionOrderEntity] = FilterNodeColumnMaskTable.OrderMotionOrder;
            columnMaskTableMap[EntityType.PayPalOrderEntity] = FilterNodeColumnMaskTable.PayPalOrder;
            columnMaskTableMap[EntityType.ProStoresOrderEntity] = FilterNodeColumnMaskTable.ProStoresOrder;
            columnMaskTableMap[EntityType.PostalShipmentEntity] = FilterNodeColumnMaskTable.PostalShipment;
            columnMaskTableMap[EntityType.UpsShipmentEntity] = FilterNodeColumnMaskTable.UpsShipment;
            columnMaskTableMap[EntityType.FedExShipmentEntity] = FilterNodeColumnMaskTable.FedExShipment;
            columnMaskTableMap[EntityType.CommerceInterfaceOrderEntity] = FilterNodeColumnMaskTable.CommerceInterfaceOrder;
            columnMaskTableMap[EntityType.ShopifyOrderEntity] = FilterNodeColumnMaskTable.ShopifyOrder;
            columnMaskTableMap[EntityType.EtsyOrderEntity] = FilterNodeColumnMaskTable.EtsyOrder;
            columnMaskTableMap[EntityType.YahooOrderEntity] = FilterNodeColumnMaskTable.YahooOrder;
            columnMaskTableMap[EntityType.NeweggOrderEntity] = FilterNodeColumnMaskTable.NeweggOrder;
            columnMaskTableMap[EntityType.BuyDotComOrderItemEntity] = FilterNodeColumnMaskTable.BuyDotComOrderItem;
            columnMaskTableMap[EntityType.SearsOrderEntity] = FilterNodeColumnMaskTable.SearsOrder;
            columnMaskTableMap[EntityType.BigCommerceOrderItemEntity] = FilterNodeColumnMaskTable.BigCommerceOrderItem;
            columnMaskTableMap[EntityType.InsurancePolicyEntity] = FilterNodeColumnMaskTable.InsurancePolicy;
            columnMaskTableMap[EntityType.GrouponOrderEntity] = FilterNodeColumnMaskTable.GrouponOrder;
            columnMaskTableMap[EntityType.LemonStandOrderEntity] = FilterNodeColumnMaskTable.LemonStandOrder;
            columnMaskTableMap[EntityType.WalmartOrderEntity] = FilterNodeColumnMaskTable.WalmartOrder;
            columnMaskTableMap[EntityType.WalmartOrderItemEntity] = FilterNodeColumnMaskTable.WalmartOrderItem;
            columnMaskTableMap[EntityType.AmazonOrderSearchEntity] = FilterNodeColumnMaskTable.AmazonOrderSearch;
            columnMaskTableMap[EntityType.ChannelAdvisorOrderSearchEntity] = FilterNodeColumnMaskTable.ChannelAdvisorOrderSearch;
            columnMaskTableMap[EntityType.ClickCartProOrderSearchEntity] = FilterNodeColumnMaskTable.ClickCartProOrderSearch;
            columnMaskTableMap[EntityType.CommerceInterfaceOrderSearchEntity] = FilterNodeColumnMaskTable.CommerceInterfaceOrderSearch;
            columnMaskTableMap[EntityType.EbayOrderSearchEntity] = FilterNodeColumnMaskTable.EbayOrderSearch;
            columnMaskTableMap[EntityType.GrouponOrderSearchEntity] = FilterNodeColumnMaskTable.GrouponOrderSearch;
            columnMaskTableMap[EntityType.LemonStandOrderSearchEntity] = FilterNodeColumnMaskTable.LemonStandOrderSearch;
            columnMaskTableMap[EntityType.MagentoOrderSearchEntity] = FilterNodeColumnMaskTable.MagentoOrderSearch;
            columnMaskTableMap[EntityType.MarketplaceAdvisorOrderSearchEntity] = FilterNodeColumnMaskTable.MarketplaceAdvisorOrderSearch;
            columnMaskTableMap[EntityType.NetworkSolutionsOrderSearchEntity] = FilterNodeColumnMaskTable.NetworkSolutionsOrderSearch;
            columnMaskTableMap[EntityType.OrderMotionOrderSearchEntity] = FilterNodeColumnMaskTable.OrderMotionOrderSearch;
            columnMaskTableMap[EntityType.OrderSearchEntity] = FilterNodeColumnMaskTable.OrderSearch;
            columnMaskTableMap[EntityType.PayPalOrderSearchEntity] = FilterNodeColumnMaskTable.PayPalOrderSearch;
            columnMaskTableMap[EntityType.ProStoresOrderSearchEntity] = FilterNodeColumnMaskTable.ProStoresOrderSearch;
            columnMaskTableMap[EntityType.SearsOrderSearchEntity] = FilterNodeColumnMaskTable.SearsOrderSearch;
            columnMaskTableMap[EntityType.ShopifyOrderSearchEntity] = FilterNodeColumnMaskTable.ShopifyOrderSearch;
            columnMaskTableMap[EntityType.ThreeDCartOrderSearchEntity] = FilterNodeColumnMaskTable.ThreeDCartOrderSearch;
            columnMaskTableMap[EntityType.WalmartOrderSearchEntity] = FilterNodeColumnMaskTable.WalmartOrderSearch;
            columnMaskTableMap[EntityType.YahooOrderSearchEntity] = FilterNodeColumnMaskTable.YahooOrderSearch;
            columnMaskTableMap[EntityType.GenericModuleOrderEntity] = FilterNodeColumnMaskTable.GenericModuleOrder;
            columnMaskTableMap[EntityType.OverstockOrderEntity] = FilterNodeColumnMaskTable.OverstockOrder;

            // Make sure we've mapped each table
            if (columnMaskTableMap.Keys.Count != Enum.GetValues(typeof(FilterNodeColumnMaskTable)).Length)
            {
                throw new InvalidOperationException("There is a table mapping missing.");
            }

            // Make sure our hard-coded column counts have not changed
            foreach (EntityType entityType in columnMaskTableMap.Keys)
            {
                string entityName = EntityTypeProvider.GetEntityTypeName(entityType);

                // For derived types (like AmazonOrder / Order)... all the fields for AmazonOrder and Order are in the enity... we want to make sure we only
                // count the ones relative to the entity we care about.
                int fieldCount = GeneralEntityFactory.Create(entityType).Fields.OfType<EntityField2>().Count(field => field.ContainingObjectName == entityName);

                // OrderItem has TotalWeight, which is a computed field, which isn't in the entity
                if (entityType == EntityType.OrderItemEntity)
                {
                    fieldCount++;
                }

                var actualFIeldCount = FilterNodeColumnMaskUtility.GetTableBitCount(columnMaskTableMap[entityType]);

                if (fieldCount != actualFIeldCount)
                {
                    // FilterNodeColumnMaskUtility Constructor
                    throw new InvalidOperationException($"Looks like we need to update the bitcount we use to track table to: {actualFIeldCount} \n {entityType}");
                }
            }

            // Validate our column mask columns have enough bits to hold the data
            if (new FilterNodeContentEntity().Fields[(int) FilterNodeContentFieldIndex.ColumnMask].MaxLength < FilterNodeColumnMaskUtility.TotalBytes)
            {
                throw new InvalidOperationException("Looks like the db schema needs updated to fit the bits.  Update both ColumnMask, and all the ColumnsUpdated out there.");
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterSqlResult(long filterCountID, string initialSql, string updateSql, string existsSql, ICollection<EntityField2> columnsUsed, ICollection<FilterNodeJoinType> joinsUsed)
            : this(filterCountID, initialSql, updateSql, existsSql, CreateColumnMask(columnsUsed), CreateJoinMask(joinsUsed))
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterSqlResult(long filterCountID, string initialSql, string updateSql, string existsSql, byte[] columnMask, int joinMask)
        {
            this.columnMask = columnMask;
            this.joinMask = joinMask;

            // Make sure the columnMask has at least one byte - can't create a SQL variable for varbinary(0)
            if (this.columnMask == null || this.columnMask.Length == 0)
            {
                this.columnMask = new byte[1];
            }

            this.initialSql = GenerateSql(filterCountID, initialSql);
            this.updateSql = GenerateSql(filterCountID, updateSql);
            this.existsSql = existsSql;
        }

        /// <summary>
        /// Create the column mask based on the given utilized columns
        /// </summary>
        private static byte[] CreateColumnMask(ICollection<EntityField2> columnsUsed)
        {
            BitArray bits = new BitArray(FilterNodeColumnMaskUtility.TotalBytes * 8);

            // This is a list of fields that can be filtered on, but that are basically "read-only" and can never change - and therefore
            // it does not make sense to track them for updates.
            List<EntityField2> knownIgnoredFields = new List<EntityField2>
                {
                    StoreFields.TypeCode,
                    DownloadFields.Started
                };

            // Set each bit
            foreach (EntityField2 field in columnsUsed)
            {
                if (knownIgnoredFields.Any(ignore => EntityUtility.IsSameField(field, ignore)))
                {
                    // log.DebugFormat("Filter column IGNORING: {0}.{1}", field.ContainingObjectName, field.Name);
                }
                else
                {
                    // For derived tables - like AmazonOrder/Order... the fieldindexes do not map to the database, but to the index in the LLBL fields collection.  So we have
                    // to offset by the index of the PK, since we always know that will be at DB index 0.
                    int dbFieldIndex = field.FieldIndex - GeneralEntityFactory.Create(
                        EntityTypeProvider.GetEntityType(field.ActualContainingObjectName)).PrimaryKeyFields.First(pk => pk.ContainingObjectName == field.ContainingObjectName).FieldIndex;

                    int bitPosition = FilterNodeColumnMaskUtility.GetBitPosition(columnMaskTableMap[EntityTypeProvider.GetEntityType(field.ContainingObjectName)], dbFieldIndex);

                    // log.DebugFormat("Filter column used: {0}.{1} (Index:{2}, Bit:{3})", field.ContainingObjectName, field.Name, dbFieldIndex, bitPosition);
                    bits[bitPosition] = true;
                }
            }

            return FilterNodeColumnMaskUtility.ConvertBitArrayToBitmask(bits);
        }

        /// <summary>
        /// Create a single JoinMask by taking the union and combination of the given list of joins used
        /// </summary>
        private static int CreateJoinMask(ICollection<FilterNodeJoinType> joinsUsed)
        {
            int joinMask = 0;

            // First pass - add them all in as-is
            foreach (var join in joinsUsed)
            {
                joinMask |= (int) join;
            }

            // Next up - figure out the "extensions" (i just made that term up)
            foreach (var join in joinsUsed)
            {
                if (join == FilterNodeJoinType.CustomerToOrder && joinsUsed.Contains(FilterNodeJoinType.OrderToItem))
                {
                    joinMask |= (int) FilterNodeJoinType.CustomerToItem;
                }

                if (join == FilterNodeJoinType.CustomerToOrder && joinsUsed.Contains(FilterNodeJoinType.OrderToShipment))
                {
                    joinMask |= (int) FilterNodeJoinType.CustomerToShipment;
                }

                if (join == FilterNodeJoinType.ItemToOrder && joinsUsed.Contains(FilterNodeJoinType.OrderToCustomer))
                {
                    joinMask |= (int) FilterNodeJoinType.ItemToCustomer;
                }

                if (join == FilterNodeJoinType.ShipmentToOrder && joinsUsed.Contains(FilterNodeJoinType.OrderToCustomer))
                {
                    joinMask |= (int) FilterNodeJoinType.ShipmentToCustomer;
                }
            }

            log.DebugFormat("Joins used: {0}", (FilterNodeJoinType) joinMask);

            return joinMask;
        }

        /// <summary>
        /// The full, complete, executable SQL statement to do the initial calculation
        /// </summary>
        public string InitialSql
        {
            get { return initialSql; }
        }

        /// <summary>
        /// The full, complete, executable SQL statement to do the update calculation
        /// </summary>
        public string UpdateSql
        {
            get { return updateSql; }
        }

        /// <summary>
        /// The full, complete, executable SQL statement to do the entity exists query
        /// </summary>
        public string ExistsSql
        {
            get { return existsSql; }
        }

        /// <summary>
        /// The bitmask of columns whose changes would affect the contents of the filter
        /// </summary>
        public byte[] ColumnMask
        {
            get { return columnMask; }
        }

        /// <summary>
        /// The bitmask of the joins used by the filter, to know which related edits could affect
        /// </summary>
        public int JoinMask
        {
            get { return joinMask; }
        }

        /// <summary>
        /// Generate the complete sql statement for a node based on the given sql, the parameter collection, and count ID
        /// </summary>
        private string GenerateSql(long filterCountID, string sql)
        {
            StringBuilder sb = new StringBuilder(500);

            // Append the list of local parameter declarations
            AppendLocalParameters(sb, filterCountID);

            // Call using sp_executesql so we get parameter sniffing
            sb.AppendLine("EXECUTE sp_executesql");
            sb.AppendLine();

            // Append the SQL statement portion
            sb.AppendLine("N'");
            sb.AppendLine(sql.Replace("'", "''"));
            sb.AppendLine("',");
            sb.AppendLine();

            // Append the parameter declarations and values for sp_executesql
            AppendExecuteSqlParameters(sb);

            return sb.ToString();
        }

        /// <summary>
        /// Append all parameter declarations and values to the output
        /// </summary>
        private void AppendLocalParameters(StringBuilder sb, long filterCountID)
        {
            // Declare the count id and node id (the node id will be set at execution time)
            sb.AppendLine("DECLARE @filterNodeContentID bigint");
            sb.AppendLine("DECLARE @filterNodeID bigint");

            // Declare the column mask.  Currently, all our column masks are 100 in size.  But in case we change that later,
            // use the max of 100 or the actual length.
            sb.AppendLine(string.Format("DECLARE @columnMask varbinary({0})", Math.Max(100, columnMask.Length)));

            sb.AppendLine();

            // Set the count id and column mask
            sb.AppendFormat("SET @filterNodeContentID = {0}\n", filterCountID);
            sb.AppendFormat("SET @columnMask = {0}\n", "0x" + BitConverter.ToString(columnMask).Replace("-", ""));

            // Set the placeholder for the node ID value... it will get replaced at execution time with the current node id value
            sb.AppendLine("SET @filterNodeID = <SwFilterNodeID />");

            sb.AppendLine();
        }

        /// <summary>
        /// Append the parameter declarations, and values, as part of an sp_executesql call
        /// </summary>
        private void AppendExecuteSqlParameters(StringBuilder sb)
        {
            StringBuilder declarations = new StringBuilder();
            StringBuilder values = new StringBuilder();

            AppendExecuteSqlParameter(new SqlParameter("@filterNodeContentID", SqlDbType.BigInt), declarations, values);
            AppendExecuteSqlParameter(new SqlParameter("@filterNodeID", SqlDbType.BigInt), declarations, values);
            AppendExecuteSqlParameter(new SqlParameter("@columnMask", SqlDbType.Binary, columnMask.Length), declarations, values);

            sb.AppendFormat("N'{0}',", declarations);
            sb.AppendLine();
            sb.AppendLine();

            sb.Append(values.ToString());
        }

        /// <summary>
        /// Append the parameter declaration, and value, for the purpose of passing as arguments to sp_executesql
        /// </summary>
        private static void AppendExecuteSqlParameter(SqlParameter parameter, StringBuilder declarations, StringBuilder values)
        {
            // If not the first one, need a comma separator
            if (declarations.Length > 0)
            {
                declarations.Append(", ");
                values.AppendLine(",");
            }

            declarations.AppendFormat("{0} {1}", parameter.ParameterName, GetParameterTypeDeclaration(parameter));

            // This seems odd.  But what it is doing is assigning the input paramter to sp_executesql to the local variable value, which
            // both will happen to have the same name.
            values.AppendFormat("{0} = {0}", parameter.ParameterName);
        }

        /// <summary>
        /// Get the type declaration to represent the given parameter
        /// </summary>
        private static string GetParameterTypeDeclaration(SqlParameter parameter)
        {
            switch (parameter.SqlDbType)
            {
                case SqlDbType.BigInt:
                    return "bigint";

                case SqlDbType.Bit:
                    return "bit";

                case SqlDbType.DateTime:
                    return "datetime";

                case SqlDbType.Decimal:
                    return "money";

                case SqlDbType.Float:
                    return "float";

                case SqlDbType.Int:
                    return "int";

                case SqlDbType.NChar:
                case SqlDbType.NVarChar:
                    return string.Format("nvarchar({0})", (parameter.Size == 0 || parameter.Size > 4000) ? "max" : parameter.Size.ToString());

                case SqlDbType.Binary:
                case SqlDbType.VarBinary:
                    // Currently, all our column masks are 100 in size.But in case we change that later,
                    // use the max of 100 or the actual length.  Also, this could be used for one of the other varbinary
                    // columns that have a larger size.
                    int size = Math.Max(100, parameter.Size);
                    return string.Format("varbinary({0})", size > 8000 ? "max" : size.ToString());
            }

            throw new InvalidOperationException(string.Format("Invalid parameter type: {0}.", parameter.SqlDbType));
        }

    }
}
