using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.ThreeDCart.AdvancedApi
{
    /// <summary>
    /// 3dcart sites can run against a sql server database or an MS Access database.
    /// The advanced api takes sql statements to return results.  These sql statements
    /// must be correct for the type of database the site is running.
    /// 
    /// This interface is used to return the appropriate sql statements based on database
    /// type.
    /// </summary>
    public interface IApiQueryProvider
    {
        /// <summary>
        /// A sql statement that when run will NOT result in a sql error.  
        /// This is used to see if the site is running a SQL Server or MS Access database.
        /// </summary>
        /// <value>SQL query that should NOT return an error when run against a database.</value>
        string ValidSqlStatementForThisDatabaseTypeOnly
        {
            get;
        }

        /// <summary>
        /// Query to get orders by last order date format.
        /// </summary>
        /// <param name="orderSearchCriteria">ThreeDCartWebClientOrderSearchCriteria from which to build the sql statement</param>
        /// <returns>SQL statement to get orders by last order date format.</returns>
        string QueryByOrderModifiedDate(ThreeDCartWebClientOrderSearchCriteria orderSearchCriteria);

        /// <summary>
        /// Query to get orders by create date.
        /// </summary>
        /// <param name="orderSearchCriteria">ThreeDCartWebClientOrderSearchCriteria from which to build the sql statement</param>
        /// <returns>SQL statement to get orders by order create date.</returns>
        string QueryByOrderCreateDate(ThreeDCartWebClientOrderSearchCriteria orderSearchCriteria);

        /// <summary>
        /// Query to get order count by date.  Date queried is based on ThreeDCartWebClientOrderDateSearchType.
        /// </summary>
        /// <param name="orderSearchCriteria">ThreeDCartWebClientOrderSearchCriteria from which to build the sql statement</param>
        /// <returns>SQL statement to get order count by date format.</returns>
        string QueryOrderCount(ThreeDCartWebClientOrderSearchCriteria orderSearchCriteria);

        /// <summary>
        /// Query to get all order statuses.
        /// </summary>
        /// <value>SQL query to get all order statuses.</value>
        string QueryOrderStatuses
        {
            get;
        }

        /// <summary>
        /// Query to get product info by 3dcart item id.
        /// NOTE: 3dcart item id is actually referred to as ProductID when using the regular cart api
        /// </summary>
        /// <param name="invoiceNumber"> </param>
        /// <param name="threeDCartProductID">ThreeDCartWebClientOrderSearchCriteria from which to build the sql statement</param>
        /// <returns>SQL query to get all order statuses.</returns>
        string QueryProductsByItemID(long invoiceNumber, string threeDCartProductID);
    }
}
