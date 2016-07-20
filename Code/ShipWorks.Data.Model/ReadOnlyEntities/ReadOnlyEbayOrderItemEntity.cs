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
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Entity interface which represents the entity 'EbayOrderItem'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyEbayOrderItemEntity : ReadOnlyOrderItemEntity, IEbayOrderItemEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyEbayOrderItemEntity(IEbayOrderItemEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            LocalEbayOrderID = source.LocalEbayOrderID;
            EbayItemID = source.EbayItemID;
            EbayTransactionID = source.EbayTransactionID;
            SellingManagerRecord = source.SellingManagerRecord;
            EffectiveCheckoutStatus = source.EffectiveCheckoutStatus;
            EffectivePaymentMethod = source.EffectivePaymentMethod;
            PaymentStatus = source.PaymentStatus;
            PaymentMethod = source.PaymentMethod;
            CompleteStatus = source.CompleteStatus;
            FeedbackLeftType = source.FeedbackLeftType;
            FeedbackLeftComments = source.FeedbackLeftComments;
            FeedbackReceivedType = source.FeedbackReceivedType;
            FeedbackReceivedComments = source.FeedbackReceivedComments;
            MyEbayPaid = source.MyEbayPaid;
            MyEbayShipped = source.MyEbayShipped;
            PayPalTransactionID = source.PayPalTransactionID;
            PayPalAddressStatus = source.PayPalAddressStatus;
            
            
            

            CopyCustomEbayOrderItemData(source);
        }

        
        /// <summary> The LocalEbayOrderID property of the Entity EbayOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderItem"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 LocalEbayOrderID { get; }
        /// <summary> The EbayItemID property of the Entity EbayOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderItem"."EbayItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 EbayItemID { get; }
        /// <summary> The EbayTransactionID property of the Entity EbayOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderItem"."EbayTransactionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 EbayTransactionID { get; }
        /// <summary> The SellingManagerRecord property of the Entity EbayOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderItem"."SellingManagerRecord"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 SellingManagerRecord { get; }
        /// <summary> The EffectiveCheckoutStatus property of the Entity EbayOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderItem"."EffectiveCheckoutStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 EffectiveCheckoutStatus { get; }
        /// <summary> The EffectivePaymentMethod property of the Entity EbayOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderItem"."EffectivePaymentMethod"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 EffectivePaymentMethod { get; }
        /// <summary> The PaymentStatus property of the Entity EbayOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderItem"."PaymentStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 PaymentStatus { get; }
        /// <summary> The PaymentMethod property of the Entity EbayOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderItem"."PaymentMethod"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 PaymentMethod { get; }
        /// <summary> The CompleteStatus property of the Entity EbayOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderItem"."CompleteStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 CompleteStatus { get; }
        /// <summary> The FeedbackLeftType property of the Entity EbayOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderItem"."FeedbackLeftType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 FeedbackLeftType { get; }
        /// <summary> The FeedbackLeftComments property of the Entity EbayOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderItem"."FeedbackLeftComments"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 80<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FeedbackLeftComments { get; }
        /// <summary> The FeedbackReceivedType property of the Entity EbayOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderItem"."FeedbackReceivedType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 FeedbackReceivedType { get; }
        /// <summary> The FeedbackReceivedComments property of the Entity EbayOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderItem"."FeedbackReceivedComments"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 80<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FeedbackReceivedComments { get; }
        /// <summary> The MyEbayPaid property of the Entity EbayOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderItem"."MyEbayPaid"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean MyEbayPaid { get; }
        /// <summary> The MyEbayShipped property of the Entity EbayOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderItem"."MyEbayShipped"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean MyEbayShipped { get; }
        /// <summary> The PayPalTransactionID property of the Entity EbayOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderItem"."PayPalTransactionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PayPalTransactionID { get; }
        /// <summary> The PayPalAddressStatus property of the Entity EbayOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderItem"."PayPalAddressStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 PayPalAddressStatus { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IEbayOrderItemEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IEbayOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomEbayOrderItemData(IEbayOrderItemEntity source);
    }
}
