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
    /// Read-only representation of the entity 'EbayOrder'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyEbayOrderEntity : ReadOnlyOrderEntity, IEbayOrderEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyEbayOrderEntity(IEbayOrderEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            EbayOrderID = source.EbayOrderID;
            EbayBuyerID = source.EbayBuyerID;
            CombinedLocally = source.CombinedLocally;
            SelectedShippingMethod = source.SelectedShippingMethod;
            SellingManagerRecord = source.SellingManagerRecord;
            GspEligible = source.GspEligible;
            GspFirstName = source.GspFirstName;
            GspLastName = source.GspLastName;
            GspStreet1 = source.GspStreet1;
            GspStreet2 = source.GspStreet2;
            GspCity = source.GspCity;
            GspStateProvince = source.GspStateProvince;
            GspPostalCode = source.GspPostalCode;
            GspCountryCode = source.GspCountryCode;
            GspReferenceID = source.GspReferenceID;
            RollupEbayItemCount = source.RollupEbayItemCount;
            RollupEffectiveCheckoutStatus = source.RollupEffectiveCheckoutStatus;
            RollupEffectivePaymentMethod = source.RollupEffectivePaymentMethod;
            RollupFeedbackLeftType = source.RollupFeedbackLeftType;
            RollupFeedbackLeftComments = source.RollupFeedbackLeftComments;
            RollupFeedbackReceivedType = source.RollupFeedbackReceivedType;
            RollupFeedbackReceivedComments = source.RollupFeedbackReceivedComments;
            RollupPayPalAddressStatus = source.RollupPayPalAddressStatus;
            
            
            
            EbayCombinedOrderRelation = source.EbayCombinedOrderRelation?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IEbayCombinedOrderRelationEntity>();

            CopyCustomEbayOrderData(source);
        }

        
        /// <summary> The EbayOrderID property of the Entity EbayOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrder"."EbayOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 EbayOrderID { get; }
        /// <summary> The EbayBuyerID property of the Entity EbayOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrder"."EbayBuyerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String EbayBuyerID { get; }
        /// <summary> The CombinedLocally property of the Entity EbayOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrder"."CombinedLocally"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean CombinedLocally { get; }
        /// <summary> The SelectedShippingMethod property of the Entity EbayOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrder"."SelectedShippingMethod"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 SelectedShippingMethod { get; }
        /// <summary> The SellingManagerRecord property of the Entity EbayOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrder"."SellingManagerRecord"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> SellingManagerRecord { get; }
        /// <summary> The GspEligible property of the Entity EbayOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrder"."GspEligible"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean GspEligible { get; }
        /// <summary> The GspFirstName property of the Entity EbayOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrder"."GspFirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 128<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String GspFirstName { get; }
        /// <summary> The GspLastName property of the Entity EbayOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrder"."GspLastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 128<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String GspLastName { get; }
        /// <summary> The GspStreet1 property of the Entity EbayOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrder"."GspStreet1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 512<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String GspStreet1 { get; }
        /// <summary> The GspStreet2 property of the Entity EbayOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrder"."GspStreet2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 512<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String GspStreet2 { get; }
        /// <summary> The GspCity property of the Entity EbayOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrder"."GspCity"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 128<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String GspCity { get; }
        /// <summary> The GspStateProvince property of the Entity EbayOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrder"."GspStateProvince"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 128<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String GspStateProvince { get; }
        /// <summary> The GspPostalCode property of the Entity EbayOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrder"."GspPostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 9<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String GspPostalCode { get; }
        /// <summary> The GspCountryCode property of the Entity EbayOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrder"."GspCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String GspCountryCode { get; }
        /// <summary> The GspReferenceID property of the Entity EbayOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrder"."GspReferenceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 128<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String GspReferenceID { get; }
        /// <summary> The RollupEbayItemCount property of the Entity EbayOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrder"."RollupEbayItemCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 RollupEbayItemCount { get; }
        /// <summary> The RollupEffectiveCheckoutStatus property of the Entity EbayOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrder"."RollupEffectiveCheckoutStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> RollupEffectiveCheckoutStatus { get; }
        /// <summary> The RollupEffectivePaymentMethod property of the Entity EbayOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrder"."RollupEffectivePaymentMethod"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> RollupEffectivePaymentMethod { get; }
        /// <summary> The RollupFeedbackLeftType property of the Entity EbayOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrder"."RollupFeedbackLeftType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> RollupFeedbackLeftType { get; }
        /// <summary> The RollupFeedbackLeftComments property of the Entity EbayOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrder"."RollupFeedbackLeftComments"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 80<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String RollupFeedbackLeftComments { get; }
        /// <summary> The RollupFeedbackReceivedType property of the Entity EbayOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrder"."RollupFeedbackReceivedType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> RollupFeedbackReceivedType { get; }
        /// <summary> The RollupFeedbackReceivedComments property of the Entity EbayOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrder"."RollupFeedbackReceivedComments"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 80<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String RollupFeedbackReceivedComments { get; }
        /// <summary> The RollupPayPalAddressStatus property of the Entity EbayOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrder"."RollupPayPalAddressStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> RollupPayPalAddressStatus { get; }
        
        
        
        public IEnumerable<IEbayCombinedOrderRelationEntity> EbayCombinedOrderRelation { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IEbayOrderEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IEbayOrderEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomEbayOrderData(IEbayOrderEntity source);
    }
}
