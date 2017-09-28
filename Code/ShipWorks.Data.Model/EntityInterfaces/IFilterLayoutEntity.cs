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
    /// Entity interface which represents the entity 'FilterLayout'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IFilterLayoutEntity
    {
        
        /// <summary> The FilterLayoutID property of the Entity FilterLayout<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterLayout"."FilterLayoutID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 FilterLayoutID { get; }
        /// <summary> The RowVersion property of the Entity FilterLayout<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterLayout"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The UserID property of the Entity FilterLayout<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterLayout"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> UserID { get; }
        /// <summary> The FilterTarget property of the Entity FilterLayout<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterLayout"."FilterTarget"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 FilterTarget { get; }
        /// <summary> The FilterNodeID property of the Entity FilterLayout<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterLayout"."FilterNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 FilterNodeID { get; }
        
        
        IFilterNodeEntity FilterNode { get; }
        IUserEntity User { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IFilterLayoutEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IFilterLayoutEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'FilterLayout'. <br/><br/>
    /// 
    /// </summary>
    public partial class FilterLayoutEntity : IFilterLayoutEntity
    {
        
        IFilterNodeEntity IFilterLayoutEntity.FilterNode => FilterNode;
        IUserEntity IFilterLayoutEntity.User => User;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFilterLayoutEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IFilterLayoutEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IFilterLayoutEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyFilterLayoutEntity(this, objectMap);
        }

        
    }
}
