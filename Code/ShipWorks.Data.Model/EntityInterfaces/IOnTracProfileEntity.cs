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
    /// Entity interface which represents the entity 'OnTracProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IOnTracProfileEntity
    {
        
        /// <summary> The ShippingProfileID property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShippingProfileID { get; }
        /// <summary> The OnTracAccountID property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."OnTracAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> OnTracAccountID { get; }
        /// <summary> The ResidentialDetermination property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."ResidentialDetermination"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> ResidentialDetermination { get; }
        /// <summary> The Service property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> Service { get; }
        /// <summary> The SaturdayDelivery property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."SaturdayDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> SaturdayDelivery { get; }
        /// <summary> The SignatureRequired property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."SignatureRequired"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> SignatureRequired { get; }
        /// <summary> The PackagingType property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."PackagingType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> PackagingType { get; }
        /// <summary> The Weight property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> Weight { get; }
        /// <summary> The DimsProfileID property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DimsHeight { get; }
        /// <summary> The DimsWeight property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DimsWeight { get; }
        /// <summary> The DimsAddWeight property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> DimsAddWeight { get; }
        /// <summary> The Reference1 property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."Reference1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String Reference1 { get; }
        /// <summary> The Reference2 property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."Reference2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String Reference2 { get; }
        /// <summary> The Instructions property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."Instructions"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String Instructions { get; }
        
        IShippingProfileEntity ShippingProfile { get; }
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOnTracProfileEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOnTracProfileEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'OnTracProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial class OnTracProfileEntity : IOnTracProfileEntity
    {
        IShippingProfileEntity IOnTracProfileEntity.ShippingProfile => ShippingProfile;
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOnTracProfileEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IOnTracProfileEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IOnTracProfileEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyOnTracProfileEntity(this, objectMap);
        }

        
    }
}
