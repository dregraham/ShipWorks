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
    /// Grid column type for displaying order numbers
    /// </summary>
    public class GridOrderNumberDisplayType : GridColumnDisplayType
    {
        bool showStoreIcon = true;

        /// <summary>
        /// Class that the data gets transfered to display
        /// </summary>
        public class DisplayData
        {
            public string OrderNumber { get; set; }
            public Image StoreIcon { get; set; }
        }

        /// <summary>
        /// Construct a new instance to display data from the given store property
        /// </summary>
        public GridOrderNumberDisplayType()
        {

        }

        /// <summary>
        /// The format editor for the display type.
        /// </summary>
        public override GridColumnDisplayEditor CreateEditor()
        {
            return new GridOrderNumberDisplayEditor(this);
        }

        /// <summary>
        /// Indicates if the icon representing the store type
        /// </summary>
        public bool ShowStoreIcon
        {
            get { return showStoreIcon; }
            set { showStoreIcon = value; }
        }

        /// <summary>
        /// Provide the data to be used by the display functions
        /// </summary>
        protected override object GetEntityValue(EntityBase2 entity)
        {
            OrderEntity order = entity as OrderEntity;

            if (order != null)
            {
                DisplayData displayData = new DisplayData();
                displayData.OrderNumber = base.GetEntityValue(order) as string;

                if (!string.IsNullOrEmpty(displayData.OrderNumber) && showStoreIcon)
                {
                    StoreEntity store = StoreManager.GetStore(order.StoreID);

                    if (store != null)
                    {
                        displayData.StoreIcon = EnumHelper.GetImage((StoreTypeCode) store.TypeCode);
                    }
                }

                return displayData;
            }

            return null;
        }

        /// <summary>
        /// Return the order number
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            DisplayData displayData = (DisplayData) value;

            if (displayData == null)
            {
                return "";
            }

            return displayData.OrderNumber;
        }

        /// <summary>
        /// Get the image to display for the store
        /// </summary>
        protected override Image GetDisplayImage(object value)
        {
            DisplayData displayData = (DisplayData) value;

            if (displayData == null || !showStoreIcon)
            {
                return null;
            }

            return displayData.StoreIcon;
        }

        /// <summary>
        /// Sample data to use for grid column previews
        /// </summary>
        public static DisplayData SampleData(StoreTypeCode storeType, string orderNumber = "1084")
        {
            return new DisplayData { OrderNumber = orderNumber, StoreIcon = EnumHelper.GetImage(storeType) };
        }
    }
}
