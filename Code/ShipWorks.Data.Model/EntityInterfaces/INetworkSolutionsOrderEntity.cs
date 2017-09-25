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
    /// Entity interface which represents the entity 'NetworkSolutionsOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial interface INetworkSolutionsOrderEntity: IOrderEntity
    {
        
        /// <summary> The NetworkSolutionsOrderID property of the Entity NetworkSolutionsOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NetworkSolutionsOrder"."NetworkSolutionsOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 NetworkSolutionsOrderID { get; }
        
        
        
        IEnumerable<INetworkSolutionsOrderSearchEntity> NetworkSolutionsOrderSearch { get; }

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new INetworkSolutionsOrderEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new INetworkSolutionsOrderEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'NetworkSolutionsOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial class NetworkSolutionsOrderEntity : INetworkSolutionsOrderEntity
    {
        
        
        IEnumerable<INetworkSolutionsOrderSearchEntity> INetworkSolutionsOrderEntity.NetworkSolutionsOrderSearch => NetworkSolutionsOrderSearch;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new INetworkSolutionsOrderEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new INetworkSolutionsOrderEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (INetworkSolutionsOrderEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyNetworkSolutionsOrderEntity(this, objectMap);
        }
    }
}
