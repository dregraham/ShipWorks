using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;

namespace ShipWorks.Stores.Platforms.Sears
{
    /// <summary>
    /// Represents a single page of sears orders
    /// </summary>
    public class SearsOrdersPage
    {
        int pageNumber;
        int totalPages;

        XPathNavigator response;

        /// <summary>
        /// Constructor
        /// </summary>
        public SearsOrdersPage(int pageNumber, int totalPages, XPathNavigator response)
        {
            this.pageNumber = pageNumber;
            this.totalPages = totalPages;
            this.response = response;
        }

        /// <summary>
        /// The page number this object represents
        /// </summary>
        public int PageNumber
        {
            get { return pageNumber; }
        }

        /// <summary>
        /// The total number of pages to be downloaded
        /// </summary>
        public int TotalPages
        {
            get { return totalPages; }
        }

        /// <summary>
        /// Indicates if this is the last page to be downloaded
        /// </summary>
        public bool IsLastPage
        {
            get { return pageNumber >= totalPages; }
        }

        /// <summary>
        /// The response returned as an XPath document
        /// </summary>
        public XPathNavigator XPathResponse
        {
            get { return response; }
        }
    }
}
