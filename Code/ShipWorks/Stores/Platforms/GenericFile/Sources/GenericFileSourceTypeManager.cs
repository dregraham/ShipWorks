using System;
using System.Collections.Generic;
using ShipWorks.Stores.Platforms.GenericFile.Sources.Disk;
using ShipWorks.Stores.Platforms.GenericFile.Sources.FTP;
using ShipWorks.Stores.Platforms.GenericFile.Sources.Email;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using Autofac;
using ShipWorks.Editions;

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
        public static List<GenericFileSourceType> GetFileSources(GenericFileStoreEntity store, bool newStore)
        {

            var result = new List<GenericFileSourceType>
                {
                    GetFileSourceType(GenericFileSourceTypeCode.Disk),
                    GetFileSourceType(GenericFileSourceTypeCode.FTP),
                    GetFileSourceType(GenericFileSourceTypeCode.Email)
                };

            if (IncludeWarehouseOption(store, newStore))
            {
                result.Add(GetFileSourceType(GenericFileSourceTypeCode.Warehouse));
            }

            return result;
        }

        /// <summary>
        /// Include warehouse if the store is not new and this is a warehouse user
        /// </summary>
        private static bool IncludeWarehouseOption(GenericFileStoreEntity store, bool newStore)
        {
            using (var scope = IoC.BeginLifetimeScope())
            {
                ILicenseService licenseService = scope.Resolve<ILicenseService>();
                EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.Warehouse, null);

                if (!newStore && restrictionLevel == EditionRestrictionLevel.None)
                {
                    return true;
                }
            }

            return false;
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
