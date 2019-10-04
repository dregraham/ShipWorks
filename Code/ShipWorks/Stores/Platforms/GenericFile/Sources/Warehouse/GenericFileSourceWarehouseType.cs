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
    public class GenericFileSourceWarehouseType : GenericFileSourceType
    {
        /// <summary>
        /// The GenericFileSourceType represented by this source object
        /// </summary>
        public override GenericFileSourceTypeCode FileSourceTypeCode
        {
            get { return GenericFileSourceTypeCode.Warehouse; }
        }

        /// <summary>
        /// The user-visible description that can be seen when choosing the type
        /// </summary>
        public override string Description
        {
            get { return "The ShipWorks Hub";  }
        }

        public override GenericFileSource CreateFileSource(GenericFileStoreEntity store)
        {
            return null;
        }

        public override GenericFileSourceSettingsControlBase CreateSettingsControl()
        {
            return new GenericFileSourceSettingsControlBase();
        }
    }
}
