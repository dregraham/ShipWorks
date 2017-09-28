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
    /// Read-only representation of the entity 'FedExProfile'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyFedExProfileEntity : IFedExProfileEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyFedExProfileEntity(IFedExProfileEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShippingProfileID = source.ShippingProfileID;
            FedExAccountID = source.FedExAccountID;
            Service = source.Service;
            Signature = source.Signature;
            PackagingType = source.PackagingType;
            NonStandardContainer = source.NonStandardContainer;
            ReferenceCustomer = source.ReferenceCustomer;
            ReferenceInvoice = source.ReferenceInvoice;
            ReferencePO = source.ReferencePO;
            ReferenceShipmentIntegrity = source.ReferenceShipmentIntegrity;
            PayorTransportType = source.PayorTransportType;
            PayorTransportAccount = source.PayorTransportAccount;
            PayorDutiesType = source.PayorDutiesType;
            PayorDutiesAccount = source.PayorDutiesAccount;
            SaturdayDelivery = source.SaturdayDelivery;
            EmailNotifySender = source.EmailNotifySender;
            EmailNotifyRecipient = source.EmailNotifyRecipient;
            EmailNotifyOther = source.EmailNotifyOther;
            EmailNotifyOtherAddress = source.EmailNotifyOtherAddress;
            EmailNotifyMessage = source.EmailNotifyMessage;
            ResidentialDetermination = source.ResidentialDetermination;
            SmartPostIndicia = source.SmartPostIndicia;
            SmartPostEndorsement = source.SmartPostEndorsement;
            SmartPostConfirmation = source.SmartPostConfirmation;
            SmartPostCustomerManifest = source.SmartPostCustomerManifest;
            SmartPostHubID = source.SmartPostHubID;
            EmailNotifyBroker = source.EmailNotifyBroker;
            DropoffType = source.DropoffType;
            OriginResidentialDetermination = source.OriginResidentialDetermination;
            PayorTransportName = source.PayorTransportName;
            ReturnType = source.ReturnType;
            RmaNumber = source.RmaNumber;
            RmaReason = source.RmaReason;
            ReturnSaturdayPickup = source.ReturnSaturdayPickup;
            ReturnsClearance = source.ReturnsClearance;
            ReferenceFIMS = source.ReferenceFIMS;
            ThirdPartyConsignee = source.ThirdPartyConsignee;
            
            ShippingProfile = (IShippingProfileEntity) source.ShippingProfile?.AsReadOnly(objectMap);
            
            
            Packages = source.Packages?.Select(x => x.AsReadOnly(objectMap)).OfType<IFedExProfilePackageEntity>().ToReadOnly() ??
                Enumerable.Empty<IFedExProfilePackageEntity>();

            CopyCustomFedExProfileData(source);
        }

        
        /// <summary> The ShippingProfileID property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ShippingProfileID { get; }
        /// <summary> The FedExAccountID property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."FedExAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> FedExAccountID { get; }
        /// <summary> The Service property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> Service { get; }
        /// <summary> The Signature property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."Signature"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> Signature { get; }
        /// <summary> The PackagingType property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."PackagingType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> PackagingType { get; }
        /// <summary> The NonStandardContainer property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."NonStandardContainer"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> NonStandardContainer { get; }
        /// <summary> The ReferenceCustomer property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."ReferenceCustomer"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ReferenceCustomer { get; }
        /// <summary> The ReferenceInvoice property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."ReferenceInvoice"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ReferenceInvoice { get; }
        /// <summary> The ReferencePO property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."ReferencePO"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ReferencePO { get; }
        /// <summary> The ReferenceShipmentIntegrity property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."ReferenceShipmentIntegrity"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ReferenceShipmentIntegrity { get; }
        /// <summary> The PayorTransportType property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."PayorTransportType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> PayorTransportType { get; }
        /// <summary> The PayorTransportAccount property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."PayorTransportAccount"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 12<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String PayorTransportAccount { get; }
        /// <summary> The PayorDutiesType property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."PayorDutiesType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> PayorDutiesType { get; }
        /// <summary> The PayorDutiesAccount property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."PayorDutiesAccount"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 12<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String PayorDutiesAccount { get; }
        /// <summary> The SaturdayDelivery property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."SaturdayDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> SaturdayDelivery { get; }
        /// <summary> The EmailNotifySender property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."EmailNotifySender"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> EmailNotifySender { get; }
        /// <summary> The EmailNotifyRecipient property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."EmailNotifyRecipient"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> EmailNotifyRecipient { get; }
        /// <summary> The EmailNotifyOther property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."EmailNotifyOther"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> EmailNotifyOther { get; }
        /// <summary> The EmailNotifyOtherAddress property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."EmailNotifyOtherAddress"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String EmailNotifyOtherAddress { get; }
        /// <summary> The EmailNotifyMessage property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."EmailNotifyMessage"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 120<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String EmailNotifyMessage { get; }
        /// <summary> The ResidentialDetermination property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."ResidentialDetermination"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> ResidentialDetermination { get; }
        /// <summary> The SmartPostIndicia property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."SmartPostIndicia"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> SmartPostIndicia { get; }
        /// <summary> The SmartPostEndorsement property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."SmartPostEndorsement"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> SmartPostEndorsement { get; }
        /// <summary> The SmartPostConfirmation property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."SmartPostConfirmation"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> SmartPostConfirmation { get; }
        /// <summary> The SmartPostCustomerManifest property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."SmartPostCustomerManifest"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String SmartPostCustomerManifest { get; }
        /// <summary> The SmartPostHubID property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."SmartPostHubID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String SmartPostHubID { get; }
        /// <summary> The EmailNotifyBroker property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."EmailNotifyBroker"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> EmailNotifyBroker { get; }
        /// <summary> The DropoffType property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."DropoffType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> DropoffType { get; }
        /// <summary> The OriginResidentialDetermination property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."OriginResidentialDetermination"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> OriginResidentialDetermination { get; }
        /// <summary> The PayorTransportName property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."PayorTransportName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String PayorTransportName { get; }
        /// <summary> The ReturnType property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."ReturnType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> ReturnType { get; }
        /// <summary> The RmaNumber property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."RmaNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String RmaNumber { get; }
        /// <summary> The RmaReason property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."RmaReason"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String RmaReason { get; }
        /// <summary> The ReturnSaturdayPickup property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."ReturnSaturdayPickup"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> ReturnSaturdayPickup { get; }
        /// <summary> The ReturnsClearance property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."ReturnsClearance"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> ReturnsClearance { get; }
        /// <summary> The ReferenceFIMS property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."ReferenceFIMS"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ReferenceFIMS { get; }
        /// <summary> The ThirdPartyConsignee property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."ThirdPartyConsignee"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> ThirdPartyConsignee { get; }
        
        public IShippingProfileEntity ShippingProfile { get; }
        
        
        
        public IEnumerable<IFedExProfilePackageEntity> Packages { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFedExProfileEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFedExProfileEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomFedExProfileData(IFedExProfileEntity source);
    }
}
