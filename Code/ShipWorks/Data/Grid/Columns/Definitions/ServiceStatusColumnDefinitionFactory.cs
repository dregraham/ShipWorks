﻿using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.WindowsServices;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators;
using ShipWorks.Data.Grid.Columns.SortProviders;
using ShipWorks.Data.Grid.Columns.ValueProviders;
using ShipWorks.Data.Model.EntityClasses;
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
                    { DefaultWidth = 140 },

                new GridColumnDefinition("{367F6C99-2563-4DA4-B307-7F8C8892937A}", true,
                    new GridEnumDisplayType<ShipWorksServiceType>(EnumSortMethod.Value),
                    "Service Type", "Sample Service",
                    WindowsServiceFields.ServiceType)
                    { DefaultWidth = 140 },

                new GridColumnDefinition("{F1979994-BB60-45D6-B2C1-18966AB15B65}", true,
                    new GridEnumDisplayType<ServiceStatus>(EnumSortMethod.Value),
                    "Status", "(icon)",
                    new GridColumnFunctionValueProvider(x => ((WindowsServiceEntity)x).GetStatus()),
                    null) //new GridColumnSortProvider(CreateStatusSortField()))
                    { DefaultWidth = 125 },

                new GridColumnDefinition("{D5FCAF56-6931-4AA3-AD2D-3CCFB1D66098}", true, 
                    new GridDateDisplayType { UseDescriptiveDates = true },
                    "Last Started", DateTimeUtility.ParseEnUS("03/04/2012 1:30 PM").ToUniversalTime(),
                    WindowsServiceFields.LastStartDateTime),

                new GridColumnDefinition("{50003D6C-4082-4E52-9E23-F72E2CFBA924}", true, 
                    new GridDateDisplayType { UseDescriptiveDates = true }.Decorate(new GridServiceStopTimeDecorator()),
                    "Last Stopped", DateTimeUtility.ParseEnUS("03/16/2012 7:41 PM").ToUniversalTime(),
                    WindowsServiceFields.LastStopDateTime)
            };

            return definitions;
        }


        /// <summary>
        /// Mirrors the logic in <see cref="WindowsServiceEntityExtensions.GetStatus"/>.
        /// </summary>
        //static EntityField2 CreateStatusSortField()
        //{
        //    var expression = new StringBuilder()
        //        .Append(" CASE WHEN LastStartDateTime IS NULL THEN ")
        //            .Append((int)ServiceStatus.NeverRun)
        //        .Append(" ELSE ")
        //            .Append(" CASE WHEN LastStopDateTime > LastStartDateTime THEN ")
        //                .Append((int)ServiceStatus.Stopped)
        //            .Append(" ELSE ")
        //                .Append(" CASE WHEN LastCheckInDateTime IS NULL OR DATEDIFF(MINUTE, LastCheckInDateTime, GETUTCDATE()) > 10 THEN ")
        //                    .Append((int)ServiceStatus.NotResponding)
        //                .Append(" ELSE ")
        //                    .Append((int)ServiceStatus.Running)
        //                .Append(" END ")
        //            .Append(" END ")
        //        .Append(" END ")
        //        .ToString();

        //    return WindowsServiceFields.LastCheckInDateTime.SetExpression(new DbFunctionCall(expression, null));
        //}
    }
}
