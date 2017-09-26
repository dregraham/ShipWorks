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
    /// Read-only representation of the entity 'PayPalOrder'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyPayPalOrderEntity : ReadOnlyOrderEntity, IPayPalOrderEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyPayPalOrderEntity(IPayPalOrderEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            TransactionID = source.TransactionID;
            AddressStatus = source.AddressStatus;
            PayPalFee = source.PayPalFee;
            PaymentStatus = source.PaymentStatus;
            
            
            

            CopyCustomPayPalOrderData(source);
        }

        
        /// <summary> The TransactionID property of the Entity PayPalOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalOrder"."TransactionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String TransactionID { get; }
        /// <summary> The AddressStatus property of the Entity PayPalOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalOrder"."AddressStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 AddressStatus { get; }
        /// <summary> The PayPalFee property of the Entity PayPalOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalOrder"."PayPalFee"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal PayPalFee { get; }
        /// <summary> The PaymentStatus property of the Entity PayPalOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PayPalOrder"."PaymentStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 PaymentStatus { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IPayPalOrderEntity AsReadOnlyPayPalOrder() => this;

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IPayPalOrderEntity AsReadOnlyPayPalOrder(IDictionary<object, object> objectMap) => this;
        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomPayPalOrderData(IPayPalOrderEntity source);
    }
}
