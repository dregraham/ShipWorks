using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.GenericFile.Sources.FTP
{
    /// <summary>
    /// FTP source type
    /// </summary>
    public class GenericFileSourceFtpType : GenericFileSourceType
    {
        /// <summary>
        /// The GenericFileSourceType represented by this source object
        /// </summary>
        public override GenericFileSourceTypeCode FileSourceTypeCode
        {
            get { return GenericFileSourceTypeCode.FTP; }
        }

        /// <summary>
        /// The user-visible description that can be seen when choosing the type
        /// </summary>
        public override string Description
        {
            get { return "An FTP folder"; }
        }

        /// <summary>
        /// Create the settings control used to edit the settings for the type
        /// </summary>
        public override GenericFileSourceSettingsControlBase CreateSettingsControl()
        {
            return new GenericFileSourceFtpControl();
        }

        /// <summary>
        /// Create the file source instance based on the configuration in the store
        /// </summary>
        public override GenericFileSource CreateFileSource(GenericFileStoreEntity store)
        {
            return new GenericFileFtpSource(store);
        }
    }
}
