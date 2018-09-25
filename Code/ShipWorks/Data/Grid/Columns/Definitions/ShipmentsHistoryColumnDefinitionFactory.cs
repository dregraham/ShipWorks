using Interapptive.Shared.Utility;
using ShipWorks.AddressValidation;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Grid.Columns.ValueProviders;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Properties;
using ShipWorks.Shipping;
using ShipWorks.Shipping.CoreExtensions.Grid;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.ShipSense;

namespace ShipWorks.Data.Grid.Columns.Definitions
{
    /// <summary>
    /// Factory class for creating the builtin column definitions
    /// </summary>
    public static class ShipmentsHistoryColumnDefinitionFactory
    {
        /// <summary>
        /// Create the default grid column definitions for all possible shipment columns.
        /// </summary>
        public static GridColumnDefinitionCollection CreateDefinitions()
        {
            EntityGridAddressSelector addressSelector = new EntityGridAddressSelector("Ship");

            return new GridColumnDefinitionCollection
            {
                new GridColumnDefinition("{175794CD-E6A4-444A-9019-12F9D704C2C8}",
                    new GridEntityDisplayType { AllowHyperlink = false }, "Order", new GridEntityDisplayInfo(6, EntityType.OrderEntity, "Order 1028"),
                    ShipmentFields.OrderID,
                    OrderFields.OrderNumber),

                new GridColumnDefinition("{84970902-4F39-4EAC-BFD7-C00ED0F4F8F4}", true,
                    new ShipmentStatusDisplayType(), "Status", new ShipmentEntity { Processed = true },
                    ShipmentFields.Processed)  { DefaultWidth = 24 },

                new GridColumnDefinition("{651073F0-AE91-4BAC-A79A-DC3C89A258DA}", true,
                    new ShipmentNumberDisplayType(), "Count", "1 of 2",
                    ShipmentFields.ShipmentID ) { DefaultWidth = 40 },

                new GridColumnDefinition("{9449449A-7332-4472-85BD-378AD5433DE3}", true,
                    new GridProviderDisplayType(EnumSortMethod.Value), "Provider", ShipmentTypeCode.Endicia,
                    ShipmentFields.ShipmentType),

                new GridColumnDefinition("{B8497103-6434-4DAE-A0E1-D75A4C27560E}", true,
                    new ShipmentServiceUsedDisplayType(), "Service", "First Class",
                    ShipmentFields.ShipmentType),

                new GridColumnDefinition("{0F413267-F462-45A2-AFBA-F27EAF268E5C}",
                    new ShipmentInsuredDisplayType(), "Insured By", new ShipmentEntity { Insurance = true, InsuranceProvider = (int) InsuranceProvider.ShipWorks },
                    ShipmentFields.ShipmentID),

                new GridColumnDefinition("{A11F61D6-0630-4BA6-9F1D-4A0A5B06C131}",
                    new GridDateDisplayType { UseDescriptiveDates = true, TimeDisplayFormat = TimeDisplayFormat.None },
                    "Process Date", DateTimeUtility.ParseEnUS("03/04/2001 1:30").ToUniversalTime(),
                    ShipmentFields.ProcessedDate),

                new GridColumnDefinition("{5BA29E77-584E-4E36-8761-206F7002260D}",
                    new GridUserDisplayType(), "Processed By", new object[] { "Joe", Resources.user_16 },
                    ShipmentFields.ProcessedUserID,
                    UserFields.Username),

                new GridColumnDefinition("{7A3953A2-91FC-4385-A77F-251F49C4CD4C}",
                    new GridComputerDisplayType(), "Processed On", "\\ShippingPC",
                    ShipmentFields.ProcessedComputerID,
                    ComputerFields.Name),

                new GridColumnDefinition("{CC32BBA2-B1D6-4644-8500-4CEC0DE401C4}",
                    new GridDateDisplayType { UseDescriptiveDates = true, TimeDisplayFormat = TimeDisplayFormat.None },
                    "Ship Date", DateTimeUtility.ParseEnUS("03/04/2001 1:30").ToUniversalTime(),
                    ShipmentFields.ShipDate),

                new GridColumnDefinition("{3A994FCF-C701-4969-AF49-3AD55A2F33E7}",
                    new GridDateDisplayType { UseDescriptiveDates = true, TimeDisplayFormat = TimeDisplayFormat.None },
                    "Void Date", DateTimeUtility.ParseEnUS("03/04/2001 1:30").ToUniversalTime(),
                    ShipmentFields.VoidedDate),

                new GridColumnDefinition("{877CEDD3-F2E6-497F-8E74-9119791C1083}",
                    new GridUserDisplayType(), "Voided By", new object[] { "Joe", Resources.user_16 },
                    ShipmentFields.VoidedUserID,
                    UserFields.Username),

                new GridColumnDefinition("{0B136818-6FCB-455B-95CE-F3FDF20B342C}",
                    new GridComputerDisplayType(), "Voided On", "\\ShippingPC",
                    ShipmentFields.VoidedComputerID,
                    ComputerFields.Name),

                new GridColumnDefinition("{7AC435CD-3D41-4DCE-A97C-4912A3E52744}",
                    new GridWeightDisplayType(), "Weight", 3.1,
                    ShipmentFields.TotalWeight),

                new GridColumnDefinition("{1B81177C-5B5C-4F02-B1AD-CBADAE84B0B1}", true,
                    new GridTextDisplayType(), "Tracking", "1Z0139787879870954",
                    ShipmentFields.TrackingNumber),

                new GridColumnDefinition("{E34B855B-C5DB-4B22-96F9-25B71DB63CDB}",
                    new GridMoneyDisplayType(), "Cost", 4.18m,
                    ShipmentFields.ShipmentCost),

                new GridColumnDefinition("{100CFCA1-DB69-4D63-9B6C-FEDFB9A9F14B}", false,
                    new GridEnumDisplayType<ShipSenseStatus>(EnumSortMethod.Value), "ShipSense", ShipSenseStatus.Applied,
                    ShipmentFields.ShipSenseStatus)  { DefaultWidth = 120 },

                new GridColumnDefinition("{B7EFA46F-045D-446C-B537-2901822351E1}",
                    new GridEnumDisplayType<AddressValidationStatusType>(EnumSortMethod.Description),
                    "S: Validation Status", AddressValidationStatusType.Valid,
                    ShipmentFields.ShipAddressValidationStatus)
                    { DefaultWidth = 100 },

                new GridColumnDefinition("{2CFB33E5-BD62-4F99-98C9-447A5B0FE4FB}",
                    new GridActionDisplayType(addressSelector.DisplayValidationSuggestionLabel,
                        addressSelector.ShowAddressOptionMenu, addressSelector.IsValidationSuggestionLinkEnabled),
                    "S: Validation Suggestions", "2 Suggestions",
                    new GridColumnFunctionValueProvider(x => x),
                    new GridColumnSortProvider(ShipmentFields.ShipAddressValidationSuggestionCount, ShipmentFields.ShipAddressValidationStatus))
                    { DefaultWidth = 120 },

                new GridColumnDefinition("{B42A0B4A-4947-45B1-9CBC-DBC7D1974662}",
                    new GridEnumDisplayType<ValidationDetailStatusType>(EnumSortMethod.Description),
                    "S: Residential status",  ValidationDetailStatusType.Yes,
                    ShipmentFields.ShipResidentialStatus)
                    { DefaultWidth = 100 },

                new GridColumnDefinition("{6DBF7E86-C2E6-497C-ABED-A1376E303DEA}",
                    new GridEnumDisplayType<ValidationDetailStatusType>(EnumSortMethod.Description),
                    "S: PO Box", ValidationDetailStatusType.Yes,
                    ShipmentFields.ShipPOBox)
                    { DefaultWidth = 72 },

                new GridColumnDefinition("{1B21559C-78EB-4011-B1E7-06A926D66AAD}",
                    new GridEnumDisplayType<ValidationDetailStatusType>(EnumSortMethod.Description),
                    "S: International Territory",  ValidationDetailStatusType.Yes,
                    ShipmentFields.ShipUSTerritory)
                    { DefaultWidth = 145 },

                new GridColumnDefinition("{49360D66-F61F-4377-92E6-8C71E78ADC86}",
                    new GridEnumDisplayType<ValidationDetailStatusType>(EnumSortMethod.Description),
                    "S: Military Address",  ValidationDetailStatusType.Yes,
                    ShipmentFields.ShipMilitaryAddress)
                    { DefaultWidth = 115 },

                new GridColumnDefinition("{232D707A-E652-47F9-ABA6-D96CE9165197}", false,
                    new GridEnumDisplayType<ThermalLanguage>(EnumSortMethod.Description), "Requested Label Format", ThermalLanguage.None,
                    ShipmentFields.RequestedLabelFormat)  { DefaultWidth = 60 },

                new GridColumnDefinition("{628D74DD-B10A-4E04-A929-F524E0CC3096}", false,
                    new GridActualLabelFormatDisplayType(), "Actual Label Format", ThermalLanguage.None,
                    ShipmentFields.ActualLabelFormat)  { DefaultWidth = 60 },
            };
        }
    }
}
