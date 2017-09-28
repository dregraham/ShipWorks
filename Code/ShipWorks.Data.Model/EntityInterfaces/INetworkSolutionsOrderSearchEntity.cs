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
    /// Entity interface which represents the entity 'NetworkSolutionsOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial interface INetworkSolutionsOrderSearchEntity
    {
        
        /// <summary> The NetworkSolutionsOrderSearchID property of the Entity NetworkSolutionsOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NetworkSolutionsOrderSearch"."NetworkSolutionsOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 NetworkSolutionsOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity NetworkSolutionsOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NetworkSolutionsOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The NetworkSolutionsOrderID property of the Entity NetworkSolutionsOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NetworkSolutionsOrderSearch"."NetworkSolutionsOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 NetworkSolutionsOrderID { get; }
        /// <summary> The OriginalOrderID property of the Entity NetworkSolutionsOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NetworkSolutionsOrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OriginalOrderID { get; }
        
        
        INetworkSolutionsOrderEntity NetworkSolutionsOrder { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        INetworkSolutionsOrderSearchEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        INetworkSolutionsOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'NetworkSolutionsOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial class NetworkSolutionsOrderSearchEntity : INetworkSolutionsOrderSearchEntity
    {
        
        INetworkSolutionsOrderEntity INetworkSolutionsOrderSearchEntity.NetworkSolutionsOrder => NetworkSolutionsOrder;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual INetworkSolutionsOrderSearchEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual INetworkSolutionsOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (INetworkSolutionsOrderSearchEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyNetworkSolutionsOrderSearchEntity(this, objectMap);
        }

        
    }
}
