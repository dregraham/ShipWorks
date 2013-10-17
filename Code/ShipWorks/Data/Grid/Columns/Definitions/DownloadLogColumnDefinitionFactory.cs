using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Communication;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms;

namespace ShipWorks.Data.Grid.Columns.Definitions
{
    /// <summary>
    /// Factory class for creating the builtin column definitions
    /// </summary>
    public static class DownloadLogColumnDefinitionFactory
    {
        /// <summary>
        /// Create the default grid column definitions for all possible download log columns.
        /// </summary>
        public static GridColumnDefinitionCollection CreateDefinitions()
        {
            GridColumnDefinitionCollection definitions = new GridColumnDefinitionCollection
                {
                    new GridColumnDefinition("{B5E42492-471F-43ca-A092-11486DB64C93}", true, 
                        new GridEnumDisplayType<DownloadResult>(EnumSortMethod.Value), "Result", DownloadResult.Success,
                        DownloadFields.Result) { DefaultWidth = 95 },

                    new GridColumnDefinition("{918DF5BD-A5DD-46e3-A746-0CC2149848EB}", true, 
                        new GridDateDisplayType { UseDescriptiveDates = true }, "Started", DateTimeUtility.ParseEnUS("03/04/2001 1:30 PM").ToUniversalTime(),
                        DownloadFields.Started),

                    new GridColumnDefinition("{75519F5D-F32B-486f-AB81-4CCE4DCE3580}",
                        new GridDateDisplayType { ShowDate = false }, "Ended", DateTimeUtility.ParseEnUS("03/04/2001 1:35 PM").ToUniversalTime(),
                        DownloadFields.Ended),

                    new GridColumnDefinition("{CADE1B04-49D8-448d-866E-71F055F8B20E}", true,
                        new GridDurationDisplayType(), "Duration", TimeSpan.FromSeconds(65),
                        DownloadFields.Duration),

                    new GridColumnDefinition("{B9D08195-2BF1-440f-B020-61062408A47D}", true,
                        new GridTextDisplayType(), "Orders", 14,
                        DownloadFields.QuantityTotal) { DefaultWidth = 60 },

                    new GridColumnDefinition("{06E46F0D-AF36-4d02-8EF1-64931CF56BE7}", true,
                        new GridTextDisplayType(), "New", 14,
                        DownloadFields.QuantityNew) { DefaultWidth = 60 },

                    new GridColumnDefinition("{E36583A3-1535-42a0-AD1B-BFEC9C699D64}", true,
                        new GridStoreDisplayType(StoreProperty.StoreName), "Store Name", new GridStoreDisplayType.DisplayData { StoreText = "My Store", StoreIcon = StoreIcons.magento },
                        DownloadFields.StoreID,
                        StoreFields.StoreName),

                    new GridColumnDefinition("{A88E4F26-73CA-4a4b-AB27-9679EBA0270D}", true,
                        new GridComputerDisplayType(), "Computer", "\\ShippingPC",
                        DownloadFields.ComputerID,
                        ComputerFields.Name),

                    new GridColumnDefinition("{6234F47E-CDE0-499f-A459-AB3E745CC620}",
                        new GridEnumDisplayType<DownloadInitiatedBy>(EnumSortMethod.Value), "Initiated By", DownloadInitiatedBy.User,
                        DownloadFields.InitiatedBy),
                };

            // Return the definition set
            return definitions;
       }
    }
}
