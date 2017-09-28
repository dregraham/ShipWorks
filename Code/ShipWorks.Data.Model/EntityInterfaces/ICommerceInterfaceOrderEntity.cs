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
    /// Entity interface which represents the entity 'CommerceInterfaceOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial interface ICommerceInterfaceOrderEntity: IOrderEntity
    {
        
        /// <summary> The CommerceInterfaceOrderNumber property of the Entity CommerceInterfaceOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "CommerceInterfaceOrder"."CommerceInterfaceOrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String CommerceInterfaceOrderNumber { get; }
        
        
        
        IEnumerable<ICommerceInterfaceOrderSearchEntity> CommerceInterfaceOrderSearch { get; }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        ICommerceInterfaceOrderEntity AsReadOnlyCommerceInterfaceOrder();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        ICommerceInterfaceOrderEntity AsReadOnlyCommerceInterfaceOrder(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'CommerceInterfaceOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial class CommerceInterfaceOrderEntity : ICommerceInterfaceOrderEntity
    {
        
        
        IEnumerable<ICommerceInterfaceOrderSearchEntity> ICommerceInterfaceOrderEntity.CommerceInterfaceOrderSearch => CommerceInterfaceOrderSearch;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public override IOrderEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (ICommerceInterfaceOrderEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyCommerceInterfaceOrderEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public ICommerceInterfaceOrderEntity AsReadOnlyCommerceInterfaceOrder() =>
            (ICommerceInterfaceOrderEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public ICommerceInterfaceOrderEntity AsReadOnlyCommerceInterfaceOrder(IDictionary<object, object> objectMap) =>
            (ICommerceInterfaceOrderEntity) AsReadOnly(objectMap);
        
    }
}
