namespace ShipWorks.Data.Grid.Paging
{
    /// <summary>
    /// Manage paging information
    /// </summary>
    public struct Pager
    {
        private readonly int pageSize;

        /// <summary>
        /// Constructor
        /// </summary>
        public Pager(int pageSize)
        {
            this.pageSize = pageSize;
        }

        /// <summary>
        /// Get the page for a given row
        /// </summary>
        public PageDetails PageForRow(int row) =>
            new PageDetails(pageSize, row);
    }
}
