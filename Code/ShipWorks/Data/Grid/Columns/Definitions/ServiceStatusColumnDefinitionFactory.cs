﻿using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Services;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators;
using ShipWorks.Data.Grid.Columns.SortProviders;
using ShipWorks.Data.Grid.Columns.ValueProviders;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Users;

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
            GridColumnDefinitionCollection definitions = new GridColumnDefinitionCollection
            {
                new GridColumnDefinition("{F1979994-BB60-45D6-B2C1-18966AB15B65}", true,
                    new GridEnumDisplayType<ServiceStatus>(EnumSortMethod.Value),
                    "Status", "Running",
                    new GridColumnFunctionValueProvider(x => ((ServiceStatusEntity)x).GetStatus()),
                    new GridColumnSortProvider(GetStatusDescription)) 
                    { DefaultWidth = 125 },

                new GridColumnDefinition("{7BE7DBCE-3500-4B01-8594-471035C001AD}", true,
                    new GridComputerDisplayType(),
                    "Computer", @"\\ShippingPC",
                    new GridColumnFieldValueProvider(ServiceStatusFields.ComputerID),
                    new GridColumnSortProvider(x => ComputerManager.GetComputer(((ServiceStatusEntity)x).ComputerID).Name))
                    { DefaultWidth = 140 },
                    
                new GridColumnDefinition("{D5FCAF56-6931-4AA3-AD2D-3CCFB1D66098}", true, 
                    new GridDateDisplayType { UseDescriptiveDates = true },
                    "Last Started", DateTimeUtility.ParseEnUS("03/04/2012 1:30 PM").ToUniversalTime(),
                    ServiceStatusFields.LastStartDateTime) { DefaultWidth = 125 },

                new GridColumnDefinition("{50003D6C-4082-4E52-9E23-F72E2CFBA924}", true, 
                    new GridDateDisplayType { UseDescriptiveDates = true }.Decorate(new GridServiceStopTimeDecorator()),
                    "Last Stopped", DateTimeUtility.ParseEnUS("03/16/2012 7:41 PM").ToUniversalTime(),
                    ServiceStatusFields.LastStopDateTime) { DefaultWidth = 125 },

                new GridColumnDefinition("{AD6840FF-2D93-49EC-AB63-A621161285CA}", true,
                    new GridActionDisplayType(DisplayStartServiceLabel, GridLinkAction.Start), 
                    "Start", "Start",
                    new GridColumnFunctionValueProvider(x => x),
                    new GridColumnSortProvider(DisplayStartServiceLabel))
                    { DefaultWidth = 35 }
            };

            return definitions;
        }

        /// <summary>
        /// Displays the start link text based on input
        /// </summary>
        /// <param name="arg">Input on which to base whether the start link should be shown</param>
        /// <returns></returns>
        private static string DisplayStartServiceLabel(object arg)
        {
            var service = arg as ServiceStatusEntity;

            return service != null &&
                   Equals(service.Computer, UserSession.Computer) &&
                   service.GetStatus() != ServiceStatus.Running
                       ? "Start"
                       : "";
        }

        /// <summary>
        /// Gets the status description for the current service status
        /// </summary>
        /// <param name="arg">Service status entity from which to retrieve the status description</param>
        /// <returns></returns>
        private static string GetStatusDescription(EntityBase2 arg)
        {
            return EnumHelper.GetDescription(((ServiceStatusEntity) arg).GetStatus());
        }
    }
}
