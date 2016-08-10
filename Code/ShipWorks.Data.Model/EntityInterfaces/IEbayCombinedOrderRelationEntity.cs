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
    /// Entity interface which represents the entity 'EbayCombinedOrderRelation'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IEbayCombinedOrderRelationEntity
    {
        
        /// <summary> The EbayCombinedOrderRelationID property of the Entity EbayCombinedOrderRelation<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayCombinedOrderRelation"."EbayCombinedOrderRelationID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 EbayCombinedOrderRelationID { get; }
        /// <summary> The OrderID property of the Entity EbayCombinedOrderRelation<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayCombinedOrderRelation"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The EbayOrderID property of the Entity EbayCombinedOrderRelation<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayCombinedOrderRelation"."EbayOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 EbayOrderID { get; }
        /// <summary> The StoreID property of the Entity EbayCombinedOrderRelation<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayCombinedOrderRelation"."StoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 StoreID { get; }
        
        
        IEbayOrderEntity EbayOrder { get; }
        IEbayStoreEntity EbayStore { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IEbayCombinedOrderRelationEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IEbayCombinedOrderRelationEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'EbayCombinedOrderRelation'. <br/><br/>
    /// 
    /// </summary>
    public partial class EbayCombinedOrderRelationEntity : IEbayCombinedOrderRelationEntity
    {
        
        IEbayOrderEntity IEbayCombinedOrderRelationEntity.EbayOrder => EbayOrder;
        IEbayStoreEntity IEbayCombinedOrderRelationEntity.EbayStore => EbayStore;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEbayCombinedOrderRelationEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IEbayCombinedOrderRelationEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IEbayCombinedOrderRelationEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyEbayCombinedOrderRelationEntity(this, objectMap);
        }
    }
}
