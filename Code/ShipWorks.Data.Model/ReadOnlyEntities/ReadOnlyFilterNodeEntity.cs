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
    /// Read-only representation of the entity 'FilterNode'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyFilterNodeEntity : IFilterNodeEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyFilterNodeEntity(IFilterNodeEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            FilterNodeID = source.FilterNodeID;
            RowVersion = source.RowVersion;
            ParentFilterNodeID = source.ParentFilterNodeID;
            FilterSequenceID = source.FilterSequenceID;
            FilterNodeContentID = source.FilterNodeContentID;
            Created = source.Created;
            Purpose = source.Purpose;
            
            
            ParentNode = (IFilterNodeEntity) source.ParentNode?.AsReadOnly(objectMap);
            FilterNodeContent = (IFilterNodeContentEntity) source.FilterNodeContent?.AsReadOnly(objectMap);
            FilterSequence = (IFilterSequenceEntity) source.FilterSequence?.AsReadOnly(objectMap);
            
            ChildNodes = source.ChildNodes?.Select(x => x.AsReadOnly(objectMap)).OfType<IFilterNodeEntity>().ToReadOnly() ??
                Enumerable.Empty<IFilterNodeEntity>();

            CopyCustomFilterNodeData(source);
        }

        
        /// <summary> The FilterNodeID property of the Entity FilterNode<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNode"."FilterNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 FilterNodeID { get; }
        /// <summary> The RowVersion property of the Entity FilterNode<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNode"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The ParentFilterNodeID property of the Entity FilterNode<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNode"."ParentFilterNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> ParentFilterNodeID { get; }
        /// <summary> The FilterSequenceID property of the Entity FilterNode<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNode"."FilterSequenceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 FilterSequenceID { get; }
        /// <summary> The FilterNodeContentID property of the Entity FilterNode<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNode"."FilterNodeContentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 FilterNodeContentID { get; }
        /// <summary> The Created property of the Entity FilterNode<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNode"."Created"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime Created { get; }
        /// <summary> The Purpose property of the Entity FilterNode<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNode"."Purpose"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Purpose { get; }
        
        
        public IFilterNodeEntity ParentNode { get; }
        
        public IFilterNodeContentEntity FilterNodeContent { get; }
        
        public IFilterSequenceEntity FilterSequence { get; }
        
        
        public IEnumerable<IFilterNodeEntity> ChildNodes { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFilterNodeEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFilterNodeEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomFilterNodeData(IFilterNodeEntity source);
    }
}
