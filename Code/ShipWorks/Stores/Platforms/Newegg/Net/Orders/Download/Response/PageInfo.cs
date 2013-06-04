using System;
using System.Xml;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.Download.Response
{
    /// <summary>
    /// A data transport object containing metadata about the pages within an order download from Newegg.
    /// </summary>
    [Serializable]
    [XmlRoot("PageInfo")]
    public class PageInfo
    {
        /// <summary>
        /// Gets or sets the total number of records for the request.
        /// </summary>
        /// <value>The record count.</value>
        [XmlElement("TotalCount")]
        public int RecordCount { get; set; }

        /// <summary>
        /// Gets or sets the total page count for the request
        /// </summary>
        /// <value>The page count.</value>
        [XmlElement("TotalPageCount")]
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets the index of the current page. This indicates
        /// what page we're on in relation to the total page count.
        /// </summary>
        /// <value>The index of the current page.</value>
        [XmlElement("PageIndex")]
        public int CurrentPageIndex { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of records contained on each page.
        /// </summary>
        /// <value>The maximum number of records on a page.</value>
        [XmlElement("PageSize")]
        public int MaxNumberOfRecordsOnAPage { get; set; }
    }
}
