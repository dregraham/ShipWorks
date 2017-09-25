using System;
using System.Collections.Generic;
using System.Diagnostics;
using Interapptive.Shared.Utility;
using log4net;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// ShopifyGetOrdersDateRange is used to create a tree of date ranges and order counts contained in that date range.
    /// A child node is created based on the number of orders that fit into
    /// a date range where the number of orders is less than or equal to ShopifyConstants.MaxResultsToReturn.
    /// When a child nodes are created, the left node will have a start and end date of half it's parent's date range, the right node
    /// will have the other half.
    ///
    /// For example, a parent with start date of 5/1/2012 and end date of 5/30/2012, order count of 50, and max results of 10 will
    /// have a first level left child of 5/1/2012 - 5/15/2012 and right child of 5/16/2012 - 5/30/2012.  The left and right child
    /// will then make a call to shopify to find the number of orders in it's range.
    ///
    /// The whole point of this isbecause Shopify does not return orders sorted in 
    /// an ascending order.  We have to figure out what ranges we need to ask for to make sure we get them
    /// ascending, otherwise if the user canceled early we'd miss any chunk that was between when they canceled and the last download.
    ///
    /// </summary>
    public class ShopifyGetOrdersDateRange
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShopifyGetOrdersDateRange));

        int? orderCount = null;
        int pageCount = 1;

        DateTime startDate;
        DateTime endDate;

        ShopifyGetOrdersDateRange leftChild;
        ShopifyGetOrdersDateRange rightChild;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyGetOrdersDateRange(Range<DateTime> range) : this(range.Start, range.End)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rangeStart">Start date of the range </param>
        /// <param name="rangeEnd">End date of the range</param>
        private ShopifyGetOrdersDateRange(DateTime rangeStart, DateTime rangeEnd)
        {
            startDate = rangeStart;
            endDate = rangeEnd;
        }

        /// <summary>
        /// The number of orders within this date range
        /// </summary>
        public int OrderCount => orderCount ?? 0;

        /// <summary>
        /// PageCount will always be zero unless the range has been reduced to its smallest possible unit (one second) and still can't fit in an
        /// entire page of orders.  When that happens, this will be set to the total number of page requests required to get all of the orders.
        /// </summary>
        public int PageCount => pageCount;

        /// <summary>
        /// Start date of the range, always in UTC
        /// </summary>
        public DateTime StartDate => startDate;

        /// <summary>
        /// End date of the range, always in UTC
        /// </summary>
        public DateTime EndDate => endDate;

        /// <summary>
        /// The time span between the start date and end date.
        /// This is always a positive time span.
        /// </summary>
        private TimeSpan Duration => (EndDate - StartDate).Duration();

        /// <summary>
        /// Helper property to get the start date plus 1/2 of the difference between start and end date
        /// </summary>
        private DateTime MidpointDate => StartDate.AddMilliseconds(Duration.TotalMilliseconds / 2);

        /// <summary>
        /// Generates an ordered list of date ranges that represent blocks that contain orders less than or equal to the maximum page size Shopify allows
        /// </summary>
        public IEnumerable<ShopifyGetOrdersDateRange> GenerateOrderRanges(IShopifyWebClient webClient, int ordersPerPage)
        {
            MethodConditions.EnsureArgumentIsNotNull(webClient, nameof(webClient));

            // Figure out what count this node instance we are right now represents.
            QueryOrderCount(webClient);

            // If there are no orders in this node, there's nothing to iterate
            if (OrderCount == 0)
            {
                yield break;
            }

            // If it's small enough to fit all orders in this range, then all we return is ourselves
            if (OrderCount <= ordersPerPage)
            {
                // All the orders fit in this node!
                yield return this;
                yield break;
            }

            // If we are less than two seconds then we cant divide the range any further, since we can't query in any smaller intervals than a second.  Instead,
            // we'll break up the range we have into pages.
            if (Duration < TimeSpan.FromSeconds(2))
            {
                pageCount = (OrderCount + ordersPerPage - 1) / ordersPerPage; 

                yield return this;
                yield break;
            }

            // Create the left child
            leftChild = new ShopifyGetOrdersDateRange(StartDate, MidpointDate);

            // Yield all our descendants on the left side (if any)
            foreach (var range in leftChild.GenerateOrderRanges(webClient, ordersPerPage))
            {
                yield return range;
            }

            // If all the orders didn't fit in the left range, we also need the right-hand child range
            if (leftChild.OrderCount < this.OrderCount)
            {
                // Create the right child
                rightChild = new ShopifyGetOrdersDateRange(leftChild.EndDate, EndDate);

                // Yield all our descendants on the right side (if any)
                foreach (var range in rightChild.GenerateOrderRanges(webClient, ordersPerPage))
                {
                    yield return range;
                }

                // They should add up
                Debug.Assert(leftChild.OrderCount + rightChild.OrderCount == this.OrderCount);
            }
        }

        /// <summary>
        /// Calls the shopify web client to get the number of orders contained within this date range
        /// </summary>
        /// <param name="webClient">Shopify web client on which to make API calls</param>
        /// <returns>Number of orders in this date range</returns>
        private void QueryOrderCount(IShopifyWebClient webClient)
        {
            MethodConditions.EnsureArgumentIsNotNull(webClient, nameof(webClient));

            // If we already have the count, don't need to get it again
            if (orderCount == null)
            {
                // Query the count form the web client
                orderCount = webClient.GetOrderCount(startDate, endDate);
            }
        }

        /// <summary>
        /// ToString override
        /// </summary>
        /// <returns>This objects children info, plus its children's ToString</returns>
        public override string ToString()
        {
            string text = string.Format("Start: '{0}', End '{1}', Count: {2}, Left: {3}, Right: {4}",
                StartDate, EndDate, OrderCount,
                leftChild == null ? "{null}" : leftChild.OrderCount.ToString(),
                rightChild == null ? "{null}" : rightChild.OrderCount.ToString());

            if (pageCount > 1)
            {
                text += ", Pages: " + pageCount;
            }

            return text;
        }

        /// <summary>
        /// Visually log the tree
        /// </summary>
        public void LogTree()
        {
            LogTreeNode("", true);
        }

        /// <summary>
        /// Visually log the tree
        /// </summary>
        private void LogTreeNode(string indent, bool last)
        {
            Debug.Write(indent);

            if (last)
            {
                Debug.Write("\\-");
                indent += "  ";
            }
            else
            {
                Debug.Write("|-");
                indent += "| ";
            }

            string count = OrderCount.ToString();
            if (pageCount > 1)
            {
                count += string.Format(" [{0} pages]", pageCount);
            }

            Debug.WriteLine("{0} -> {1} ({2})", StartDate, EndDate, count);

            List<ShopifyGetOrdersDateRange> children = new List<ShopifyGetOrdersDateRange>();

            if (leftChild != null)
            {
                children.Add(leftChild);
            }

            if (rightChild != null)
            {
                children.Add(rightChild);
            }

            for (int i = 0; i < children.Count; i++)
            {
                children[i].LogTreeNode(indent, i == children.Count - 1);
            }
        }
    }
}
