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
    /// Read-only representation of the entity 'ObjectReference'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyObjectReferenceEntity : IObjectReferenceEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyObjectReferenceEntity(IObjectReferenceEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ObjectReferenceID = source.ObjectReferenceID;
            ConsumerID = source.ConsumerID;
            ReferenceKey = source.ReferenceKey;
            EntityID = source.EntityID;
            Reason = source.Reason;
            
            
            

            CopyCustomObjectReferenceData(source);
        }

        
        /// <summary> The ObjectReferenceID property of the Entity ObjectReference<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ObjectReference"."ObjectReferenceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ObjectReferenceID { get; }
        /// <summary> The ConsumerID property of the Entity ObjectReference<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ObjectReference"."ConsumerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ConsumerID { get; }
        /// <summary> The ReferenceKey property of the Entity ObjectReference<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ObjectReference"."ReferenceKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 250<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ReferenceKey { get; }
        /// <summary> The EntityID property of the Entity ObjectReference<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ObjectReference"."ObjectID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 EntityID { get; }
        /// <summary> The Reason property of the Entity ObjectReference<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ObjectReference"."Reason"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 250<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String Reason { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IObjectReferenceEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IObjectReferenceEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomObjectReferenceData(IObjectReferenceEntity source);
    }
}
