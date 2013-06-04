using System;
using System.Collections.Generic;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.Filters.Content.SqlGeneration
{
    /// <summary>
    /// The sql templates used to generate the sql output for filter generation
    /// </summary>
    public static class FilterSqlTemplates
    {
        static string filterInitial;
        static string filterUpdate;
        static string folderEmpty;
        static string folderInitial;
        static string folderUpdate;

        /// <summary>
        /// Static constructor
        /// </summary>
        static FilterSqlTemplates()
        {
            filterInitial = ResourceUtility.ReadString("ShipWorks.Filters.Content.SqlGeneration.Templates.FilterInitial.sql");
            filterUpdate  = ResourceUtility.ReadString("ShipWorks.Filters.Content.SqlGeneration.Templates.FilterUpdate.sql");
            folderEmpty   = ResourceUtility.ReadString("ShipWorks.Filters.Content.SqlGeneration.Templates.FolderEmpty.sql");
            folderInitial = ResourceUtility.ReadString("ShipWorks.Filters.Content.SqlGeneration.Templates.FolderInitial.sql");
            folderUpdate  = ResourceUtility.ReadString("ShipWorks.Filters.Content.SqlGeneration.Templates.FolderUpdate.sql");
        }

        /// <summary>
        /// SQL template for the initial calculation of a filter
        /// </summary>
        public static string FilterInitial
        {
            get { return filterInitial; }
        }

        /// <summary>
        /// SQL template for the update calculation of a filter
        /// </summary>
        public static string FilterUpdate
        {
            get { return filterUpdate; }
        }

        /// <summary>
        /// SQL template for when a folder has no child filters or folders
        /// </summary>
        public static string FolderEmpty
        {
            get { return folderEmpty; }
        }

        /// <summary>
        /// SQL template for the initial calculation of a folder
        /// </summary>
        public static string FolderInitial
        {
            get { return folderInitial; }
        }

        /// <summary>
        /// SQL template for the update calculation of a folder
        /// </summary>
        public static string FolderUpdate
        {
            get { return folderUpdate; }
        }
    }
}
