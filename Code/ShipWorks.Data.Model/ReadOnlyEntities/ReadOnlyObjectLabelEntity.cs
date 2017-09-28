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
    /// Read-only representation of the entity 'ObjectLabel'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyObjectLabelEntity : IObjectLabelEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyObjectLabelEntity(IObjectLabelEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            EntityID = source.EntityID;
            RowVersion = source.RowVersion;
            ObjectType = source.ObjectType;
            ParentID = source.ParentID;
            Label = source.Label;
            IsDeleted = source.IsDeleted;
            
            
            

            CopyCustomObjectLabelData(source);
        }

        
        /// <summary> The EntityID property of the Entity ObjectLabel<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ObjectLabel"."ObjectID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 EntityID { get; }
        /// <summary> The RowVersion property of the Entity ObjectLabel<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ObjectLabel"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The ObjectType property of the Entity ObjectLabel<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ObjectLabel"."ObjectType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ObjectType { get; }
        /// <summary> The ParentID property of the Entity ObjectLabel<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ObjectLabel"."ParentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> ParentID { get; }
        /// <summary> The Label property of the Entity ObjectLabel<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ObjectLabel"."Label"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Label { get; }
        /// <summary> The IsDeleted property of the Entity ObjectLabel<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ObjectLabel"."IsDeleted"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean IsDeleted { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IObjectLabelEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IObjectLabelEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomObjectLabelData(IObjectLabelEntity source);
    }
}
