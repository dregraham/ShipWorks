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
    /// Read-only representation of the entity 'WalmartOrder'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyWalmartOrderEntity : ReadOnlyOrderEntity, IWalmartOrderEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyWalmartOrderEntity(IWalmartOrderEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            PurchaseOrderID = source.PurchaseOrderID;
            CustomerOrderID = source.CustomerOrderID;
            EstimatedDeliveryDate = source.EstimatedDeliveryDate;
            EstimatedShipDate = source.EstimatedShipDate;
            RequestedShippingMethodCode = source.RequestedShippingMethodCode;
            
            
            

            CopyCustomWalmartOrderData(source);
        }

        
        /// <summary> The PurchaseOrderID property of the Entity WalmartOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartOrder"."PurchaseOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 32<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PurchaseOrderID { get; }
        /// <summary> The CustomerOrderID property of the Entity WalmartOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartOrder"."CustomerOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CustomerOrderID { get; }
        /// <summary> The EstimatedDeliveryDate property of the Entity WalmartOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartOrder"."EstimatedDeliveryDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime EstimatedDeliveryDate { get; }
        /// <summary> The EstimatedShipDate property of the Entity WalmartOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartOrder"."EstimatedShipDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime EstimatedShipDate { get; }
        /// <summary> The RequestedShippingMethodCode property of the Entity WalmartOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartOrder"."RequestedShippingMethodCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 12<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String RequestedShippingMethodCode { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IWalmartOrderEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IWalmartOrderEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomWalmartOrderData(IWalmartOrderEntity source);
    }
}
