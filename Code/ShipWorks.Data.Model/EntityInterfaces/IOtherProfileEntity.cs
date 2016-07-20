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
    /// Entity interface which represents the entity 'OtherProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IOtherProfileEntity
    {
        
        /// <summary> The ShippingProfileID property of the Entity OtherProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OtherProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShippingProfileID { get; }
        /// <summary> The Carrier property of the Entity OtherProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OtherProfile"."Carrier"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String Carrier { get; }
        /// <summary> The Service property of the Entity OtherProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OtherProfile"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String Service { get; }
        
        IShippingProfileEntity ShippingProfile { get; }
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOtherProfileEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOtherProfileEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'OtherProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial class OtherProfileEntity : IOtherProfileEntity
    {
        IShippingProfileEntity IOtherProfileEntity.ShippingProfile => ShippingProfile;
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOtherProfileEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IOtherProfileEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IOtherProfileEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyOtherProfileEntity(this, objectMap);
        }
    }
}
