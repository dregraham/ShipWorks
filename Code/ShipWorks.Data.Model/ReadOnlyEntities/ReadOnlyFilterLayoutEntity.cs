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
    /// Read-only representation of the entity 'FilterLayout'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyFilterLayoutEntity : IFilterLayoutEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyFilterLayoutEntity(IFilterLayoutEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            FilterLayoutID = source.FilterLayoutID;
            RowVersion = source.RowVersion;
            UserID = source.UserID;
            FilterTarget = source.FilterTarget;
            FilterNodeID = source.FilterNodeID;
            
            
            FilterNode = (IFilterNodeEntity) source.FilterNode?.AsReadOnly(objectMap);
            User = (IUserEntity) source.User?.AsReadOnly(objectMap);
            

            CopyCustomFilterLayoutData(source);
        }

        
        /// <summary> The FilterLayoutID property of the Entity FilterLayout<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterLayout"."FilterLayoutID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 FilterLayoutID { get; }
        /// <summary> The RowVersion property of the Entity FilterLayout<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterLayout"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The UserID property of the Entity FilterLayout<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterLayout"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> UserID { get; }
        /// <summary> The FilterTarget property of the Entity FilterLayout<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterLayout"."FilterTarget"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 FilterTarget { get; }
        /// <summary> The FilterNodeID property of the Entity FilterLayout<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterLayout"."FilterNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 FilterNodeID { get; }
        
        
        public IFilterNodeEntity FilterNode { get; }
        
        public IUserEntity User { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFilterLayoutEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFilterLayoutEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomFilterLayoutData(IFilterLayoutEntity source);
    }
}
