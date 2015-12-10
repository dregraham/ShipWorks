using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.AddressValidation;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Data.Grid.Columns.ValueProviders;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping.Editing;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content.Controls;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using ShipWorks.Stores.Content.Panels;
using ShipWorks.Shipping.CoreExtensions.Grid;
using ShipWorks.Stores.Content.Panels.CoreExtensions.Grid;
using Interapptive.Shared.Utility;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Properties;
using ShipWorks.Shipping.ShipSense;

namespace ShipWorks.Data.Grid.Columns.Definitions
{
    /// <summary>
    /// Factory class for creating the builtin column definitions
    /// </summary>
    public static class ShipmentPanelColumnDefinitionFactory
    {
        /// <summary>
        /// Create the default grid column definitions for all possible shipment columns.
        /// </summary>
        public static GridColumnDefinitionCollection CreateDefinitions()
        {
            EntityGridAddressSelector addressSelector = new EntityGridAddressSelector("Ship");

            return new GridColumnDefinitionCollection
            {
                new GridColumnDefinition("{ACA5305A-DB99-414e-8705-5E2CF8B00509}",
                    new GridEntityDisplayType(), "Order", new GridEntityDisplayInfo(6, EntityType.OrderEntity, "Order 1028"),
                    ShipmentFields.OrderID,
                    OrderFields.OrderNumber),

                new GridColumnDefinition("{CE7A10AF-FDFF-4109-B0DE-18F47A329C5D}", true,
                    new ShipmentStatusDisplayType(), "Status", new ShipmentEntity { Processed = true },
                    ShipmentFields.Processed)  { DefaultWidth = 24 },

                new GridColumnDefinition("{B72A225D-4A57-4f06-A191-3B7BE8EDABEE}", true,
                    new ShipmentNumberDisplayType(), "Count", "1 of 2",
                    ShipmentFields.ShipmentID ) { DefaultWidth = 40 },

                new GridColumnDefinition("{DB9C8EC1-1289-48df-A29B-B67D3D391A9C}", true,
                    new GridProviderDisplayType(EnumSortMethod.Value), "Provider", ShipmentTypeCode.Endicia,
                    ShipmentFields.ShipmentType),

                new GridColumnDefinition("{98038AB5-AA95-4778-9801-574C2B723DD4}", true,
                    new ShipmentServiceUsedDisplayType(), "Service", "First Class",
                    ShipmentFields.ShipmentType),

                new GridColumnDefinition("{55C1B735-5774-453C-B2E1-30C09C6BB27F}",
                    new ShipmentInsuredDisplayType(), "Insured By", new ShipmentEntity { Insurance = true, InsuranceProvider = (int) InsuranceProvider.ShipWorks },
                    ShipmentFields.ShipmentID),

                new GridColumnDefinition("{B19232A4-FD7B-42c9-A45A-5213396C0A49}", 
                    new GridDateDisplayType { UseDescriptiveDates = true, TimeDisplayFormat = TimeDisplayFormat.None }, 
                    "Process Date", DateTimeUtility.ParseEnUS("03/04/2001 1:30").ToUniversalTime(),
                    ShipmentFields.ProcessedDate),

                new GridColumnDefinition("{0D458821-9A30-479D-AE84-1F517EC49458}", 
                    new GridUserDisplayType(), "Processed By", new object[] { "Joe", Resources.user_16 },
                    ShipmentFields.ProcessedUserID,
                    UserFields.Username),

                new GridColumnDefinition("{E8F8F052-5797-4ABC-AAD1-C751A13F0ADA}", 
                    new GridComputerDisplayType(), "Processed On", "\\ShippingPC",
                    ShipmentFields.ProcessedComputerID,
                    ComputerFields.Name),

                new GridColumnDefinition("{4037EBEF-0391-4b07-80C3-575BEB07E201}",
                    new GridDateDisplayType { UseDescriptiveDates = true, TimeDisplayFormat = TimeDisplayFormat.None }, 
                    "Ship Date", DateTimeUtility.ParseEnUS("03/04/2001 1:30").ToUniversalTime(),
                    ShipmentFields.ShipDate),

                new GridColumnDefinition("{95292493-01A9-40cc-8518-04068A6A5BA3}", 
                    new GridDateDisplayType { UseDescriptiveDates = true, TimeDisplayFormat = TimeDisplayFormat.None }, 
                    "Void Date", DateTimeUtility.ParseEnUS("03/04/2001 1:30").ToUniversalTime(),
                    ShipmentFields.VoidedDate),

                new GridColumnDefinition("{D53E7614-1771-4942-8CF9-F419B43F01A0}", 
                    new GridUserDisplayType(), "Voided By", new object[] { "Joe", Resources.user_16 },
                    ShipmentFields.VoidedUserID,
                    UserFields.Username),

                new GridColumnDefinition("{C6258887-A20F-458E-8C59-2F91DD174881}", 
                    new GridComputerDisplayType(), "Voided On", "\\ShippingPC",
                    ShipmentFields.VoidedComputerID,
                    ComputerFields.Name),

                new GridColumnDefinition("{B7195820-4EA1-4815-9E8B-9857C5D59091}",
                    new GridWeightDisplayType(), "Weight", 3.1,
                    ShipmentFields.TotalWeight),

                new GridColumnDefinition("{D12A7AE9-00F7-4e45-A420-81D7D61331CF}", true,
                    new GridTextDisplayType(), "Tracking", "1Z0139787879870954",
                    ShipmentFields.TrackingNumber),

                new GridColumnDefinition("{C83A0678-5375-4ead-A439-47193425CE11}",
                    new GridMoneyDisplayType(), "Cost", 4.18m,
                    ShipmentFields.ShipmentCost),

                new GridColumnDefinition("{30BC16E8-19B1-4D9D-90D5-AD81E29BD1DA}", false,
                    new GridEnumDisplayType<ShipSenseStatus>(EnumSortMethod.Value), "ShipSense", ShipSenseStatus.Applied,
                    ShipmentFields.ShipSenseStatus)  { DefaultWidth = 120 },

                new GridColumnDefinition("{7712114B-5E84-4ad0-9E49-5CA6EAC34B73}", true,
                    new GridShipmentEditViewDisplayType(), "Ship", "Ship",
                    ShipmentFields.ShipmentID) { DefaultWidth = 31 },

                new GridColumnDefinition("{E735E64D-9F31-4244-B803-7B4C68B31B34}", true,
                    new GridActionDisplayType("Delete", GridLinkAction.Delete), "Delete", "Delete",
                    ShipmentFields.ShipmentID) 
                    { 
                        DefaultWidth = 45,
                        ApplicableTest = (o) => o == null || UserSession.Security.HasPermission(PermissionType.ShipmentsVoidDelete, (long) o)
                    },
                    
                new GridColumnDefinition("{5BB01A4C-203C-4602-A1DD-143C8485455F}",
                    new GridEnumDisplayType<AddressValidationStatusType>(EnumSortMethod.Description),
                    "S: Validation Status", AddressValidationStatusType.Valid,
                    ShipmentFields.ShipAddressValidationStatus) 
                    { DefaultWidth = 100 },

                new GridColumnDefinition("{A0015494-C387-44D9-9A1F-466A9341B634}",
                    new GridActionDisplayType(addressSelector.DisplayValidationSuggestionLabel, 
                        addressSelector.ShowAddressOptionMenu, addressSelector.IsValidationSuggestionLinkEnabled), 
                    "S: Validation Suggestions", "2 Suggestions",
                    new GridColumnFunctionValueProvider(x => x),
                    new GridColumnSortProvider(ShipmentFields.ShipAddressValidationSuggestionCount, ShipmentFields.ShipAddressValidationStatus))
                    { DefaultWidth = 120 }, 

                new GridColumnDefinition("{70F17C2C-74AD-4AF3-8E44-1DA5597A92F6}",
                    new GridEnumDisplayType<ValidationDetailStatusType>(EnumSortMethod.Description),
                    "S: Residential status",  ValidationDetailStatusType.Yes,
                    ShipmentFields.ShipResidentialStatus) 
                    { DefaultWidth = 100 }, 

                new GridColumnDefinition("{3AF5D697-F319-48E5-87B7-500D3186AABE}",
                    new GridEnumDisplayType<ValidationDetailStatusType>(EnumSortMethod.Description),
                    "S: PO Box", ValidationDetailStatusType.Yes,
                    ShipmentFields.ShipPOBox) 
                    { DefaultWidth = 72 }, 

                new GridColumnDefinition("{2BAFB097-004D-4DE5-9779-F4D86854943E}",
                    new GridEnumDisplayType<ValidationDetailStatusType>(EnumSortMethod.Description),
                    "S: International Territory",  ValidationDetailStatusType.Yes,
                    ShipmentFields.ShipUSTerritory) 
                    { DefaultWidth = 145 }, 

                new GridColumnDefinition("{8097B252-BB97-412E-9DA4-CC1D8ADC0007}",
                    new GridEnumDisplayType<ValidationDetailStatusType>(EnumSortMethod.Description),
                    "S: Military Address",  ValidationDetailStatusType.Yes,
                    ShipmentFields.ShipMilitaryAddress) 
                    { DefaultWidth = 115 },

                new GridColumnDefinition("{72E01539-A415-4D49-B08C-B96FA551CCE9}", false,
                    new GridEnumDisplayType<ThermalLanguage>(EnumSortMethod.Description), "Requested Label Format", ThermalLanguage.None,
                    ShipmentFields.RequestedLabelFormat)  { DefaultWidth = 60 },

                new GridColumnDefinition("{ADB80862-F35B-4F79-8405-607E329155F1}", false,
                    new GridActualLabelFormatDisplayType(), "Actual Label Format", ThermalLanguage.None,
                    ShipmentFields.ActualLabelFormat)  { DefaultWidth = 60 },
            };
        }
    }
}
