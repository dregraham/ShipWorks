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
    /// Entity interface which represents the entity 'CommerceInterfaceOrder'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyCommerceInterfaceOrderEntity : ReadOnlyOrderEntity, ICommerceInterfaceOrderEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyCommerceInterfaceOrderEntity(ICommerceInterfaceOrderEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            CommerceInterfaceOrderNumber = source.CommerceInterfaceOrderNumber;
            
            
            

            CopyCustomCommerceInterfaceOrderData(source);
        }

        
        /// <summary> The CommerceInterfaceOrderNumber property of the Entity CommerceInterfaceOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "CommerceInterfaceOrder"."CommerceInterfaceOrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CommerceInterfaceOrderNumber { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new ICommerceInterfaceOrderEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new ICommerceInterfaceOrderEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomCommerceInterfaceOrderData(ICommerceInterfaceOrderEntity source);
    }
}
