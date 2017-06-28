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
    /// Read-only representation of the entity 'NetworkSolutionsOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyNetworkSolutionsOrderSearchEntity : INetworkSolutionsOrderSearchEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyNetworkSolutionsOrderSearchEntity(INetworkSolutionsOrderSearchEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            NetworkSolutionsOrderSearchID = source.NetworkSolutionsOrderSearchID;
            OrderID = source.OrderID;
            StoreID = source.StoreID;
            OrderNumber = source.OrderNumber;
            OrderNumberComplete = source.OrderNumberComplete;
            NetworkSolutionsOrderID = source.NetworkSolutionsOrderID;
            
            
            NetworkSolutionsOrder = source.NetworkSolutionsOrder?.AsReadOnly(objectMap);
            Store = source.Store?.AsReadOnly(objectMap);
            

            CopyCustomNetworkSolutionsOrderSearchData(source);
        }

        
        /// <summary> The NetworkSolutionsOrderSearchID property of the Entity NetworkSolutionsOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NetworkSolutionsOrderSearch"."NetworkSolutionsOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 NetworkSolutionsOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity NetworkSolutionsOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NetworkSolutionsOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The StoreID property of the Entity NetworkSolutionsOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NetworkSolutionsOrderSearch"."StoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 StoreID { get; }
        /// <summary> The OrderNumber property of the Entity NetworkSolutionsOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NetworkSolutionsOrderSearch"."OrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderNumber { get; }
        /// <summary> The OrderNumberComplete property of the Entity NetworkSolutionsOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NetworkSolutionsOrderSearch"."OrderNumberComplete"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OrderNumberComplete { get; }
        /// <summary> The NetworkSolutionsOrderID property of the Entity NetworkSolutionsOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NetworkSolutionsOrderSearch"."NetworkSolutionsOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 NetworkSolutionsOrderID { get; }
        
        
        public INetworkSolutionsOrderEntity NetworkSolutionsOrder { get; }
        
        public IStoreEntity Store { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual INetworkSolutionsOrderSearchEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual INetworkSolutionsOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomNetworkSolutionsOrderSearchData(INetworkSolutionsOrderSearchEntity source);
    }
}
