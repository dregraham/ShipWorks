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
    /// Entity interface which represents the entity 'WalmartOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IWalmartOrderEntity: IOrderEntity
    {
        
        /// <summary> The PurchaseOrderID property of the Entity WalmartOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartOrder"."PurchaseOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 32<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PurchaseOrderID { get; }
        /// <summary> The CustomerOrderID property of the Entity WalmartOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartOrder"."CustomerOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String CustomerOrderID { get; }
        /// <summary> The EstimatedDeliveryDate property of the Entity WalmartOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartOrder"."EstimatedDeliveryDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime EstimatedDeliveryDate { get; }
        /// <summary> The EstimatedShipDate property of the Entity WalmartOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartOrder"."EstimatedShipDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime EstimatedShipDate { get; }
        /// <summary> The RequestedShippingMethodCode property of the Entity WalmartOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartOrder"."RequestedShippingMethodCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 12<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String RequestedShippingMethodCode { get; }
        
        
        
        IEnumerable<IWalmartOrderSearchEntity> WalmartOrderSearch { get; }

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IWalmartOrderEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IWalmartOrderEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'WalmartOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial class WalmartOrderEntity : IWalmartOrderEntity
    {
        
        
        IEnumerable<IWalmartOrderSearchEntity> IWalmartOrderEntity.WalmartOrderSearch => WalmartOrderSearch;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IWalmartOrderEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IWalmartOrderEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IWalmartOrderEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyWalmartOrderEntity(this, objectMap);
        }
    }
}
