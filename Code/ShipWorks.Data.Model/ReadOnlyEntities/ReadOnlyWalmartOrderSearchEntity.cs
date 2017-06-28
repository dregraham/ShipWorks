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
    /// Read-only representation of the entity 'WalmartOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyWalmartOrderSearchEntity : IWalmartOrderSearchEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyWalmartOrderSearchEntity(IWalmartOrderSearchEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            WalmartOrderSearchID = source.WalmartOrderSearchID;
            OrderID = source.OrderID;
            StoreID = source.StoreID;
            OrderNumber = source.OrderNumber;
            OrderNumberComplete = source.OrderNumberComplete;
            PurchaseOrderID = source.PurchaseOrderID;
            
            
            Store = source.Store?.AsReadOnly(objectMap);
            WalmartOrder = source.WalmartOrder?.AsReadOnly(objectMap);
            

            CopyCustomWalmartOrderSearchData(source);
        }

        
        /// <summary> The WalmartOrderSearchID property of the Entity WalmartOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartOrderSearch"."WalmartOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 WalmartOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity WalmartOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The StoreID property of the Entity WalmartOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartOrderSearch"."StoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 StoreID { get; }
        /// <summary> The OrderNumber property of the Entity WalmartOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartOrderSearch"."OrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderNumber { get; }
        /// <summary> The OrderNumberComplete property of the Entity WalmartOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartOrderSearch"."OrderNumberComplete"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OrderNumberComplete { get; }
        /// <summary> The PurchaseOrderID property of the Entity WalmartOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartOrderSearch"."PurchaseOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 32<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PurchaseOrderID { get; }
        
        
        public IStoreEntity Store { get; }
        
        public IWalmartOrderEntity WalmartOrder { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IWalmartOrderSearchEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IWalmartOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomWalmartOrderSearchData(IWalmartOrderSearchEntity source);
    }
}
