using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.GenericFile.Sources.Disk
{
    /// <summary>
    /// Disk file source
    /// </summary>
    public class GenericFileSourceDiskType : GenericFileSourceType
    {
        /// <summary>
        /// The GenericFileSourceType represented by this source object
        /// </summary>
        public override GenericFileSourceTypeCode FileSourceTypeCode
        {
            get { return GenericFileSourceTypeCode.Disk; }
        }

        /// <summary>
        /// The user-visible description that can be seen when choosing the type
        /// </summary>
        public override string Description
        {
            get { return "A folder on your computer or local network";  }
        }

        /// <summary>
        /// Create the settings control used to edit the settings for the type
        /// </summary>
        public override GenericFileSourceSettingsControlBase CreateSettingsControl()
        {
            return new GenericFileSourceDiskControl();
        }

        /// <summary>
        /// Create the file source instance based on the configuration in the store
        /// </summary>
        public override GenericFileSource CreateFileSource(GenericFileStoreEntity store)
        {
            return new GenericFileDiskSource(store);
        }
    }
}
