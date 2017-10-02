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
    /// Read-only representation of the entity 'PayPalOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyPayPalOrderSearchEntity : IPayPalOrderSearchEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyPayPalOrderSearchEntity(IPayPalOrderSearchEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            PayPalOrderSearchID = source.PayPalOrderSearchID;
            OrderID = source.OrderID;
            TransactionID = source.TransactionID;
            OriginalOrderID = source.OriginalOrderID;
            
            
            PayPalOrder = (IPayPalOrderEntity) source.PayPalOrder?.AsReadOnly(objectMap);
            

            CopyCustomPayPalOrderSearchData(source);
        }

        
        /// <summary> The PayPalOrderSearchID property of the Entity PayPalOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalOrderSearch"."PayPalOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 PayPalOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity PayPalOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The TransactionID property of the Entity PayPalOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalOrderSearch"."TransactionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String TransactionID { get; }
        /// <summary> The OriginalOrderID property of the Entity PayPalOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalOrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OriginalOrderID { get; }
        
        
        public IPayPalOrderEntity PayPalOrder { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IPayPalOrderSearchEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IPayPalOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomPayPalOrderSearchData(IPayPalOrderSearchEntity source);
    }
}
