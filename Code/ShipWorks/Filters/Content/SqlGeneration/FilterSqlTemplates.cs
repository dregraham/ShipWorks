using Interapptive.Shared.Utility;

namespace ShipWorks.Filters.Content.SqlGeneration
{
    /// <summary>
    /// The sql templates used to generate the sql output for filter generation
    /// </summary>
    public static class FilterSqlTemplates
    {
        /// <summary>
        /// Static constructor
        /// </summary>
        static FilterSqlTemplates()
        {
            FilterInitial = ResourceUtility.ReadString("ShipWorks.Filters.Content.SqlGeneration.Templates.FilterInitial.sql");
            FilterUpdate  = ResourceUtility.ReadString("ShipWorks.Filters.Content.SqlGeneration.Templates.FilterUpdate.sql");

            FolderEmpty   = ResourceUtility.ReadString("ShipWorks.Filters.Content.SqlGeneration.Templates.FolderEmpty.sql");
            FolderInitial = ResourceUtility.ReadString("ShipWorks.Filters.Content.SqlGeneration.Templates.FolderInitial.sql");
            FolderUpdate  = ResourceUtility.ReadString("ShipWorks.Filters.Content.SqlGeneration.Templates.FolderUpdate.sql");

            ExistsQuery = ResourceUtility.ReadString("ShipWorks.Filters.Content.SqlGeneration.Templates.ExistsQuery.sql");
            ExistsQueryEmpty = ResourceUtility.ReadString("ShipWorks.Filters.Content.SqlGeneration.Templates.ExistsQueryEmpty.sql");
            ExistsQueryFolder = ResourceUtility.ReadString("ShipWorks.Filters.Content.SqlGeneration.Templates.ExistsQueryFolder.sql");
        }

        /// <summary>
        /// SQL template for the initial calculation of a filter
        /// </summary>
        public static string FilterInitial { get; }

        /// <summary>
        /// SQL template for the update calculation of a filter
        /// </summary>
        public static string FilterUpdate { get; }

        /// <summary>
        /// SQL template for the determining if an entity would be in the filter.
        /// </summary>
        public static string ExistsQuery { get; }

        /// <summary>
        /// Exists SQL template for when a folder has no child filters or folders
        /// </summary>
        public static string ExistsQueryEmpty { get; }

        /// <summary>
        /// Exists SQL template for a folder
        /// </summary>
        public static string ExistsQueryFolder { get; }

        /// <summary>
        /// SQL template for when a folder has no child filters or folders
        /// </summary>
        public static string FolderEmpty { get; }

        /// <summary>
        /// SQL template for the initial calculation of a folder
        /// </summary>
        public static string FolderInitial { get; }

        /// <summary>
        /// SQL template for the update calculation of a folder
        /// </summary>
        public static string FolderUpdate { get; }
    }
}
