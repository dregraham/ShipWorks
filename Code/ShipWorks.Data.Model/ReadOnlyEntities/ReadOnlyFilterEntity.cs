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
    /// Read-only representation of the entity 'Filter'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyFilterEntity : IFilterEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyFilterEntity(IFilterEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            FilterID = source.FilterID;
            RowVersion = source.RowVersion;
            Name = source.Name;
            FilterTarget = source.FilterTarget;
            IsFolder = source.IsFolder;
            Definition = source.Definition;
            State = source.State;
            
            
            
            ChildSequences = source.ChildSequences?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IFilterSequenceEntity>();
            UsedBySequences = source.UsedBySequences?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IFilterSequenceEntity>();

            CopyCustomFilterData(source);
        }

        
        /// <summary> The FilterID property of the Entity Filter<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Filter"."FilterID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 FilterID { get; }
        /// <summary> The RowVersion property of the Entity Filter<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Filter"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The Name property of the Entity Filter<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Filter"."Name"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Name { get; }
        /// <summary> The FilterTarget property of the Entity Filter<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Filter"."FilterTarget"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 FilterTarget { get; }
        /// <summary> The IsFolder property of the Entity Filter<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Filter"."IsFolder"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean IsFolder { get; }
        /// <summary> The Definition property of the Entity Filter<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Filter"."Definition"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String Definition { get; }
        /// <summary> The State property of the Entity Filter<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Filter"."State"<br/>
        /// Table field type characteristics (type, precision, scale, length): TinyInt, 3, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte State { get; }
        
        
        
        public IEnumerable<IFilterSequenceEntity> ChildSequences { get; }
        
        public IEnumerable<IFilterSequenceEntity> UsedBySequences { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFilterEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFilterEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomFilterData(IFilterEntity source);
    }
}
