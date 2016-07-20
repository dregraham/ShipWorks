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
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Read-only representation of the entity 'FilterSequence'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyFilterSequenceEntity : IFilterSequenceEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyFilterSequenceEntity(IFilterSequenceEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            FilterSequenceID = source.FilterSequenceID;
            RowVersion = source.RowVersion;
            ParentFilterID = source.ParentFilterID;
            FilterID = source.FilterID;
            Position = source.Position;
            
            
            Parent = source.Parent?.AsReadOnly(objectMap);
            Filter = source.Filter?.AsReadOnly(objectMap);
            
            NodesUsingSequence = source.NodesUsingSequence?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IFilterNodeEntity>();

            CopyCustomFilterSequenceData(source);
        }

        
        /// <summary> The FilterSequenceID property of the Entity FilterSequence<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterSequence"."FilterSequenceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 FilterSequenceID { get; }
        /// <summary> The RowVersion property of the Entity FilterSequence<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterSequence"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The ParentFilterID property of the Entity FilterSequence<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterSequence"."ParentFilterID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> ParentFilterID { get; }
        /// <summary> The FilterID property of the Entity FilterSequence<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterSequence"."FilterID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 FilterID { get; }
        /// <summary> The Position property of the Entity FilterSequence<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterSequence"."Position"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Position { get; }
        
        
        public IFilterEntity Parent { get; }
        
        public IFilterEntity Filter { get; }
        
        
        public IEnumerable<IFilterNodeEntity> NodesUsingSequence { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFilterSequenceEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFilterSequenceEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomFilterSequenceData(IFilterSequenceEntity source);
    }
}
