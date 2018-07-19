namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Column in an index
    /// </summary>
    public class IndexColumn
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public IndexColumn(string columnName, bool isInclude)
        {
            Name = columnName;
            IsInclude = isInclude;
        }

        /// <summary>
        /// Name of the column
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Is the column an include in the index
        /// </summary>
        public bool IsInclude { get; }
    }
}