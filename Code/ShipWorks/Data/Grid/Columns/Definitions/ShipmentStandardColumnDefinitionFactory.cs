using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.AddressValidation;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Shipping.Editing;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Grid.Columns.ValueProviders;
using ShipWorks.Shipping;
using ShipWorks.Shipping.CoreExtensions.Grid;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators;
using ShipWorks.Shipping.ShipSense;

namespace ShipWorks.Data.Grid.Columns.Definitions
{
    /// <summary>
    /// Factory class for creating the builtin column definitions
    /// </summary>
    public static class ShipmentStandardColumnDefinitionFactory
    {
        /// <summary>
        /// Create the default grid column definitions for all possible shipment columns.
        /// </summary>
        public static GridColumnDefinitionCollection CreateDefinitions()
        {
            return new GridColumnDefinitionCollection
            {
                new GridColumnDefinition("{61F0BCFE-CBB1-4df8-A8AA-EEBED9BF3D84}", true,
                    new ShipmentStatusDisplayType(), "Status", new ShipmentEntity { Processed = true },
                    new GridColumnFieldValueProvider(ShipmentFields.ShipmentID),
                    new GridColumnSortProvider(e => 
                        {
                            ShipmentEntity shipment = (ShipmentEntity) e;

                            if (ShippingDlg.ErrorManager?.ShipmentHasError(shipment.ShipmentID) ?? false)
                            {
                                return 3;
                            }

                            if (!shipment.Processed)
                            {
                                return 2;
                            }

                            if (shipment.Processed && !shipment.Voided)
                            {
                                return 1;
                            }

                            return 0;
                        }))  { DefaultWidth = 24 },

                new GridColumnDefinition("{EC204711-129C-4d4b-B476-168EDB171C0B}", true,
                    new GridOrderNumberDisplayType(), "Order #", GridOrderNumberDisplayType.SampleData(Stores.StoreTypeCode.GenericModule),
                    OrderFields.OrderNumberComplete, 
                    OrderFields.OrderNumber) { DefaultWidth = 54, EntityTransform = TransformShipmentToOrder  },

                new GridColumnDefinition("{5BB0C21B-DA27-4221-BA78-EDDC0DE47F91}", true,
                    new ShipmentNumberDisplayType(), "Count", "1 of 2",
                    new GridColumnFieldValueProvider(ShipmentFields.ShipmentID),
                    new GridColumnSortProvider(GetTotalSiblingShipments)) { DefaultWidth = 40 },

                new GridColumnDefinition("{BC5992E9-F098-44d7-BCE3-9459922AEF92}", true, 
                    new GridTextDisplayType().Decorate(new GridRollupDecorator(OrderFields.RollupItemCount)) , "Item Name", "Dress Shirt",
                    OrderFields.RollupItemName) { DefaultWidth = 72, EntityTransform = TransformShipmentToOrder },

                new GridColumnDefinition("{F5142246-D62E-4a1d-AFB2-F25A0D53688A}", true,
                    new GridEnumDisplayType<ShipmentTypeCode>(EnumSortMethod.Value), "Provider", ShipmentTypeCode.FedEx,
                    new GridColumnFieldValueProvider(ShipmentFields.ShipmentType),
                    new GridColumnSortProvider(e => ShipmentTypeManager.GetSortValue((ShipmentTypeCode) ((ShipmentEntity) e).ShipmentType))) { DefaultWidth = 46 },

                    new GridColumnDefinition("{4DF5DC1D-AC7E-4AF5-AEA5-17AA7D103037}", false,
                    new GridEnumDisplayType<ShipSenseStatus>(EnumSortMethod.Value), "ShipSense", ShipSenseStatus.Applied,
                    ShipmentFields.ShipSenseStatus),

                    new GridColumnDefinition("{422716BA-0CD2-4c8d-ADB8-C3BC7AB60019}", 
                    new GridTextDisplayType().Decorate(new GridRollupDecorator(OrderFields.RollupItemCount)), "Item Code", "DS-1065",
                    OrderFields.RollupItemCode) { EntityTransform = TransformShipmentToOrder },

                new GridColumnDefinition("{7F25EBBE-04C9-43c2-BCF3-005107C172B5}", 
                    new GridTextDisplayType().Decorate(new GridRollupDecorator(OrderFields.RollupItemCount)), "Item SKU", "1-4656-79798",
                    OrderFields.RollupItemSKU) { EntityTransform = TransformShipmentToOrder },

                new GridColumnDefinition("{2BDED30D-FD63-4640-B499-90C67E1F6AA4}", 
                    new GridTextDisplayType().Decorate(new GridRollupDecorator(OrderFields.RollupItemCount)), "Item Location", "Shelf 2A",
                    OrderFields.RollupItemLocation) { EntityTransform = TransformShipmentToOrder },

                new GridColumnDefinition("{69E0469A-56B8-4dd6-82A2-C0FB39B6BC14}", 
                    new GridTextDisplayType().Decorate(new GridRollupDecorator(OrderFields.RollupItemCount)), "Item Quantity", "3",
                    OrderFields.RollupItemQuantity) { EntityTransform = TransformShipmentToOrder },

                new GridColumnDefinition("{CA78A92C-25F0-4179-9AE2-0FB90C18B561}", false,
                    new GridTextDisplayType(), "Requested Shipping", "UPS Ground",
                    OrderFields.RequestedShipping) { EntityTransform = TransformShipmentToOrder },

                new GridColumnDefinition("{8D3D9D14-69AE-49f1-9DCA-674AB317BD96}",
                    new GridTextDisplayType(), "S: First Name", "John",
                    ShipmentFields.ShipFirstName),

                new GridColumnDefinition("{65F02F2C-C775-4398-B93B-29631E9C5FF1}", 
                    new GridTextDisplayType(), "S: Middle Name", "Edward",
                    ShipmentFields.ShipMiddleName),

                new GridColumnDefinition("{2BE95AE8-F7A5-457b-AD07-0D93FE141FD5}",
                    new GridTextDisplayType(), "S: Last Name", "Smith",
                    ShipmentFields.ShipLastName),

                new GridColumnDefinition("{936C218D-6CA9-4547-8763-EB3B03699775}",
                    new GridTextDisplayType(), "S: Company", "Interapptive, Inc.",
                    ShipmentFields.ShipCompany),

                new GridColumnDefinition("{03D8F2A0-BD13-4a27-951E-637B26D7AFDF}", 
                    new GridTextDisplayType(), "S: Street1", "14 Main St.",
                    ShipmentFields.ShipStreet1),

                new GridColumnDefinition("{822F22C0-EAEE-4765-B235-212ECB0FB589}", 
                    new GridTextDisplayType(), "S: Street2", "Apt. 1203",
                    ShipmentFields.ShipStreet2),

                new GridColumnDefinition("{F42E733B-1F2E-4093-B109-9D4E3DF838B4}", 
                    new GridTextDisplayType(), "S: Street3", "Attn: Jane Smith",
                    ShipmentFields.ShipStreet3),

                new GridColumnDefinition("{A59983D5-3193-4c1f-9A37-62C9D2ACAA58}", 
                    new GridTextDisplayType(), "S: City", "St. Louis",
                    ShipmentFields.ShipCity),

                new GridColumnDefinition("{DE1D6FCD-41F4-4b3d-B00E-9726F7DAFFB9}",
                    new GridStateDisplayType("Ship"), "S: State", "MO",
                    new GridColumnFunctionValueProvider(e => e),
                    new GridColumnSortProvider(ShipmentFields.ShipStateProvCode)),

                new GridColumnDefinition("{F015D712-4BF0-48a4-848C-1234C6171F37}", 
                    new GridTextDisplayType(), "S: Postal Code", "63132",
                    ShipmentFields.ShipPostalCode),

                new GridColumnDefinition("{5D15417E-1A8A-438c-8CAF-4218F7BE58A3}", 
                    new GridCountryDisplayType(), "S: Country", "US",
                    ShipmentFields.ShipCountryCode),

                new GridColumnDefinition("{2C7D93CF-1490-4c1b-9E81-3A8979DC2C82}", 
                    new GridTextDisplayType(), "S: Phone", "1-314-555-0554",
                    ShipmentFields.ShipPhone),

                new GridColumnDefinition("{73C217FA-A2A8-4585-AAA3-8A6E688E07E3}", 
                    new GridTextDisplayType(), "S: Email", "john.smith@interapptive.com",
                    ShipmentFields.ShipEmail), 

                new GridColumnDefinition("{BD8A5B41-F9D8-4C56-B0B0-2F0BBA8CAC4B}",
                    new GridEnumDisplayType<AddressValidationStatusType>(EnumSortMethod.Description),
                    "S: Validation Status", AddressValidationStatusType.Valid,
                    ShipmentFields.ShipAddressValidationStatus) 
                    { DefaultWidth = 100 },

                new GridColumnDefinition("{0791736A-46BC-40D9-A0C2-31432C1D64C4}",
                    new GridEnumDisplayType<ValidationDetailStatusType>(EnumSortMethod.Description),
                    "S: Residential status", ValidationDetailStatusType.Yes,
                    ShipmentFields.ShipResidentialStatus) 
                    { DefaultWidth = 100 }, 

                new GridColumnDefinition("{B857D3D6-32AC-43E2-B133-EA06CBBCFD5B}",
                    new GridEnumDisplayType<ValidationDetailStatusType>(EnumSortMethod.Description),
                    "S: PO Box", ValidationDetailStatusType.Yes,
                    ShipmentFields.ShipPOBox) 
                    { DefaultWidth = 72 }, 

                new GridColumnDefinition("{679D1C53-0826-4827-8CF0-9E153F51398B}",
                    new GridEnumDisplayType<ValidationDetailStatusType>(EnumSortMethod.Description),
                    "S: International Territory", ValidationDetailStatusType.Yes,
                    ShipmentFields.ShipUSTerritory) 
                    { DefaultWidth = 145 }, 

                new GridColumnDefinition("{68565201-49E4-45BB-80C2-E2D8343253F9}",
                    new GridEnumDisplayType<ValidationDetailStatusType>(EnumSortMethod.Description),
                    "S: Military Address", ValidationDetailStatusType.Yes,
                    ShipmentFields.ShipMilitaryAddress) 
                    { DefaultWidth = 115 },

                new GridColumnDefinition("{04A69989-DC89-4B0A-AA72-7D9797A5E6B1}", false,
                    new GridEnumDisplayType<ThermalLanguage>(EnumSortMethod.Description), "Requested Label Format", ThermalLanguage.None,
                    ShipmentFields.RequestedLabelFormat)  { DefaultWidth = 60 },

                new GridColumnDefinition("{B7B9C011-C66B-4B2F-933D-466A0B3AD4AE}", false,
                    new GridActualLabelFormatDisplayType(), "Actual Label Format", ThermalLanguage.None,
                    ShipmentFields.ActualLabelFormat)  { DefaultWidth = 60 },
            };
        }

        /// <summary>
        /// Get the order form the given shipment entity
        /// </summary>
        public static EntityBase2 TransformShipmentToOrder(EntityBase2 entity)
        {
            return ((ShipmentEntity) entity).Order;
        }

        /// <summary>
        /// Get the total number of sibling shipments for the shipment
        /// </summary>
        public static object GetTotalSiblingShipments(EntityBase2 entity)
        {
            return ShippingManager.GetSiblingData((ShipmentEntity) entity).TotalShipments;
        }
    }
}

