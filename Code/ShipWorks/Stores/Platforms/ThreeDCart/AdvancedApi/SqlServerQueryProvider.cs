using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.ThreeDCart.Enums;

namespace ShipWorks.Stores.Platforms.ThreeDCart.AdvancedApi
{
    /// <summary>
    /// 3dcart sites can run against a sql server database or an MS Access database.
    /// The advanced api takes sql statements to return results.  These sql statements
    /// must be correct for the type of database the site is running.
    /// 
    /// This class is used to return the appropriate sql statements for MS Sql Server.
    /// </summary>
    public class SqlServerQueryProvider : IApiQueryProvider
    {
        /// <summary>
        /// A sql statement that when run will NOT result in a sql error.  
        /// This is used to see if the site is running a SQL Server or MS Access database.
        /// </summary>
        /// <value>SQL query that should NOT return an error when run against an MS Sql Server.</value>
        public string ValidSqlStatementForThisDatabaseTypeOnly
        {
            get
            {
                return "select @@Version";
            }
        }
		
		/// <summary>
        /// Query to get all order statuses.
        /// </summary>
        /// <value>SQL query to get all order statuses.</value>
        public string QueryOrderStatuses
        {
            get
            {
                return "select * from order_Status";
            }
        }


        /// <summary>
        /// Query to get orders by last order date format.
        /// </summary>
        /// <param name="orderSearchCriteria">ThreeDCartWebClientOrderSearchCriteria from which to build the sql statement</param>
        /// <returns>SQL statement to get orders by last order date format.</returns>
        public string QueryByOrderModifiedDate(ThreeDCartWebClientOrderSearchCriteria orderSearchCriteria)
        {
            return string.Format("select top {0} with ties orderid, invoicenum, invoicenum_prefix, (odate + otime) as odate, last_update from [Orders] " +
                "where invoicenum > 0 and recurrent_frequency = 0 AND order_status <> {1} AND " +
                    "(" +
                       " last_update >= '{2}' and last_update <= '{3}' " +
                    ") " +
                    " order by last_update",
                    orderSearchCriteria.PageSize,
                    ThreeDCartConstants.OrderStatusNotCompleted,
                    orderSearchCriteria.LastModifiedFromDate,
                    orderSearchCriteria.LastModifiedToDate);
        }

        /// <summary>
        /// Query to get orders by last order date format.
        /// </summary>
        /// <param name="orderSearchCriteria">ThreeDCartWebClientOrderSearchCriteria from which to build the sql statement</param>
        /// <returns>SQL statement to get orders by last order date format.</returns>
        public string QueryByOrderCreateDate(ThreeDCartWebClientOrderSearchCriteria orderSearchCriteria)
        {
            return string.Format("select top {0} with ties orderid, invoicenum, invoicenum_prefix, (odate + otime) as odate, last_update from [Orders] " +
                "where invoicenum > 0 and recurrent_frequency = 0 AND order_status <> {1} AND " +
                "(" +
                "    ((odate + otime) >= '{2}' AND (odate + otime) <= '{3}') " +
                ") " +
                " order by (odate + otime)",
                orderSearchCriteria.PageSize,
                ThreeDCartConstants.OrderStatusNotCompleted,
                orderSearchCriteria.LastCreatedFromDate,
                orderSearchCriteria.LastCreatedToDate);
        }

        /// <summary>
        /// Query to get order count by date.  Date queried is based on ThreeDCartWebClientOrderDateSearchType.
        /// </summary>
        /// <param name="orderSearchCriteria">ThreeDCartWebClientOrderSearchCriteria from which to build the sql statement</param>
        /// <returns>SQL statement to get order count by date.</returns>
        public string QueryOrderCount(ThreeDCartWebClientOrderSearchCriteria orderSearchCriteria)
        {
            string sql = string.Format("select count(*) from orders where invoicenum > 0 and recurrent_frequency = 0 AND order_status <> {0} AND ",
                ThreeDCartConstants.OrderStatusNotCompleted);

            if (orderSearchCriteria.OrderDateSearchType == ThreeDCartWebClientOrderDateSearchType.ModifiedDate)
            {
                sql += string.Format(" last_update >= '{0}' and last_update <= '{1}' ",
                                    orderSearchCriteria.LastModifiedFromDate,
                                    orderSearchCriteria.LastModifiedToDate);
            }
            else
            {
                sql += string.Format("  (odate + otime >= '{0}' AND odate + otime <= '{1}')",
                                    orderSearchCriteria.LastCreatedFromDate,
                                    orderSearchCriteria.LastCreatedToDate);
            }

            return sql;
        }

        /// <summary>
        /// Query to get product info by 3dcart item id.
        /// NOTE: 3dcart item id is actually referred to as ProductID when using the regular cart api
        /// </summary>
        /// <param name="invoiceNumber"> </param>
        /// <param name="threeDCartProductID">ThreeDCartWebClientOrderSearchCriteria from which to build the sql statement</param>
        /// <returns>SQL query to get all order statuses.</returns>
        public string QueryProductsByItemID(long invoiceNumber, string threeDCartProductID)
        {
            return string.Format(@"select distinct p.catalogid, p.id, oi.itemid, oi.itemname, oi.options, oi.optionprice, oi.unitprice " +
                "from oitems oi, orders o, products p " +
                "where o.invoicenum = {0} and oi.orderid = o.orderid and oi.catalogid = p.catalogid and oi.itemid = '{1}'",
                invoiceNumber, threeDCartProductID);
        }
    }
}
