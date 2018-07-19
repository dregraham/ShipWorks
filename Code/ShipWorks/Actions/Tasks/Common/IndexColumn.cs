namespace ShipWorks.Actions.Tasks.Common
{
    public class IndexColumn
    {
        private string columnName;
        private bool isIncluded;

        public IndexColumn(string columnName, bool isIncluded)
        {
            this.columnName = columnName;
            this.isIncluded = isIncluded;
        }
    }
}