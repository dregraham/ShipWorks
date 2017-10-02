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
    /// Entity interface which represents the entity 'OrderMotionOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IOrderMotionOrderEntity: IOrderEntity
    {
        
        /// <summary> The OrderMotionShipmentID property of the Entity OrderMotionOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderMotionOrder"."OrderMotionShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 OrderMotionShipmentID { get; }
        /// <summary> The OrderMotionPromotion property of the Entity OrderMotionOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderMotionOrder"."OrderMotionPromotion"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OrderMotionPromotion { get; }
        /// <summary> The OrderMotionInvoiceNumber property of the Entity OrderMotionOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderMotionOrder"."OrderMotionInvoiceNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 64<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OrderMotionInvoiceNumber { get; }
        
        
        
        IEnumerable<IOrderMotionOrderSearchEntity> OrderMotionOrderSearch { get; }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOrderMotionOrderEntity AsReadOnlyOrderMotionOrder();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOrderMotionOrderEntity AsReadOnlyOrderMotionOrder(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'OrderMotionOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial class OrderMotionOrderEntity : IOrderMotionOrderEntity
    {
        
        
        IEnumerable<IOrderMotionOrderSearchEntity> IOrderMotionOrderEntity.OrderMotionOrderSearch => OrderMotionOrderSearch;

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
                return (IOrderMotionOrderEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyOrderMotionOrderEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IOrderMotionOrderEntity AsReadOnlyOrderMotionOrder() =>
            (IOrderMotionOrderEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IOrderMotionOrderEntity AsReadOnlyOrderMotionOrder(IDictionary<object, object> objectMap) =>
            (IOrderMotionOrderEntity) AsReadOnly(objectMap);
        
    }
}
