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
    /// Entity interface which represents the entity 'EbayStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IEbayStoreEntity: IStoreEntity
    {
        
        /// <summary> The EBayUserID property of the Entity EbayStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayStore"."eBayUserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String EBayUserID { get; }
        /// <summary> The EBayToken property of the Entity EbayStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayStore"."eBayToken"<br/>
        /// Table field type characteristics (type, precision, scale, length): Text, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String EBayToken { get; }
        /// <summary> The EBayTokenExpire property of the Entity EbayStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayStore"."eBayTokenExpire"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime EBayTokenExpire { get; }
        /// <summary> The AcceptedPaymentList property of the Entity EbayStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayStore"."AcceptedPaymentList"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AcceptedPaymentList { get; }
        /// <summary> The DownloadItemDetails property of the Entity EbayStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayStore"."DownloadItemDetails"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean DownloadItemDetails { get; }
        /// <summary> The DownloadOlderOrders property of the Entity EbayStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayStore"."DownloadOlderOrders"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean DownloadOlderOrders { get; }
        /// <summary> The DownloadPayPalDetails property of the Entity EbayStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayStore"."DownloadPayPalDetails"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean DownloadPayPalDetails { get; }
        /// <summary> The PayPalApiCredentialType property of the Entity EbayStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayStore"."PayPalApiCredentialType"<br/>
        /// Table field type characteristics (type, precision, scale, length): SmallInt, 5, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int16 PayPalApiCredentialType { get; }
        /// <summary> The PayPalApiUserName property of the Entity EbayStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayStore"."PayPalApiUserName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PayPalApiUserName { get; }
        /// <summary> The PayPalApiPassword property of the Entity EbayStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayStore"."PayPalApiPassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 80<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PayPalApiPassword { get; }
        /// <summary> The PayPalApiSignature property of the Entity EbayStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayStore"."PayPalApiSignature"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 80<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PayPalApiSignature { get; }
        /// <summary> The PayPalApiCertificate property of the Entity EbayStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayStore"."PayPalApiCertificate"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarBinary, 0, 0, 2048<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.Byte[] PayPalApiCertificate { get; }
        /// <summary> The DomesticShippingService property of the Entity EbayStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayStore"."DomesticShippingService"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String DomesticShippingService { get; }
        /// <summary> The InternationalShippingService property of the Entity EbayStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayStore"."InternationalShippingService"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String InternationalShippingService { get; }
        /// <summary> The FeedbackUpdatedThrough property of the Entity EbayStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayStore"."FeedbackUpdatedThrough"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.DateTime> FeedbackUpdatedThrough { get; }
        
        
        
        IEnumerable<IEbayCombinedOrderRelationEntity> EbayCombinedOrderRelation { get; }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IEbayStoreEntity AsReadOnlyEbayStore();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IEbayStoreEntity AsReadOnlyEbayStore(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'EbayStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class EbayStoreEntity : IEbayStoreEntity
    {
        
        
        IEnumerable<IEbayCombinedOrderRelationEntity> IEbayStoreEntity.EbayCombinedOrderRelation => EbayCombinedOrderRelation;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IStoreEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public override IStoreEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IEbayStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyEbayStoreEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IEbayStoreEntity AsReadOnlyEbayStore() =>
            (IEbayStoreEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IEbayStoreEntity AsReadOnlyEbayStore(IDictionary<object, object> objectMap) =>
            (IEbayStoreEntity) AsReadOnly(objectMap);
        
    }
}
