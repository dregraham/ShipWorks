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
    /// Entity interface which represents the entity 'UpsPackage'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IUpsPackageEntity
    {
        
        /// <summary> The UpsPackageID property of the Entity UpsPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackage"."UpsPackageID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 UpsPackageID { get; }
        /// <summary> The ShipmentID property of the Entity UpsPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackage"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ShipmentID { get; }
        /// <summary> The PackagingType property of the Entity UpsPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackage"."PackagingType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 PackagingType { get; }
        /// <summary> The Weight property of the Entity UpsPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackage"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double Weight { get; }
        /// <summary> The DimsProfileID property of the Entity UpsPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackage"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity UpsPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackage"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity UpsPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackage"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity UpsPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackage"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsHeight { get; }
        /// <summary> The DimsWeight property of the Entity UpsPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackage"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsWeight { get; }
        /// <summary> The DimsAddWeight property of the Entity UpsPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackage"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean DimsAddWeight { get; }
        /// <summary> The Insurance property of the Entity UpsPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackage"."Insurance"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean Insurance { get; }
        /// <summary> The InsuranceValue property of the Entity UpsPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackage"."InsuranceValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal InsuranceValue { get; }
        /// <summary> The InsurancePennyOne property of the Entity UpsPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackage"."InsurancePennyOne"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean InsurancePennyOne { get; }
        /// <summary> The DeclaredValue property of the Entity UpsPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackage"."DeclaredValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal DeclaredValue { get; }
        /// <summary> The TrackingNumber property of the Entity UpsPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackage"."TrackingNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String TrackingNumber { get; }
        /// <summary> The UspsTrackingNumber property of the Entity UpsPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackage"."UspsTrackingNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String UspsTrackingNumber { get; }
        /// <summary> The AdditionalHandlingEnabled property of the Entity UpsPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackage"."AdditionalHandlingEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean AdditionalHandlingEnabled { get; }
        /// <summary> The VerbalConfirmationEnabled property of the Entity UpsPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackage"."VerbalConfirmationEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean VerbalConfirmationEnabled { get; }
        /// <summary> The VerbalConfirmationName property of the Entity UpsPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackage"."VerbalConfirmationName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String VerbalConfirmationName { get; }
        /// <summary> The VerbalConfirmationPhone property of the Entity UpsPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackage"."VerbalConfirmationPhone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 15<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String VerbalConfirmationPhone { get; }
        /// <summary> The VerbalConfirmationPhoneExtension property of the Entity UpsPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackage"."VerbalConfirmationPhoneExtension"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 4<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String VerbalConfirmationPhoneExtension { get; }
        /// <summary> The DryIceEnabled property of the Entity UpsPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackage"."DryIceEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean DryIceEnabled { get; }
        /// <summary> The DryIceRegulationSet property of the Entity UpsPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackage"."DryIceRegulationSet"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 DryIceRegulationSet { get; }
        /// <summary> The DryIceWeight property of the Entity UpsPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackage"."DryIceWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DryIceWeight { get; }
        /// <summary> The DryIceIsForMedicalUse property of the Entity UpsPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackage"."DryIceIsForMedicalUse"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean DryIceIsForMedicalUse { get; }
        
        
        IUpsShipmentEntity UpsShipment { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUpsPackageEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUpsPackageEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'UpsPackage'. <br/><br/>
    /// 
    /// </summary>
    public partial class UpsPackageEntity : IUpsPackageEntity
    {
        
        IUpsShipmentEntity IUpsPackageEntity.UpsShipment => UpsShipment;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsPackageEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IUpsPackageEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IUpsPackageEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyUpsPackageEntity(this, objectMap);
        }

        
    }
}
