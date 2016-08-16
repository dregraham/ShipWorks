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
    /// Entity interface which represents the entity 'EmailOutboundRelation'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IEmailOutboundRelationEntity
    {
        
        /// <summary> The EmailOutboundRelationID property of the Entity EmailOutboundRelation<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutboundRelation"."EmailOutboundRelationID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 EmailOutboundRelationID { get; }
        /// <summary> The EmailOutboundID property of the Entity EmailOutboundRelation<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutboundRelation"."EmailOutboundID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 EmailOutboundID { get; }
        /// <summary> The ObjectID property of the Entity EmailOutboundRelation<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutboundRelation"."ObjectID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ObjectID { get; }
        /// <summary> The RelationType property of the Entity EmailOutboundRelation<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutboundRelation"."RelationType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 RelationType { get; }
        
        
        IEmailOutboundEntity EmailOutbound { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IEmailOutboundRelationEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IEmailOutboundRelationEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'EmailOutboundRelation'. <br/><br/>
    /// 
    /// </summary>
    public partial class EmailOutboundRelationEntity : IEmailOutboundRelationEntity
    {
        
        IEmailOutboundEntity IEmailOutboundRelationEntity.EmailOutbound => EmailOutbound;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEmailOutboundRelationEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IEmailOutboundRelationEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IEmailOutboundRelationEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyEmailOutboundRelationEntity(this, objectMap);
        }
    }
}
