﻿///////////////////////////////////////////////////////////////
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
    /// Entity interface which represents the entity 'FilterSequence'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IFilterSequenceEntity
    {
        
        /// <summary> The FilterSequenceID property of the Entity FilterSequence<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterSequence"."FilterSequenceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 FilterSequenceID { get; }
        /// <summary> The RowVersion property of the Entity FilterSequence<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterSequence"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The ParentFilterID property of the Entity FilterSequence<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterSequence"."ParentFilterID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> ParentFilterID { get; }
        /// <summary> The FilterID property of the Entity FilterSequence<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterSequence"."FilterID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 FilterID { get; }
        /// <summary> The Position property of the Entity FilterSequence<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterSequence"."Position"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Position { get; }
        
        
        IFilterEntity Filter { get; }
        IFilterEntity Parent { get; }
        
        IEnumerable<IFilterNodeEntity> NodesUsingSequence { get; }

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IFilterSequenceEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IFilterSequenceEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'FilterSequence'. <br/><br/>
    /// 
    /// </summary>
    public partial class FilterSequenceEntity : IFilterSequenceEntity
    {
        
        IFilterEntity IFilterSequenceEntity.Filter => Filter;
        IFilterEntity IFilterSequenceEntity.Parent => Parent;
        
        IEnumerable<IFilterNodeEntity> IFilterSequenceEntity.NodesUsingSequence => NodesUsingSequence;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFilterSequenceEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IFilterSequenceEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IFilterSequenceEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyFilterSequenceEntity(this, objectMap);
        }

        
    }
}
