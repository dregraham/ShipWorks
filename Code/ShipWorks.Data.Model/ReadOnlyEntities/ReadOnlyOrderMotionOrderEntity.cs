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
    /// Read-only representation of the entity 'OrderMotionOrder'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyOrderMotionOrderEntity : ReadOnlyOrderEntity, IOrderMotionOrderEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyOrderMotionOrderEntity(IOrderMotionOrderEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            OrderMotionShipmentID = source.OrderMotionShipmentID;
            OrderMotionPromotion = source.OrderMotionPromotion;
            OrderMotionInvoiceNumber = source.OrderMotionInvoiceNumber;
            
            
            
            OrderMotionOrderSearch = source.OrderMotionOrderSearch?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IOrderMotionOrderSearchEntity>();

            CopyCustomOrderMotionOrderData(source);
        }

        
        /// <summary> The OrderMotionShipmentID property of the Entity OrderMotionOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderMotionOrder"."OrderMotionShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 OrderMotionShipmentID { get; }
        /// <summary> The OrderMotionPromotion property of the Entity OrderMotionOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderMotionOrder"."OrderMotionPromotion"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OrderMotionPromotion { get; }
        /// <summary> The OrderMotionInvoiceNumber property of the Entity OrderMotionOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderMotionOrder"."OrderMotionInvoiceNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 64<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OrderMotionInvoiceNumber { get; }
        
        
        
        public IEnumerable<IOrderMotionOrderSearchEntity> OrderMotionOrderSearch { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IOrderMotionOrderEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IOrderMotionOrderEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomOrderMotionOrderData(IOrderMotionOrderEntity source);
    }
}
