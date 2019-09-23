using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.GenericFile.Sources.Disk;
using ShipWorks.Stores.Platforms.GenericFile.Sources.FTP;
using ShipWorks.Stores.Platforms.GenericFile.Sources.Email;

namespace ShipWorks.Stores.Platforms.GenericFile.Sources
{
    /// <summary>
    /// Provides the generic flat file import file sources
    /// </summary>
    public static class GenericFileSourceTypeManager
    {
        /// <summary>
        /// Returns all file sources
        /// </summary>
        public static List<GenericFileSourceType> FileSources
        {
            get
            {
                return new List<GenericFileSourceType>
                    {
                        GetFileSourceType(GenericFileSourceTypeCode.Disk),
                        GetFileSourceType(GenericFileSourceTypeCode.FTP),
                        GetFileSourceType(GenericFileSourceTypeCode.Email),
                        GetFileSourceType(GenericFileSourceTypeCode.Warehouse)
                    };
            }
        }

        /// <summary>
        /// Get the file source of the given type
        /// </summary>
        public static GenericFileSourceType GetFileSourceType(GenericFileSourceTypeCode typeCode)
        {
            switch (typeCode)
            {
                case GenericFileSourceTypeCode.Disk: return new GenericFileSourceDiskType();
                case GenericFileSourceTypeCode.FTP: return new GenericFileSourceFtpType();
                case GenericFileSourceTypeCode.Email: return new GenericFileSourceEmailType();
                case GenericFileSourceTypeCode.Warehouse: return new GenericFileSourceWarehouseType();
            }

            throw new InvalidOperationException("Invalid GenericFileSourceType: " + typeCode);
        }
    }
}
