using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.GenericFile.Sources.Email
{
    /// <summary>
    /// Email source type
    /// </summary>
    public class GenericFileSourceEmailType : GenericFileSourceType
    {
        /// <summary>
        /// The GenericFileSourceType represented by this source object
        /// </summary>
        public override GenericFileSourceTypeCode FileSourceTypeCode
        {
            get { return GenericFileSourceTypeCode.Email; }
        }

        /// <summary>
        /// The user-visible description that can be seen when choosing the type
        /// </summary>
        public override string Description
        {
            get { return "Incoming email";  }
        }

        /// <summary>
        /// Create the settings control used to edit the settings for the type
        /// </summary>
        public override GenericFileSourceSettingsControlBase CreateSettingsControl()
        {
            return new GenericFileSourceEmailControl();
        }

        /// <summary>
        /// Create the file source instance based on the configuration in the store
        /// </summary>
        public override GenericFileSource CreateFileSource(GenericFileStoreEntity store)
        {
            return new GenericFileEmailSource(store);
        }
    }
}
