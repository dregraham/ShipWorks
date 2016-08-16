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
    /// Entity interface which represents the entity 'Filter'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IFilterEntity
    {
        
        /// <summary> The FilterID property of the Entity Filter<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Filter"."FilterID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 FilterID { get; }
        /// <summary> The RowVersion property of the Entity Filter<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Filter"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The Name property of the Entity Filter<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Filter"."Name"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Name { get; }
        /// <summary> The FilterTarget property of the Entity Filter<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Filter"."FilterTarget"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 FilterTarget { get; }
        /// <summary> The IsFolder property of the Entity Filter<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Filter"."IsFolder"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean IsFolder { get; }
        /// <summary> The Definition property of the Entity Filter<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Filter"."Definition"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String Definition { get; }
        /// <summary> The State property of the Entity Filter<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Filter"."State"<br/>
        /// Table field type characteristics (type, precision, scale, length): TinyInt, 3, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte State { get; }
        
        
        
        IEnumerable<IFilterSequenceEntity> ChildSequences { get; }
        IEnumerable<IFilterSequenceEntity> UsedBySequences { get; }

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IFilterEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IFilterEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'Filter'. <br/><br/>
    /// 
    /// </summary>
    public partial class FilterEntity : IFilterEntity
    {
        
        
        IEnumerable<IFilterSequenceEntity> IFilterEntity.ChildSequences => ChildSequences;
        IEnumerable<IFilterSequenceEntity> IFilterEntity.UsedBySequences => UsedBySequences;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFilterEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IFilterEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IFilterEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyFilterEntity(this, objectMap);
        }
    }
}
