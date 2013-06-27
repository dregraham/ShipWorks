using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.WindowsServices;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Grid.Columns.SortProviders;
using ShipWorks.Data.Grid.Columns.ValueProviders;
using ShipWorks.Data.Model.HelperClasses;


namespace ShipWorks.Data.Grid.Columns.Definitions
{
    /// <summary>
    /// Creates the column definitions for service status.
    /// </summary>
    public static class ServiceStatusColumnDefinitionFactory
    {
        /// <summary>
        /// Creates the default grid column definitions for all possible service status columns.
        /// </summary>
        public static GridColumnDefinitionCollection CreateDefinitions()
        {
            var definitions = new GridColumnDefinitionCollection
            {
                new GridColumnDefinition("{7BE7DBCE-3500-4B01-8594-471035C001AD}", true,
                    new GridComputerDisplayType(),
                    "Computer", @"\\ShippingPC",
                    new GridColumnFieldValueProvider(WindowsServiceFields.ComputerID),
                    new GridColumnAdvancedSortProvider(ComputerFields.Name, ComputerFields.ComputerID, WindowsServiceFields.ComputerID, JoinHint.Right))
                    { DefaultWidth = 125 },

                new GridColumnDefinition("{367F6C99-2563-4DA4-B307-7F8C8892937A}", true,
                    new GridEnumDisplayType<ShipWorksServiceType>(EnumSortMethod.Value),
                    "Service Type", "ShipWorks Sample Service",
                    WindowsServiceFields.ServiceType)
                    { DefaultWidth = 160 },

                new GridColumnDefinition("{D5FCAF56-6931-4AA3-AD2D-3CCFB1D66098}", true, 
                    new GridDateDisplayType { }, "Last Check-in", Interapptive.Shared.Utility.DateTimeUtility.ParseEnUS("03/04/2001 1:30 PM").ToUniversalTime(),
                    WindowsServiceFields.LastCheckInDateTime)
            };

            return definitions;
        }
    }
}
