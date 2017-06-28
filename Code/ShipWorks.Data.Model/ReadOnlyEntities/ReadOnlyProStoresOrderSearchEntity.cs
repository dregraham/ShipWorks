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
    /// Read-only representation of the entity 'ProStoresOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyProStoresOrderSearchEntity : IProStoresOrderSearchEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyProStoresOrderSearchEntity(IProStoresOrderSearchEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ProStoresOrderSearchID = source.ProStoresOrderSearchID;
            OrderID = source.OrderID;
            StoreID = source.StoreID;
            OrderNumber = source.OrderNumber;
            OrderNumberComplete = source.OrderNumberComplete;
            ConfirmationNumber = source.ConfirmationNumber;
            
            
            ProStoresOrder = source.ProStoresOrder?.AsReadOnly(objectMap);
            Store = source.Store?.AsReadOnly(objectMap);
            

            CopyCustomProStoresOrderSearchData(source);
        }

        
        /// <summary> The ProStoresOrderSearchID property of the Entity ProStoresOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresOrderSearch"."ProStoresOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ProStoresOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity ProStoresOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The StoreID property of the Entity ProStoresOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresOrderSearch"."StoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 StoreID { get; }
        /// <summary> The OrderNumber property of the Entity ProStoresOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresOrderSearch"."OrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderNumber { get; }
        /// <summary> The OrderNumberComplete property of the Entity ProStoresOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresOrderSearch"."OrderNumberComplete"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OrderNumberComplete { get; }
        /// <summary> The ConfirmationNumber property of the Entity ProStoresOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresOrderSearch"."ConfirmationNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 12<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ConfirmationNumber { get; }
        
        
        public IProStoresOrderEntity ProStoresOrder { get; }
        
        public IStoreEntity Store { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProStoresOrderSearchEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProStoresOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomProStoresOrderSearchData(IProStoresOrderSearchEntity source);
    }
}
