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
    /// Read-only representation of the entity 'EmailOutboundRelation'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyEmailOutboundRelationEntity : IEmailOutboundRelationEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyEmailOutboundRelationEntity(IEmailOutboundRelationEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            EmailOutboundRelationID = source.EmailOutboundRelationID;
            EmailOutboundID = source.EmailOutboundID;
            EntityID = source.EntityID;
            RelationType = source.RelationType;
            
            
            EmailOutbound = (IEmailOutboundEntity) source.EmailOutbound?.AsReadOnly(objectMap);
            

            CopyCustomEmailOutboundRelationData(source);
        }

        
        /// <summary> The EmailOutboundRelationID property of the Entity EmailOutboundRelation<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutboundRelation"."EmailOutboundRelationID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 EmailOutboundRelationID { get; }
        /// <summary> The EmailOutboundID property of the Entity EmailOutboundRelation<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutboundRelation"."EmailOutboundID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 EmailOutboundID { get; }
        /// <summary> The EntityID property of the Entity EmailOutboundRelation<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutboundRelation"."ObjectID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 EntityID { get; }
        /// <summary> The RelationType property of the Entity EmailOutboundRelation<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutboundRelation"."RelationType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 RelationType { get; }
        
        
        public IEmailOutboundEntity EmailOutbound { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEmailOutboundRelationEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEmailOutboundRelationEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomEmailOutboundRelationData(IEmailOutboundRelationEntity source);
    }
}
