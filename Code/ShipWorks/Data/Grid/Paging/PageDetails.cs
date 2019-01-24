using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Data.Grid.Paging
{
    /// <summary>
    /// Details about a page
    /// </summary>
    public struct PageDetails
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PageDetails(int pageSize, int row)
        {
            Size = pageSize;
            PageNumber = 1 + (row - (row % pageSize)) / pageSize;
            StartRow = (PageNumber - 1) * pageSize;
            EndRow = PageNumber * pageSize - 1;
        }

        /// <summary>
        /// Size of the page
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// Number of the page
        /// </summary>
        public int PageNumber { get; }

        /// <summary>
        /// First row in the page
        /// </summary>
        public int StartRow { get; }

        /// <summary>
        /// Last row in the page
        /// </summary>
        public int EndRow { get; }

        /// <summary>
        /// Enumerable the row numbers for this page
        /// </summary>
        public IEnumerable<int> EnumerateRows() =>
            Enumerable.Range(StartRow, Size);
    }
}
