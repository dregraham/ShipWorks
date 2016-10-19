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
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Read-only representation of the entity 'FedExProfilePackage'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyFedExProfilePackageEntity : IFedExProfilePackageEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyFedExProfilePackageEntity(IFedExProfilePackageEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            FedExProfilePackageID = source.FedExProfilePackageID;
            ShippingProfileID = source.ShippingProfileID;
            Weight = source.Weight;
            DimsProfileID = source.DimsProfileID;
            DimsLength = source.DimsLength;
            DimsWidth = source.DimsWidth;
            DimsHeight = source.DimsHeight;
            DimsWeight = source.DimsWeight;
            DimsAddWeight = source.DimsAddWeight;
            PriorityAlert = source.PriorityAlert;
            PriorityAlertEnhancementType = source.PriorityAlertEnhancementType;
            PriorityAlertDetailContent = source.PriorityAlertDetailContent;
            DryIceWeight = source.DryIceWeight;
            ContainsAlcohol = source.ContainsAlcohol;
            DangerousGoodsEnabled = source.DangerousGoodsEnabled;
            DangerousGoodsType = source.DangerousGoodsType;
            DangerousGoodsAccessibilityType = source.DangerousGoodsAccessibilityType;
            DangerousGoodsCargoAircraftOnly = source.DangerousGoodsCargoAircraftOnly;
            DangerousGoodsEmergencyContactPhone = source.DangerousGoodsEmergencyContactPhone;
            DangerousGoodsOfferor = source.DangerousGoodsOfferor;
            DangerousGoodsPackagingCount = source.DangerousGoodsPackagingCount;
            HazardousMaterialNumber = source.HazardousMaterialNumber;
            HazardousMaterialClass = source.HazardousMaterialClass;
            HazardousMaterialProperName = source.HazardousMaterialProperName;
            HazardousMaterialPackingGroup = source.HazardousMaterialPackingGroup;
            HazardousMaterialQuantityValue = source.HazardousMaterialQuantityValue;
            HazardousMaterialQuanityUnits = source.HazardousMaterialQuanityUnits;
            SignatoryContactName = source.SignatoryContactName;
            SignatoryTitle = source.SignatoryTitle;
            SignatoryPlace = source.SignatoryPlace;
            
            
            FedExProfile = source.FedExProfile?.AsReadOnly(objectMap);
            

            CopyCustomFedExProfilePackageData(source);
        }

        
        /// <summary> The FedExProfilePackageID property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."FedExProfilePackageID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 FedExProfilePackageID { get; }
        /// <summary> The ShippingProfileID property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ShippingProfileID { get; }
        /// <summary> The Weight property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Double> Weight { get; }
        /// <summary> The DimsProfileID property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Double> DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Double> DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Double> DimsHeight { get; }
        /// <summary> The DimsWeight property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Double> DimsWeight { get; }
        /// <summary> The DimsAddWeight property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> DimsAddWeight { get; }
        /// <summary> The PriorityAlert property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."PriorityAlert"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> PriorityAlert { get; }
        /// <summary> The PriorityAlertEnhancementType property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."PriorityAlertEnhancementType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> PriorityAlertEnhancementType { get; }
        /// <summary> The PriorityAlertDetailContent property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."PriorityAlertDetailContent"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 1024<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String PriorityAlertDetailContent { get; }
        /// <summary> The DryIceWeight property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."DryIceWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Double> DryIceWeight { get; }
        /// <summary> The ContainsAlcohol property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."ContainsAlcohol"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> ContainsAlcohol { get; }
        /// <summary> The DangerousGoodsEnabled property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."DangerousGoodsEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> DangerousGoodsEnabled { get; }
        /// <summary> The DangerousGoodsType property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."DangerousGoodsType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> DangerousGoodsType { get; }
        /// <summary> The DangerousGoodsAccessibilityType property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."DangerousGoodsAccessibilityType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> DangerousGoodsAccessibilityType { get; }
        /// <summary> The DangerousGoodsCargoAircraftOnly property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."DangerousGoodsCargoAircraftOnly"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> DangerousGoodsCargoAircraftOnly { get; }
        /// <summary> The DangerousGoodsEmergencyContactPhone property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."DangerousGoodsEmergencyContactPhone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 16<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String DangerousGoodsEmergencyContactPhone { get; }
        /// <summary> The DangerousGoodsOfferor property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."DangerousGoodsOfferor"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 128<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String DangerousGoodsOfferor { get; }
        /// <summary> The DangerousGoodsPackagingCount property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."DangerousGoodsPackagingCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> DangerousGoodsPackagingCount { get; }
        /// <summary> The HazardousMaterialNumber property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."HazardousMaterialNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 16<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String HazardousMaterialNumber { get; }
        /// <summary> The HazardousMaterialClass property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."HazardousMaterialClass"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 8<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String HazardousMaterialClass { get; }
        /// <summary> The HazardousMaterialProperName property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."HazardousMaterialProperName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 64<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String HazardousMaterialProperName { get; }
        /// <summary> The HazardousMaterialPackingGroup property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."HazardousMaterialPackingGroup"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> HazardousMaterialPackingGroup { get; }
        /// <summary> The HazardousMaterialQuantityValue property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."HazardousMaterialQuantityValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Double> HazardousMaterialQuantityValue { get; }
        /// <summary> The HazardousMaterialQuanityUnits property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."HazardousMaterialQuanityUnits"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> HazardousMaterialQuanityUnits { get; }
        /// <summary> The SignatoryContactName property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."SignatoryContactName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String SignatoryContactName { get; }
        /// <summary> The SignatoryTitle property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."SignatoryTitle"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String SignatoryTitle { get; }
        /// <summary> The SignatoryPlace property of the Entity FedExProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfilePackage"."SignatoryPlace"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String SignatoryPlace { get; }
        
        
        public IFedExProfileEntity FedExProfile { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFedExProfilePackageEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFedExProfilePackageEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomFedExProfilePackageData(IFedExProfilePackageEntity source);
    }
}
