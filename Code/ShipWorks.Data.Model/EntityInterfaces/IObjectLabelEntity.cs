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
    /// Entity interface which represents the entity 'ObjectLabel'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IObjectLabelEntity
    {
        
        /// <summary> The ObjectID property of the Entity ObjectLabel<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ObjectLabel"."ObjectID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ObjectID { get; }
        /// <summary> The RowVersion property of the Entity ObjectLabel<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ObjectLabel"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The ObjectType property of the Entity ObjectLabel<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ObjectLabel"."ObjectType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ObjectType { get; }
        /// <summary> The ParentID property of the Entity ObjectLabel<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ObjectLabel"."ParentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> ParentID { get; }
        /// <summary> The Label property of the Entity ObjectLabel<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ObjectLabel"."Label"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Label { get; }
        /// <summary> The IsDeleted property of the Entity ObjectLabel<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ObjectLabel"."IsDeleted"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean IsDeleted { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IObjectLabelEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IObjectLabelEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ObjectLabel'. <br/><br/>
    /// 
    /// </summary>
    public partial class ObjectLabelEntity : IObjectLabelEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IObjectLabelEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IObjectLabelEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IObjectLabelEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyObjectLabelEntity(this, objectMap);
        }
    }
}