///////////////////////////////////////////////////////////////
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
    /// Entity interface which represents the entity 'FedExPackage'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IFedExPackageEntity
    {
        
        /// <summary> The FedExPackageID property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."FedExPackageID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 FedExPackageID { get; }
        /// <summary> The ShipmentID property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ShipmentID { get; }
        /// <summary> The Weight property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double Weight { get; }
        /// <summary> The DimsProfileID property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsHeight { get; }
        /// <summary> The DimsWeight property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsWeight { get; }
        /// <summary> The DimsAddWeight property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean DimsAddWeight { get; }
        /// <summary> The SkidPieces property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."SkidPieces"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 SkidPieces { get; }
        /// <summary> The Insurance property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."Insurance"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean Insurance { get; }
        /// <summary> The InsuranceValue property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."InsuranceValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal InsuranceValue { get; }
        /// <summary> The InsurancePennyOne property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."InsurancePennyOne"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean InsurancePennyOne { get; }
        /// <summary> The DeclaredValue property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DeclaredValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal DeclaredValue { get; }
        /// <summary> The TrackingNumber property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."TrackingNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String TrackingNumber { get; }
        /// <summary> The PriorityAlert property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."PriorityAlert"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean PriorityAlert { get; }
        /// <summary> The PriorityAlertEnhancementType property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."PriorityAlertEnhancementType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 PriorityAlertEnhancementType { get; }
        /// <summary> The PriorityAlertDetailContent property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."PriorityAlertDetailContent"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 1024<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PriorityAlertDetailContent { get; }
        /// <summary> The DryIceWeight property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DryIceWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DryIceWeight { get; }
        /// <summary> The ContainsAlcohol property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."ContainsAlcohol"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean ContainsAlcohol { get; }
        /// <summary> The DangerousGoodsEnabled property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DangerousGoodsEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean DangerousGoodsEnabled { get; }
        /// <summary> The DangerousGoodsType property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DangerousGoodsType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 DangerousGoodsType { get; }
        /// <summary> The DangerousGoodsAccessibilityType property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DangerousGoodsAccessibilityType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 DangerousGoodsAccessibilityType { get; }
        /// <summary> The DangerousGoodsCargoAircraftOnly property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DangerousGoodsCargoAircraftOnly"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean DangerousGoodsCargoAircraftOnly { get; }
        /// <summary> The DangerousGoodsEmergencyContactPhone property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DangerousGoodsEmergencyContactPhone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 16<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String DangerousGoodsEmergencyContactPhone { get; }
        /// <summary> The DangerousGoodsOfferor property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DangerousGoodsOfferor"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 128<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String DangerousGoodsOfferor { get; }
        /// <summary> The DangerousGoodsPackagingCount property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DangerousGoodsPackagingCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 DangerousGoodsPackagingCount { get; }
        /// <summary> The HazardousMaterialNumber property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."HazardousMaterialNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 16<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String HazardousMaterialNumber { get; }
        /// <summary> The HazardousMaterialClass property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."HazardousMaterialClass"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 8<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String HazardousMaterialClass { get; }
        /// <summary> The HazardousMaterialProperName property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."HazardousMaterialProperName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 64<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String HazardousMaterialProperName { get; }
        /// <summary> The HazardousMaterialPackingGroup property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."HazardousMaterialPackingGroup"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 HazardousMaterialPackingGroup { get; }
        /// <summary> The HazardousMaterialQuantityValue property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."HazardousMaterialQuantityValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double HazardousMaterialQuantityValue { get; }
        /// <summary> The HazardousMaterialQuanityUnits property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."HazardousMaterialQuanityUnits"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 HazardousMaterialQuanityUnits { get; }
        /// <summary> The HazardousMaterialTechnicalName property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."HazardousMaterialTechnicalName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 64<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String HazardousMaterialTechnicalName { get; }
        
        
        IFedExShipmentEntity FedExShipment { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IFedExPackageEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IFedExPackageEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'FedExPackage'. <br/><br/>
    /// 
    /// </summary>
    public partial class FedExPackageEntity : IFedExPackageEntity
    {
        
        IFedExShipmentEntity IFedExPackageEntity.FedExShipment => FedExShipment;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFedExPackageEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IFedExPackageEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IFedExPackageEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyFedExPackageEntity(this, objectMap);
        }
    }
}
