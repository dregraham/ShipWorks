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
    /// Entity interface which represents the entity 'OrderCharge'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IOrderChargeEntity
    {
        
        /// <summary> The OrderChargeID property of the Entity OrderCharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderCharge"."OrderChargeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 OrderChargeID { get; }
        /// <summary> The RowVersion property of the Entity OrderCharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderCharge"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The OrderID property of the Entity OrderCharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderCharge"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The Type property of the Entity OrderCharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderCharge"."Type"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Type { get; }
        /// <summary> The Description property of the Entity OrderCharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderCharge"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Description { get; }
        /// <summary> The Amount property of the Entity OrderCharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderCharge"."Amount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal Amount { get; }
        
        
        IOrderEntity Order { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOrderChargeEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOrderChargeEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'OrderCharge'. <br/><br/>
    /// 
    /// </summary>
    public partial class OrderChargeEntity : IOrderChargeEntity
    {
        
        IOrderEntity IOrderChargeEntity.Order => Order;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOrderChargeEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IOrderChargeEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IOrderChargeEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyOrderChargeEntity(this, objectMap);
        }
    }
}
