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
    /// Read-only representation of the entity 'SearsOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlySearsOrderSearchEntity : ISearsOrderSearchEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlySearsOrderSearchEntity(ISearsOrderSearchEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            SearsOrderSearchID = source.SearsOrderSearchID;
            OrderID = source.OrderID;
            StoreID = source.StoreID;
            OrderNumber = source.OrderNumber;
            OrderNumberComplete = source.OrderNumberComplete;
            PoNumber = source.PoNumber;
            
            
            SearsOrder = source.SearsOrder?.AsReadOnly(objectMap);
            Store = source.Store?.AsReadOnly(objectMap);
            

            CopyCustomSearsOrderSearchData(source);
        }

        
        /// <summary> The SearsOrderSearchID property of the Entity SearsOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrderSearch"."SearsOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 SearsOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity SearsOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The StoreID property of the Entity SearsOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrderSearch"."StoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 StoreID { get; }
        /// <summary> The OrderNumber property of the Entity SearsOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrderSearch"."OrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderNumber { get; }
        /// <summary> The OrderNumberComplete property of the Entity SearsOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrderSearch"."OrderNumberComplete"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OrderNumberComplete { get; }
        /// <summary> The PoNumber property of the Entity SearsOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrderSearch"."PoNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PoNumber { get; }
        
        
        public ISearsOrderEntity SearsOrder { get; }
        
        public IStoreEntity Store { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ISearsOrderSearchEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ISearsOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomSearsOrderSearchData(ISearsOrderSearchEntity source);
    }
}
