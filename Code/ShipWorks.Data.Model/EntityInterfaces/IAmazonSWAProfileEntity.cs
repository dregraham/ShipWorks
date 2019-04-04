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
    /// Entity interface which represents the entity 'AmazonSWAProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IAmazonSWAProfileEntity
    {
        
        /// <summary> The ShippingProfileID property of the Entity AmazonSWAProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShippingProfileID { get; }
        /// <summary> The AmazonSWAAccountID property of the Entity AmazonSWAProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAProfile"."AmazonSWAAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> AmazonSWAAccountID { get; }
        /// <summary> The Service property of the Entity AmazonSWAProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAProfile"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> Service { get; }
        
        IShippingProfileEntity ShippingProfile { get; }
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IAmazonSWAProfileEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IAmazonSWAProfileEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'AmazonSWAProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial class AmazonSWAProfileEntity : IAmazonSWAProfileEntity
    {
        IShippingProfileEntity IAmazonSWAProfileEntity.ShippingProfile => ShippingProfile;
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAmazonSWAProfileEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IAmazonSWAProfileEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IAmazonSWAProfileEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyAmazonSWAProfileEntity(this, objectMap);
        }

        
    }
}
