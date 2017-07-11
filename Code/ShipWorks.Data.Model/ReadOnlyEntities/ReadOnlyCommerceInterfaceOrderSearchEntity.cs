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
    /// Read-only representation of the entity 'CommerceInterfaceOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyCommerceInterfaceOrderSearchEntity : ICommerceInterfaceOrderSearchEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyCommerceInterfaceOrderSearchEntity(ICommerceInterfaceOrderSearchEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            CommerceInterfaceOrderSearchID = source.CommerceInterfaceOrderSearchID;
            OrderID = source.OrderID;
            CommerceInterfaceOrderNumber = source.CommerceInterfaceOrderNumber;
            
            
            CommerceInterfaceOrder = source.CommerceInterfaceOrder?.AsReadOnly(objectMap);
            

            CopyCustomCommerceInterfaceOrderSearchData(source);
        }

        
        /// <summary> The CommerceInterfaceOrderSearchID property of the Entity CommerceInterfaceOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "CommerceInterfaceOrderSearch"."CommerceInterfaceOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 CommerceInterfaceOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity CommerceInterfaceOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "CommerceInterfaceOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The CommerceInterfaceOrderNumber property of the Entity CommerceInterfaceOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "CommerceInterfaceOrderSearch"."CommerceInterfaceOrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CommerceInterfaceOrderNumber { get; }
        
        
        public ICommerceInterfaceOrderEntity CommerceInterfaceOrder { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ICommerceInterfaceOrderSearchEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ICommerceInterfaceOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomCommerceInterfaceOrderSearchData(ICommerceInterfaceOrderSearchEntity source);
    }
}
