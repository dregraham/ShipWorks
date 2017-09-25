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
    /// Read-only representation of the entity 'ThreeDCartOrder'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyThreeDCartOrderEntity : ReadOnlyOrderEntity, IThreeDCartOrderEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyThreeDCartOrderEntity(IThreeDCartOrderEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ThreeDCartOrderID = source.ThreeDCartOrderID;
            
            
            
            ThreeDCartOrderSearch = source.ThreeDCartOrderSearch?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IThreeDCartOrderSearchEntity>();

            CopyCustomThreeDCartOrderData(source);
        }

        
        /// <summary> The ThreeDCartOrderID property of the Entity ThreeDCartOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ThreeDCartOrder"."ThreeDCartOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ThreeDCartOrderID { get; }
        
        
        
        public IEnumerable<IThreeDCartOrderSearchEntity> ThreeDCartOrderSearch { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IThreeDCartOrderEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IThreeDCartOrderEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomThreeDCartOrderData(IThreeDCartOrderEntity source);
    }
}
