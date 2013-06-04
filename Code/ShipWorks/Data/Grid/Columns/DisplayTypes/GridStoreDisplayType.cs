using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Grid column type for displaying the name of the related store
    /// </summary>
    public class GridStoreDisplayType : GridColumnDisplayType
    {
        StoreProperty property;

        /// <summary>
        /// Construct a new instance to display data from the given store property
        /// </summary>
        public GridStoreDisplayType(StoreProperty property)
        {
            this.property = property;
            this.PreviewInputType = GridColumnPreviewInputType.LiteralString;
        }

        /// <summary>
        /// Format the StoreID into the StoreName
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            long storeID = (long) value;
            StoreEntity store = StoreManager.GetStore(storeID);

            // Has been deleted, or just created and we havnt loaded it yet
            if (store == null)
            {
                return "";
            }

            switch (property)
            {
                case StoreProperty.StoreName:
                    return store.StoreName;

                case StoreProperty.StoreType:
                    return StoreTypeManager.GetType(store).StoreTypeName;

                default:
                    throw new InvalidOperationException(string.Format("Unhandled StoreProperty value {0}", property));
            }
        }
    }
}
