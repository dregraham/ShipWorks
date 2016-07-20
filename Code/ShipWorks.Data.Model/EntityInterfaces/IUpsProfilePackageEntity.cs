///////////////////////////////////////////////////////////////
// This is generated code.
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
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
    /// Entity interface which represents the entity 'UpsProfilePackage'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IUpsProfilePackageEntity
    {
        
        /// <summary> The UpsProfilePackageID property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."UpsProfilePackageID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 UpsProfilePackageID { get; }
        /// <summary> The ShippingProfileID property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ShippingProfileID { get; }
        /// <summary> The PackagingType property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."PackagingType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> PackagingType { get; }
        /// <summary> The Weight property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> Weight { get; }
        /// <summary> The DimsProfileID property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DimsHeight { get; }
        /// <summary> The DimsWeight property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DimsWeight { get; }
        /// <summary> The DimsAddWeight property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> DimsAddWeight { get; }
        /// <summary> The AdditionalHandlingEnabled property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."AdditionalHandlingEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> AdditionalHandlingEnabled { get; }
        /// <summary> The VerbalConfirmationEnabled property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."VerbalConfirmationEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> VerbalConfirmationEnabled { get; }
        /// <summary> The VerbalConfirmationName property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."VerbalConfirmationName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String VerbalConfirmationName { get; }
        /// <summary> The VerbalConfirmationPhone property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."VerbalConfirmationPhone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 15<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String VerbalConfirmationPhone { get; }
        /// <summary> The VerbalConfirmationPhoneExtension property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."VerbalConfirmationPhoneExtension"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 4<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String VerbalConfirmationPhoneExtension { get; }
        /// <summary> The DryIceEnabled property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."DryIceEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> DryIceEnabled { get; }
        /// <summary> The DryIceRegulationSet property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."DryIceRegulationSet"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> DryIceRegulationSet { get; }
        /// <summary> The DryIceWeight property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."DryIceWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DryIceWeight { get; }
        /// <summary> The DryIceIsForMedicalUse property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."DryIceIsForMedicalUse"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> DryIceIsForMedicalUse { get; }
        
        
        IUpsProfileEntity UpsProfile { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUpsProfilePackageEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUpsProfilePackageEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'UpsProfilePackage'. <br/><br/>
    /// 
    /// </summary>
    public partial class UpsProfilePackageEntity : IUpsProfilePackageEntity
    {
        
        IUpsProfileEntity IUpsProfilePackageEntity.UpsProfile => UpsProfile;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsProfilePackageEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IUpsProfilePackageEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IUpsProfilePackageEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyUpsProfilePackageEntity(this, objectMap);
        }
    }
}
