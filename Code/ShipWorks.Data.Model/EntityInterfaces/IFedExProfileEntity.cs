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
    /// Entity interface which represents the entity 'FedExProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IFedExProfileEntity
    {
        
        /// <summary> The ShippingProfileID property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShippingProfileID { get; }
        /// <summary> The FedExAccountID property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."FedExAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> FedExAccountID { get; }
        /// <summary> The Service property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> Service { get; }
        /// <summary> The Signature property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."Signature"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> Signature { get; }
        /// <summary> The PackagingType property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."PackagingType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> PackagingType { get; }
        /// <summary> The NonStandardContainer property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."NonStandardContainer"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> NonStandardContainer { get; }
        /// <summary> The ReferenceCustomer property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."ReferenceCustomer"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ReferenceCustomer { get; }
        /// <summary> The ReferenceInvoice property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."ReferenceInvoice"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ReferenceInvoice { get; }
        /// <summary> The ReferencePO property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."ReferencePO"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ReferencePO { get; }
        /// <summary> The ReferenceShipmentIntegrity property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."ReferenceShipmentIntegrity"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ReferenceShipmentIntegrity { get; }
        /// <summary> The PayorTransportType property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."PayorTransportType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> PayorTransportType { get; }
        /// <summary> The PayorTransportAccount property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."PayorTransportAccount"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 12<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String PayorTransportAccount { get; }
        /// <summary> The PayorDutiesType property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."PayorDutiesType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> PayorDutiesType { get; }
        /// <summary> The PayorDutiesAccount property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."PayorDutiesAccount"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 12<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String PayorDutiesAccount { get; }
        /// <summary> The SaturdayDelivery property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."SaturdayDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> SaturdayDelivery { get; }
        /// <summary> The EmailNotifySender property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."EmailNotifySender"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> EmailNotifySender { get; }
        /// <summary> The EmailNotifyRecipient property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."EmailNotifyRecipient"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> EmailNotifyRecipient { get; }
        /// <summary> The EmailNotifyOther property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."EmailNotifyOther"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> EmailNotifyOther { get; }
        /// <summary> The EmailNotifyOtherAddress property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."EmailNotifyOtherAddress"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String EmailNotifyOtherAddress { get; }
        /// <summary> The EmailNotifyMessage property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."EmailNotifyMessage"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 120<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String EmailNotifyMessage { get; }
        /// <summary> The ResidentialDetermination property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."ResidentialDetermination"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> ResidentialDetermination { get; }
        /// <summary> The SmartPostIndicia property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."SmartPostIndicia"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> SmartPostIndicia { get; }
        /// <summary> The SmartPostEndorsement property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."SmartPostEndorsement"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> SmartPostEndorsement { get; }
        /// <summary> The SmartPostConfirmation property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."SmartPostConfirmation"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> SmartPostConfirmation { get; }
        /// <summary> The SmartPostCustomerManifest property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."SmartPostCustomerManifest"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String SmartPostCustomerManifest { get; }
        /// <summary> The SmartPostHubID property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."SmartPostHubID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String SmartPostHubID { get; }
        /// <summary> The EmailNotifyBroker property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."EmailNotifyBroker"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> EmailNotifyBroker { get; }
        /// <summary> The DropoffType property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."DropoffType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> DropoffType { get; }
        /// <summary> The OriginResidentialDetermination property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."OriginResidentialDetermination"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> OriginResidentialDetermination { get; }
        /// <summary> The PayorTransportName property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."PayorTransportName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String PayorTransportName { get; }
        /// <summary> The ReturnType property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."ReturnType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> ReturnType { get; }
        /// <summary> The RmaNumber property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."RmaNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String RmaNumber { get; }
        /// <summary> The RmaReason property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."RmaReason"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String RmaReason { get; }
        /// <summary> The ReturnSaturdayPickup property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."ReturnSaturdayPickup"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> ReturnSaturdayPickup { get; }
        /// <summary> The ReturnsClearance property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."ReturnsClearance"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> ReturnsClearance { get; }
        /// <summary> The ReferenceFIMS property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."ReferenceFIMS"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ReferenceFIMS { get; }
        /// <summary> The ThirdPartyConsignee property of the Entity FedExProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExProfile"."ThirdPartyConsignee"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> ThirdPartyConsignee { get; }
        
        IShippingProfileEntity ShippingProfile { get; }
        
        
        IEnumerable<IFedExProfilePackageEntity> Packages { get; }

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IFedExProfileEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IFedExProfileEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'FedExProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial class FedExProfileEntity : IFedExProfileEntity
    {
        IShippingProfileEntity IFedExProfileEntity.ShippingProfile => ShippingProfile;
        
        
        IEnumerable<IFedExProfilePackageEntity> IFedExProfileEntity.Packages => Packages;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFedExProfileEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IFedExProfileEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IFedExProfileEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyFedExProfileEntity(this, objectMap);
        }

        
    }
}
