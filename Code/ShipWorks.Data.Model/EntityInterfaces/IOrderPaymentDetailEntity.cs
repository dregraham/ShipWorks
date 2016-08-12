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
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'OrderPaymentDetail'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IOrderPaymentDetailEntity
    {
        
        /// <summary> The OrderPaymentDetailID property of the Entity OrderPaymentDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderPaymentDetail"."OrderPaymentDetailID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 OrderPaymentDetailID { get; }
        /// <summary> The RowVersion property of the Entity OrderPaymentDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderPaymentDetail"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The OrderID property of the Entity OrderPaymentDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderPaymentDetail"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The Label property of the Entity OrderPaymentDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderPaymentDetail"."Label"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Label { get; }
        /// <summary> The Value property of the Entity OrderPaymentDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderPaymentDetail"."Value"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Value { get; }
        
        
        IOrderEntity Order { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOrderPaymentDetailEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOrderPaymentDetailEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'OrderPaymentDetail'. <br/><br/>
    /// 
    /// </summary>
    public partial class OrderPaymentDetailEntity : IOrderPaymentDetailEntity
    {
        
        IOrderEntity IOrderPaymentDetailEntity.Order => Order;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOrderPaymentDetailEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IOrderPaymentDetailEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IOrderPaymentDetailEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyOrderPaymentDetailEntity(this, objectMap);
        }
    }
}
