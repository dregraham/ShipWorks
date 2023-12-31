﻿using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Replaces ODBC Mappings to OrderNumberComplete from OrderNumber
    /// </summary>
    [Component]
    public class OdbcStoreUpgrade : IStoreUpgrade
    {
        private readonly IOdbcFieldMapFactory fieldMapFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcStoreUpgrade(IOdbcFieldMapFactory fieldMapFactory)
        {
            this.fieldMapFactory = fieldMapFactory;
        }

        /// <summary>
        /// Replaces ODBC Mappings to OrderNumber to OrderNumberComplete
        /// </summary>
        public void Upgrade(StoreEntity store)
        {
            OdbcStoreEntity odbcStore = store as OdbcStoreEntity;
            if (odbcStore != null)
            {
                IOdbcFieldMap importMap = fieldMapFactory.CreateFieldMapFrom(odbcStore.ImportMap);
                importMap.UpgradeToAlphanumericOrderNumbers();
                odbcStore.ImportMap = importMap.Serialize();

                IOdbcFieldMap uploadMap = fieldMapFactory.CreateFieldMapFrom(odbcStore.UploadMap);
                uploadMap.UpgradeToAlphanumericOrderNumbers();
                odbcStore.UploadMap = uploadMap.Serialize();
            }
        }
    }
}
