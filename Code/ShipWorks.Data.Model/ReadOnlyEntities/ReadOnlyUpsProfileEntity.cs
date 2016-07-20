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
    /// Read-only representation of the entity 'UpsProfile'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyUpsProfileEntity : IUpsProfileEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyUpsProfileEntity(IUpsProfileEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShippingProfileID = source.ShippingProfileID;
            UpsAccountID = source.UpsAccountID;
            Service = source.Service;
            SaturdayDelivery = source.SaturdayDelivery;
            ResidentialDetermination = source.ResidentialDetermination;
            DeliveryConfirmation = source.DeliveryConfirmation;
            ReferenceNumber = source.ReferenceNumber;
            ReferenceNumber2 = source.ReferenceNumber2;
            PayorType = source.PayorType;
            PayorAccount = source.PayorAccount;
            PayorPostalCode = source.PayorPostalCode;
            PayorCountryCode = source.PayorCountryCode;
            EmailNotifySender = source.EmailNotifySender;
            EmailNotifyRecipient = source.EmailNotifyRecipient;
            EmailNotifyOther = source.EmailNotifyOther;
            EmailNotifyOtherAddress = source.EmailNotifyOtherAddress;
            EmailNotifyFrom = source.EmailNotifyFrom;
            EmailNotifySubject = source.EmailNotifySubject;
            EmailNotifyMessage = source.EmailNotifyMessage;
            ReturnService = source.ReturnService;
            ReturnUndeliverableEmail = source.ReturnUndeliverableEmail;
            ReturnContents = source.ReturnContents;
            Endorsement = source.Endorsement;
            Subclassification = source.Subclassification;
            PaperlessAdditionalDocumentation = source.PaperlessAdditionalDocumentation;
            ShipperRelease = source.ShipperRelease;
            CarbonNeutral = source.CarbonNeutral;
            CommercialPaperlessInvoice = source.CommercialPaperlessInvoice;
            CostCenter = source.CostCenter;
            IrregularIndicator = source.IrregularIndicator;
            Cn22Number = source.Cn22Number;
            ShipmentChargeType = source.ShipmentChargeType;
            ShipmentChargeAccount = source.ShipmentChargeAccount;
            ShipmentChargePostalCode = source.ShipmentChargePostalCode;
            ShipmentChargeCountryCode = source.ShipmentChargeCountryCode;
            UspsPackageID = source.UspsPackageID;
            
            ShippingProfile = source.ShippingProfile?.AsReadOnly(objectMap);
            
            
            Packages = source.Packages?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IUpsProfilePackageEntity>();

            CopyCustomUpsProfileData(source);
        }

        
        /// <summary> The ShippingProfileID property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ShippingProfileID { get; }
        /// <summary> The UpsAccountID property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."UpsAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> UpsAccountID { get; }
        /// <summary> The Service property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> Service { get; }
        /// <summary> The SaturdayDelivery property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."SaturdayDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> SaturdayDelivery { get; }
        /// <summary> The ResidentialDetermination property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."ResidentialDetermination"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> ResidentialDetermination { get; }
        /// <summary> The DeliveryConfirmation property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."DeliveryConfirmation"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> DeliveryConfirmation { get; }
        /// <summary> The ReferenceNumber property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."ReferenceNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ReferenceNumber { get; }
        /// <summary> The ReferenceNumber2 property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."ReferenceNumber2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ReferenceNumber2 { get; }
        /// <summary> The PayorType property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."PayorType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> PayorType { get; }
        /// <summary> The PayorAccount property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."PayorAccount"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String PayorAccount { get; }
        /// <summary> The PayorPostalCode property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."PayorPostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String PayorPostalCode { get; }
        /// <summary> The PayorCountryCode property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."PayorCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String PayorCountryCode { get; }
        /// <summary> The EmailNotifySender property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."EmailNotifySender"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> EmailNotifySender { get; }
        /// <summary> The EmailNotifyRecipient property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."EmailNotifyRecipient"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> EmailNotifyRecipient { get; }
        /// <summary> The EmailNotifyOther property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."EmailNotifyOther"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> EmailNotifyOther { get; }
        /// <summary> The EmailNotifyOtherAddress property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."EmailNotifyOtherAddress"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String EmailNotifyOtherAddress { get; }
        /// <summary> The EmailNotifyFrom property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."EmailNotifyFrom"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String EmailNotifyFrom { get; }
        /// <summary> The EmailNotifySubject property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."EmailNotifySubject"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> EmailNotifySubject { get; }
        /// <summary> The EmailNotifyMessage property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."EmailNotifyMessage"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 120<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String EmailNotifyMessage { get; }
        /// <summary> The ReturnService property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."ReturnService"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> ReturnService { get; }
        /// <summary> The ReturnUndeliverableEmail property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."ReturnUndeliverableEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ReturnUndeliverableEmail { get; }
        /// <summary> The ReturnContents property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."ReturnContents"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ReturnContents { get; }
        /// <summary> The Endorsement property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."Endorsement"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> Endorsement { get; }
        /// <summary> The Subclassification property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."Subclassification"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> Subclassification { get; }
        /// <summary> The PaperlessAdditionalDocumentation property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."PaperlessAdditionalDocumentation"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> PaperlessAdditionalDocumentation { get; }
        /// <summary> The ShipperRelease property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."ShipperRelease"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> ShipperRelease { get; }
        /// <summary> The CarbonNeutral property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."CarbonNeutral"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> CarbonNeutral { get; }
        /// <summary> The CommercialPaperlessInvoice property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."CommercialPaperlessInvoice"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> CommercialPaperlessInvoice { get; }
        /// <summary> The CostCenter property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."CostCenter"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String CostCenter { get; }
        /// <summary> The IrregularIndicator property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."IrregularIndicator"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> IrregularIndicator { get; }
        /// <summary> The Cn22Number property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."Cn22Number"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String Cn22Number { get; }
        /// <summary> The ShipmentChargeType property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."ShipmentChargeType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> ShipmentChargeType { get; }
        /// <summary> The ShipmentChargeAccount property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."ShipmentChargeAccount"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ShipmentChargeAccount { get; }
        /// <summary> The ShipmentChargePostalCode property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."ShipmentChargePostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ShipmentChargePostalCode { get; }
        /// <summary> The ShipmentChargeCountryCode property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."ShipmentChargeCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ShipmentChargeCountryCode { get; }
        /// <summary> The UspsPackageID property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."UspsPackageID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String UspsPackageID { get; }
        
        public IShippingProfileEntity ShippingProfile { get; }
        
        
        
        public IEnumerable<IUpsProfilePackageEntity> Packages { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsProfileEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsProfileEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomUpsProfileData(IUpsProfileEntity source);
    }
}
