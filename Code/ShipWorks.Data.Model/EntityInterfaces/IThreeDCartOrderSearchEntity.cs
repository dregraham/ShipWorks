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
    /// Entity interface which represents the entity 'ThreeDCartOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IThreeDCartOrderSearchEntity
    {
        
        /// <summary> The ThreeDCartOrderSearchID property of the Entity ThreeDCartOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ThreeDCartOrderSearch"."ThreeDCartOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ThreeDCartOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity ThreeDCartOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ThreeDCartOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The ThreeDCartOrderID property of the Entity ThreeDCartOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ThreeDCartOrderSearch"."ThreeDCartOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ThreeDCartOrderID { get; }
        /// <summary> The OriginalOrderID property of the Entity ThreeDCartOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ThreeDCartOrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OriginalOrderID { get; }
        
        
        IThreeDCartOrderEntity ThreeDCartOrder { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IThreeDCartOrderSearchEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IThreeDCartOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ThreeDCartOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial class ThreeDCartOrderSearchEntity : IThreeDCartOrderSearchEntity
    {
        
        IThreeDCartOrderEntity IThreeDCartOrderSearchEntity.ThreeDCartOrder => ThreeDCartOrder;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IThreeDCartOrderSearchEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IThreeDCartOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IThreeDCartOrderSearchEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyThreeDCartOrderSearchEntity(this, objectMap);
        }

        
    }
}
