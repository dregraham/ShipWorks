using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Replaces ODBC Mappings to OrderNumber to OrderNumberComplete
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

                IOdbcFieldMap uploadMap = fieldMapFactory.CreateFieldMapFrom(odbcStore.UploadMap);
                uploadMap.UpgradeToAlphanumericOrderNumbers();
            }
        }
    }
}
