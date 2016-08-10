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
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Read-only representation of the entity 'EbayCombinedOrderRelation'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyEbayCombinedOrderRelationEntity : IEbayCombinedOrderRelationEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyEbayCombinedOrderRelationEntity(IEbayCombinedOrderRelationEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            EbayCombinedOrderRelationID = source.EbayCombinedOrderRelationID;
            OrderID = source.OrderID;
            EbayOrderID = source.EbayOrderID;
            StoreID = source.StoreID;
            
            
            EbayOrder = source.EbayOrder?.AsReadOnly(objectMap);
            EbayStore = source.EbayStore?.AsReadOnly(objectMap);
            

            CopyCustomEbayCombinedOrderRelationData(source);
        }

        
        /// <summary> The EbayCombinedOrderRelationID property of the Entity EbayCombinedOrderRelation<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayCombinedOrderRelation"."EbayCombinedOrderRelationID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 EbayCombinedOrderRelationID { get; }
        /// <summary> The OrderID property of the Entity EbayCombinedOrderRelation<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayCombinedOrderRelation"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The EbayOrderID property of the Entity EbayCombinedOrderRelation<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayCombinedOrderRelation"."EbayOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 EbayOrderID { get; }
        /// <summary> The StoreID property of the Entity EbayCombinedOrderRelation<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayCombinedOrderRelation"."StoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 StoreID { get; }
        
        
        public IEbayOrderEntity EbayOrder { get; }
        
        public IEbayStoreEntity EbayStore { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEbayCombinedOrderRelationEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEbayCombinedOrderRelationEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomEbayCombinedOrderRelationData(IEbayCombinedOrderRelationEntity source);
    }
}
