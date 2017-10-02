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
    /// Entity interface which represents the entity 'AmazonProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IAmazonProfileEntity
    {
        
        /// <summary> The ShippingProfileID property of the Entity AmazonProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShippingProfileID { get; }
        /// <summary> The DimsProfileID property of the Entity AmazonProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonProfile"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity AmazonProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonProfile"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity AmazonProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonProfile"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity AmazonProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonProfile"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DimsHeight { get; }
        /// <summary> The DimsWeight property of the Entity AmazonProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonProfile"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DimsWeight { get; }
        /// <summary> The DimsAddWeight property of the Entity AmazonProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonProfile"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> DimsAddWeight { get; }
        /// <summary> The DeliveryExperience property of the Entity AmazonProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonProfile"."DeliveryExperience"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> DeliveryExperience { get; }
        /// <summary> The Weight property of the Entity AmazonProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonProfile"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> Weight { get; }
        /// <summary> The ShippingServiceID property of the Entity AmazonProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonProfile"."ShippingServiceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ShippingServiceID { get; }
        
        IShippingProfileEntity ShippingProfile { get; }
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IAmazonProfileEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IAmazonProfileEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'AmazonProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial class AmazonProfileEntity : IAmazonProfileEntity
    {
        IShippingProfileEntity IAmazonProfileEntity.ShippingProfile => ShippingProfile;
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAmazonProfileEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IAmazonProfileEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IAmazonProfileEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyAmazonProfileEntity(this, objectMap);
        }

        
    }
}
