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
    /// Entity interface which represents the entity 'MagentoOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IMagentoOrderSearchEntity
    {
        
        /// <summary> The MagentoOrderSearchID property of the Entity MagentoOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MagentoOrderSearch"."MagentoOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 MagentoOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity MagentoOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MagentoOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The StoreID property of the Entity MagentoOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MagentoOrderSearch"."StoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 StoreID { get; }
        /// <summary> The OrderNumber property of the Entity MagentoOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MagentoOrderSearch"."OrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderNumber { get; }
        /// <summary> The OrderNumberComplete property of the Entity MagentoOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MagentoOrderSearch"."OrderNumberComplete"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OrderNumberComplete { get; }
        /// <summary> The MagentoOrderID property of the Entity MagentoOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MagentoOrderSearch"."MagentoOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 MagentoOrderID { get; }
        
        
        IMagentoOrderEntity MagentoOrder { get; }
        IStoreEntity Store { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IMagentoOrderSearchEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IMagentoOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'MagentoOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial class MagentoOrderSearchEntity : IMagentoOrderSearchEntity
    {
        
        IMagentoOrderEntity IMagentoOrderSearchEntity.MagentoOrder => MagentoOrder;
        IStoreEntity IMagentoOrderSearchEntity.Store => Store;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IMagentoOrderSearchEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IMagentoOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IMagentoOrderSearchEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyMagentoOrderSearchEntity(this, objectMap);
        }
    }
}
