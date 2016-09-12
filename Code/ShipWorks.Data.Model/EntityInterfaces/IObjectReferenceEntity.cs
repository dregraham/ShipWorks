﻿///////////////////////////////////////////////////////////////
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
    /// Entity interface which represents the entity 'ObjectReference'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IObjectReferenceEntity
    {
        
        /// <summary> The ObjectReferenceID property of the Entity ObjectReference<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ObjectReference"."ObjectReferenceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ObjectReferenceID { get; }
        /// <summary> The ConsumerID property of the Entity ObjectReference<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ObjectReference"."ConsumerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ConsumerID { get; }
        /// <summary> The ReferenceKey property of the Entity ObjectReference<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ObjectReference"."ReferenceKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 250<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ReferenceKey { get; }
        /// <summary> The ObjectID property of the Entity ObjectReference<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ObjectReference"."ObjectID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ObjectID { get; }
        /// <summary> The Reason property of the Entity ObjectReference<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ObjectReference"."Reason"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 250<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String Reason { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IObjectReferenceEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IObjectReferenceEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ObjectReference'. <br/><br/>
    /// 
    /// </summary>
    public partial class ObjectReferenceEntity : IObjectReferenceEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IObjectReferenceEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IObjectReferenceEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IObjectReferenceEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyObjectReferenceEntity(this, objectMap);
        }
    }
}