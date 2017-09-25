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
    /// Entity interface which represents the entity 'PayPalOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IPayPalOrderEntity: IOrderEntity
    {
        
        /// <summary> The TransactionID property of the Entity PayPalOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalOrder"."TransactionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String TransactionID { get; }
        /// <summary> The AddressStatus property of the Entity PayPalOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalOrder"."AddressStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 AddressStatus { get; }
        /// <summary> The PayPalFee property of the Entity PayPalOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalOrder"."PayPalFee"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal PayPalFee { get; }
        /// <summary> The PaymentStatus property of the Entity PayPalOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalOrder"."PaymentStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 PaymentStatus { get; }
        
        
        
        IEnumerable<IPayPalOrderSearchEntity> PayPalOrderSearch { get; }

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IPayPalOrderEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IPayPalOrderEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'PayPalOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial class PayPalOrderEntity : IPayPalOrderEntity
    {
        
        
        IEnumerable<IPayPalOrderSearchEntity> IPayPalOrderEntity.PayPalOrderSearch => PayPalOrderSearch;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IPayPalOrderEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IPayPalOrderEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IPayPalOrderEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyPayPalOrderEntity(this, objectMap);
        }
    }
}
