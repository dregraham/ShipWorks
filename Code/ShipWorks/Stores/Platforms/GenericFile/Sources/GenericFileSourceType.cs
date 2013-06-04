using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.GenericFile.Sources
{
    /// <summary>
    /// Abstract base for all flat file data sources
    /// </summary>
    public abstract class GenericFileSourceType
    {
        /// <summary>
        /// The GenericFileSourceType represented by this source object
        /// </summary>
        public abstract GenericFileSourceTypeCode FileSourceTypeCode { get; }

        /// <summary>
        /// The user-visible description that can be seen when choosing the type
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Create the settings control used to edit the settings for the type
        /// </summary>
        public abstract GenericFileSourceSettingsControlBase CreateSettingsControl();

        /// <summary>
        /// Create a file source instance for this type based on the configuration of the store.
        /// </summary>
        public abstract GenericFileSource CreateFileSource(GenericFileStoreEntity store);
    }
}
