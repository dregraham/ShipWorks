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
    /// Entity interface which represents the entity 'FilterNode'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IFilterNodeEntity
    {
        
        /// <summary> The FilterNodeID property of the Entity FilterNode<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNode"."FilterNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 FilterNodeID { get; }
        /// <summary> The RowVersion property of the Entity FilterNode<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNode"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The ParentFilterNodeID property of the Entity FilterNode<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNode"."ParentFilterNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> ParentFilterNodeID { get; }
        /// <summary> The FilterSequenceID property of the Entity FilterNode<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNode"."FilterSequenceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 FilterSequenceID { get; }
        /// <summary> The FilterNodeContentID property of the Entity FilterNode<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNode"."FilterNodeContentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 FilterNodeContentID { get; }
        /// <summary> The Created property of the Entity FilterNode<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNode"."Created"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime Created { get; }
        /// <summary> The Purpose property of the Entity FilterNode<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNode"."Purpose"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Purpose { get; }
        
        
        IFilterNodeEntity ParentNode { get; }
        IFilterNodeContentEntity FilterNodeContent { get; }
        IFilterSequenceEntity FilterSequence { get; }
        
        IEnumerable<IFilterNodeEntity> ChildNodes { get; }

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IFilterNodeEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IFilterNodeEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'FilterNode'. <br/><br/>
    /// 
    /// </summary>
    public partial class FilterNodeEntity : IFilterNodeEntity
    {
        
        IFilterNodeEntity IFilterNodeEntity.ParentNode => ParentNode;
        IFilterNodeContentEntity IFilterNodeEntity.FilterNodeContent => FilterNodeContent;
        IFilterSequenceEntity IFilterNodeEntity.FilterSequence => FilterSequence;
        
        IEnumerable<IFilterNodeEntity> IFilterNodeEntity.ChildNodes => ChildNodes;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFilterNodeEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IFilterNodeEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IFilterNodeEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyFilterNodeEntity(this, objectMap);
        }

        
    }
}
