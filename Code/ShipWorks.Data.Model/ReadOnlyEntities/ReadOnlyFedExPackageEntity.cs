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
    /// Read-only representation of the entity 'FedExPackage'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyFedExPackageEntity : IFedExPackageEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyFedExPackageEntity(IFedExPackageEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            FedExPackageID = source.FedExPackageID;
            ShipmentID = source.ShipmentID;
            Weight = source.Weight;
            DimsProfileID = source.DimsProfileID;
            DimsLength = source.DimsLength;
            DimsWidth = source.DimsWidth;
            DimsHeight = source.DimsHeight;
            DimsWeight = source.DimsWeight;
            DimsAddWeight = source.DimsAddWeight;
            SkidPieces = source.SkidPieces;
            Insurance = source.Insurance;
            InsuranceValue = source.InsuranceValue;
            InsurancePennyOne = source.InsurancePennyOne;
            DeclaredValue = source.DeclaredValue;
            TrackingNumber = source.TrackingNumber;
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
            HazardousMaterialTechnicalName = source.HazardousMaterialTechnicalName;
            SignatoryContactName = source.SignatoryContactName;
            SignatoryTitle = source.SignatoryTitle;
            SignatoryPlace = source.SignatoryPlace;
            AlcoholRecipientType = source.AlcoholRecipientType;
            ContainerType = source.ContainerType;
            NumberOfContainers = source.NumberOfContainers;
            PackingDetailsCargoAircraftOnly = source.PackingDetailsCargoAircraftOnly;
            PackingDetailsPackingInstructions = source.PackingDetailsPackingInstructions;
            
            
            FedExShipment = source.FedExShipment?.AsReadOnly(objectMap);
            

            CopyCustomFedExPackageData(source);
        }

        
        /// <summary> The FedExPackageID property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."FedExPackageID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 FedExPackageID { get; }
        /// <summary> The ShipmentID property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ShipmentID { get; }
        /// <summary> The Weight property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double Weight { get; }
        /// <summary> The DimsProfileID property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsHeight { get; }
        /// <summary> The DimsWeight property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsWeight { get; }
        /// <summary> The DimsAddWeight property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean DimsAddWeight { get; }
        /// <summary> The SkidPieces property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."SkidPieces"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 SkidPieces { get; }
        /// <summary> The Insurance property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."Insurance"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean Insurance { get; }
        /// <summary> The InsuranceValue property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."InsuranceValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal InsuranceValue { get; }
        /// <summary> The InsurancePennyOne property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."InsurancePennyOne"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean InsurancePennyOne { get; }
        /// <summary> The DeclaredValue property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DeclaredValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal DeclaredValue { get; }
        /// <summary> The TrackingNumber property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."TrackingNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String TrackingNumber { get; }
        /// <summary> The PriorityAlert property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."PriorityAlert"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean PriorityAlert { get; }
        /// <summary> The PriorityAlertEnhancementType property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."PriorityAlertEnhancementType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 PriorityAlertEnhancementType { get; }
        /// <summary> The PriorityAlertDetailContent property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."PriorityAlertDetailContent"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 1024<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PriorityAlertDetailContent { get; }
        /// <summary> The DryIceWeight property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DryIceWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DryIceWeight { get; }
        /// <summary> The ContainsAlcohol property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."ContainsAlcohol"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean ContainsAlcohol { get; }
        /// <summary> The DangerousGoodsEnabled property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DangerousGoodsEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean DangerousGoodsEnabled { get; }
        /// <summary> The DangerousGoodsType property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DangerousGoodsType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 DangerousGoodsType { get; }
        /// <summary> The DangerousGoodsAccessibilityType property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DangerousGoodsAccessibilityType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 DangerousGoodsAccessibilityType { get; }
        /// <summary> The DangerousGoodsCargoAircraftOnly property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DangerousGoodsCargoAircraftOnly"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean DangerousGoodsCargoAircraftOnly { get; }
        /// <summary> The DangerousGoodsEmergencyContactPhone property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DangerousGoodsEmergencyContactPhone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 16<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String DangerousGoodsEmergencyContactPhone { get; }
        /// <summary> The DangerousGoodsOfferor property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DangerousGoodsOfferor"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 128<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String DangerousGoodsOfferor { get; }
        /// <summary> The DangerousGoodsPackagingCount property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."DangerousGoodsPackagingCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 DangerousGoodsPackagingCount { get; }
        /// <summary> The HazardousMaterialNumber property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."HazardousMaterialNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 16<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String HazardousMaterialNumber { get; }
        /// <summary> The HazardousMaterialClass property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."HazardousMaterialClass"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 8<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String HazardousMaterialClass { get; }
        /// <summary> The HazardousMaterialProperName property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."HazardousMaterialProperName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 64<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String HazardousMaterialProperName { get; }
        /// <summary> The HazardousMaterialPackingGroup property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."HazardousMaterialPackingGroup"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 HazardousMaterialPackingGroup { get; }
        /// <summary> The HazardousMaterialQuantityValue property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."HazardousMaterialQuantityValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double HazardousMaterialQuantityValue { get; }
        /// <summary> The HazardousMaterialQuanityUnits property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."HazardousMaterialQuanityUnits"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 HazardousMaterialQuanityUnits { get; }
        /// <summary> The HazardousMaterialTechnicalName property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."HazardousMaterialTechnicalName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 64<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String HazardousMaterialTechnicalName { get; }
        /// <summary> The SignatoryContactName property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."SignatoryContactName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SignatoryContactName { get; }
        /// <summary> The SignatoryTitle property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."SignatoryTitle"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SignatoryTitle { get; }
        /// <summary> The SignatoryPlace property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."SignatoryPlace"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SignatoryPlace { get; }
        /// <summary> The AlcoholRecipientType property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."AlcoholRecipientType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 AlcoholRecipientType { get; }
        /// <summary> The ContainerType property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."ContainerType"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ContainerType { get; }
        /// <summary> The NumberOfContainers property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."NumberOfContainers"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 NumberOfContainers { get; }
        /// <summary> The PackingDetailsCargoAircraftOnly property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."PackingDetailsCargoAircraftOnly"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean PackingDetailsCargoAircraftOnly { get; }
        /// <summary> The PackingDetailsPackingInstructions property of the Entity FedExPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExPackage"."PackingDetailsPackingInstructions"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PackingDetailsPackingInstructions { get; }
        
        
        public IFedExShipmentEntity FedExShipment { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFedExPackageEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFedExPackageEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomFedExPackageData(IFedExPackageEntity source);
    }
}
