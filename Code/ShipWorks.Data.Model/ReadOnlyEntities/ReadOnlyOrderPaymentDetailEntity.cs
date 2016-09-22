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
    /// Read-only representation of the entity 'OrderPaymentDetail'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyOrderPaymentDetailEntity : IOrderPaymentDetailEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyOrderPaymentDetailEntity(IOrderPaymentDetailEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            OrderPaymentDetailID = source.OrderPaymentDetailID;
            RowVersion = source.RowVersion;
            OrderID = source.OrderID;
            Label = source.Label;
            Value = source.Value;
            
            
            Order = source.Order?.AsReadOnly(objectMap);
            

            CopyCustomOrderPaymentDetailData(source);
        }

        
        /// <summary> The OrderPaymentDetailID property of the Entity OrderPaymentDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderPaymentDetail"."OrderPaymentDetailID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 OrderPaymentDetailID { get; }
        /// <summary> The RowVersion property of the Entity OrderPaymentDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderPaymentDetail"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The OrderID property of the Entity OrderPaymentDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderPaymentDetail"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The Label property of the Entity OrderPaymentDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderPaymentDetail"."Label"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Label { get; }
        /// <summary> The Value property of the Entity OrderPaymentDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderPaymentDetail"."Value"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Value { get; }
        
        
        public IOrderEntity Order { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOrderPaymentDetailEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOrderPaymentDetailEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomOrderPaymentDetailData(IOrderPaymentDetailEntity source);
    }
}
