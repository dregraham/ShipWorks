using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores
{
    /// <summary>
    /// The result of creating filters
    /// </summary>
    public class StoreFilterRepositorySaveResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether [folder created].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [folder created]; otherwise, <c>false</c>.
        /// </value>
        public bool FolderCreated { get; set; }

        /// <summary>
        /// Gets or sets the name of the store folder.
        /// </summary>
        /// <value>
        /// The name of the store folder.
        /// </value>
        public string StoreFolderName { get; set; }

        /// <summary>
        /// Gets or sets the created filters.
        /// </summary>
        /// <value>
        /// The created filters.
        /// </value>
        public List<FilterEntity> CreatedFilters { get; set; }

        /// <summary>
        /// Gets or sets the collision filters - These were not created
        /// </summary>
        /// <value>
        /// The collision filters that couldn't be created
        /// </value>
        public List<FilterEntity> CollisionFilters { get; set; }
    }
}
