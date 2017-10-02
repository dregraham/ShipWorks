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
    /// Entity interface which represents the entity 'PayPalOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IPayPalOrderSearchEntity
    {
        
        /// <summary> The PayPalOrderSearchID property of the Entity PayPalOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalOrderSearch"."PayPalOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 PayPalOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity PayPalOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The TransactionID property of the Entity PayPalOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalOrderSearch"."TransactionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String TransactionID { get; }
        /// <summary> The OriginalOrderID property of the Entity PayPalOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalOrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OriginalOrderID { get; }
        
        
        IPayPalOrderEntity PayPalOrder { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IPayPalOrderSearchEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IPayPalOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'PayPalOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial class PayPalOrderSearchEntity : IPayPalOrderSearchEntity
    {
        
        IPayPalOrderEntity IPayPalOrderSearchEntity.PayPalOrder => PayPalOrder;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IPayPalOrderSearchEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IPayPalOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IPayPalOrderSearchEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyPayPalOrderSearchEntity(this, objectMap);
        }

        
    }
}
