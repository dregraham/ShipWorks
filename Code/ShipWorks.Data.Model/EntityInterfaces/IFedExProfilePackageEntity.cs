﻿///////////////////////////////////////////////////////////////
// This is generated code.
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 5.0
// Code is generated on: 
// Code is generated using templates: ShipWorks
// Templates vendor: Solutions Design.
// Templates version: 
//////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'FedExProfilePackage'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IFedExProfilePackageEntity
    {
        
        /// <summary> The FedExProfilePackageID property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."FedExProfilePackageID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 FedExProfilePackageID { get; }
        /// <summary> The ShippingProfileID property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ShippingProfileID { get; }
        /// <summary> The PriorityAlert property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."PriorityAlert"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> PriorityAlert { get; }
        /// <summary> The PriorityAlertEnhancementType property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."PriorityAlertEnhancementType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> PriorityAlertEnhancementType { get; }
        /// <summary> The PriorityAlertDetailContent property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."PriorityAlertDetailContent"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 1024<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String PriorityAlertDetailContent { get; }
        /// <summary> The DryIceWeight property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."DryIceWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DryIceWeight { get; }
        /// <summary> The ContainsAlcohol property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."ContainsAlcohol"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> ContainsAlcohol { get; }
        /// <summary> The DangerousGoodsEnabled property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."DangerousGoodsEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> DangerousGoodsEnabled { get; }
        /// <summary> The DangerousGoodsType property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."DangerousGoodsType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> DangerousGoodsType { get; }
        /// <summary> The DangerousGoodsAccessibilityType property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."DangerousGoodsAccessibilityType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> DangerousGoodsAccessibilityType { get; }
        /// <summary> The DangerousGoodsCargoAircraftOnly property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."DangerousGoodsCargoAircraftOnly"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> DangerousGoodsCargoAircraftOnly { get; }
        /// <summary> The DangerousGoodsEmergencyContactPhone property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."DangerousGoodsEmergencyContactPhone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 16<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String DangerousGoodsEmergencyContactPhone { get; }
        /// <summary> The DangerousGoodsOfferor property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."DangerousGoodsOfferor"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 128<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String DangerousGoodsOfferor { get; }
        /// <summary> The DangerousGoodsPackagingCount property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."DangerousGoodsPackagingCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> DangerousGoodsPackagingCount { get; }
        /// <summary> The HazardousMaterialNumber property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."HazardousMaterialNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 16<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String HazardousMaterialNumber { get; }
        /// <summary> The HazardousMaterialClass property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."HazardousMaterialClass"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 8<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String HazardousMaterialClass { get; }
        /// <summary> The HazardousMaterialProperName property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."HazardousMaterialProperName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 64<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String HazardousMaterialProperName { get; }
        /// <summary> The HazardousMaterialPackingGroup property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."HazardousMaterialPackingGroup"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> HazardousMaterialPackingGroup { get; }
        /// <summary> The HazardousMaterialQuantityValue property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."HazardousMaterialQuantityValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> HazardousMaterialQuantityValue { get; }
        /// <summary> The HazardousMaterialQuanityUnits property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."HazardousMaterialQuanityUnits"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> HazardousMaterialQuanityUnits { get; }
        /// <summary> The SignatoryContactName property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."SignatoryContactName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String SignatoryContactName { get; }
        /// <summary> The SignatoryTitle property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."SignatoryTitle"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String SignatoryTitle { get; }
        /// <summary> The SignatoryPlace property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."SignatoryPlace"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String SignatoryPlace { get; }
        /// <summary> The ContainerType property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."ContainerType"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ContainerType { get; }
        /// <summary> The NumberOfContainers property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."NumberOfContainers"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> NumberOfContainers { get; }
        /// <summary> The PackingDetailsCargoAircraftOnly property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."PackingDetailsCargoAircraftOnly"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> PackingDetailsCargoAircraftOnly { get; }
        /// <summary> The PackingDetailsPackingInstructions property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."PackingDetailsPackingInstructions"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String PackingDetailsPackingInstructions { get; }
        /// <summary> The BatteryMaterial property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."BatteryMaterial"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<ShipWorks.Shipping.FedEx.FedExBatteryMaterialType> BatteryMaterial { get; }
        /// <summary> The BatteryPacking property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."BatteryPacking"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<ShipWorks.Shipping.FedEx.FedExBatteryPackingType> BatteryPacking { get; }
        /// <summary> The BatteryRegulatorySubtype property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."BatteryRegulatorySubtype"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<ShipWorks.Shipping.FedEx.FedExBatteryRegulatorySubType> BatteryRegulatorySubtype { get; }
        /// <summary> The PackageProfileID property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."PackageProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 PackageProfileID { get; }
        
        
        IFedExProfileEntity FedExProfile { get; }
        IPackageProfileEntity PackageProfile { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IFedExProfilePackageEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IFedExProfilePackageEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'FedExProfilePackage'. <br/><br/>
    /// 
    /// </summary>
    public partial class FedExProfilePackageEntity : IFedExProfilePackageEntity
    {
        
        IFedExProfileEntity IFedExProfilePackageEntity.FedExProfile => FedExProfile;
        IPackageProfileEntity IFedExProfilePackageEntity.PackageProfile => PackageProfile;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFedExProfilePackageEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IFedExProfilePackageEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IFedExProfilePackageEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyFedExProfilePackageEntity(this, objectMap);
        }

        
    }
}
