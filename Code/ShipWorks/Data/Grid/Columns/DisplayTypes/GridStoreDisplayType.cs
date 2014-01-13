using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using System.Drawing;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Grid column type for displaying the name of the related store
    /// </summary>
    public class GridStoreDisplayType : GridColumnDisplayType
    {
        bool showIcon = true;

        StoreProperty property;

        /// <summary>
        /// Class that the data gets transfered to display
        /// </summary>
        public class DisplayData
        {
            public string StoreText { get; set; }
            public Image StoreIcon { get; set; }
        }

        /// <summary>
        /// Construct a new instance to display data from the given store property
        /// </summary>
        public GridStoreDisplayType(StoreProperty property)
        {
            this.property = property;
        }

        /// <summary>
        /// The format editor for the display type.
        /// </summary>
        public override GridColumnDisplayEditor CreateEditor()
        {
            return new GridStoreDisplayEditor(this);
        }

        /// <summary>
        /// Indicates if the icon representing the store type should be displayed
        /// </summary>
        public bool ShowIcon
        {
            get { return showIcon; }
            set { showIcon = value; }
        }

        /// <summary>
        /// Provide the data to be used by the display functions
        /// </summary>
        protected override object GetEntityValue(EntityBase2 entity)
        {
            object value = base.GetEntityValue(entity);

            if (value != null)
            {
                long storeID = (long) value;
                StoreEntity store = StoreManager.GetStore(storeID);

                if (store != null)
                {
                    DisplayData displayData = new DisplayData();

                    switch (property)
                    {
                        case StoreProperty.StoreName:
                            displayData.StoreText = store.StoreName;
                            break;

                        case StoreProperty.StoreType:
                            displayData.StoreText = StoreTypeManager.GetType(store).StoreTypeName;
                            break;

                        default:
                            throw new InvalidOperationException(string.Format("Unhandled StoreProperty value {0}", property));
                    }

                    if (showIcon)
                    {
                        displayData.StoreIcon = EnumHelper.GetImage((StoreTypeCode) store.TypeCode);
                    }

                    return displayData;
                }
            }

            return null;
        }

        /// <summary>
        /// Format the display data into the display text
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            DisplayData displayData = (DisplayData) value;

            if (displayData == null)
            {
                return "";
            }

            return displayData.StoreText;
        }

        /// <summary>
        /// Get the image to display for the store
        /// </summary>
        protected override Image GetDisplayImage(object value)
        {
            DisplayData displayData = (DisplayData) value;

            if (displayData == null || !showIcon)
            {
                return null;
            }

            return displayData.StoreIcon;
        }
    }
}
