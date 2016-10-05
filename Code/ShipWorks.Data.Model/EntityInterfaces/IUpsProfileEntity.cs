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
    /// Entity interface which represents the entity 'UpsProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IUpsProfileEntity
    {
        
        /// <summary> The ShippingProfileID property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShippingProfileID { get; }
        /// <summary> The UpsAccountID property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."UpsAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> UpsAccountID { get; }
        /// <summary> The Service property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> Service { get; }
        /// <summary> The SaturdayDelivery property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."SaturdayDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> SaturdayDelivery { get; }
        /// <summary> The ResidentialDetermination property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."ResidentialDetermination"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> ResidentialDetermination { get; }
        /// <summary> The DeliveryConfirmation property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."DeliveryConfirmation"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> DeliveryConfirmation { get; }
        /// <summary> The ReferenceNumber property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."ReferenceNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ReferenceNumber { get; }
        /// <summary> The ReferenceNumber2 property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."ReferenceNumber2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ReferenceNumber2 { get; }
        /// <summary> The PayorType property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."PayorType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> PayorType { get; }
        /// <summary> The PayorAccount property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."PayorAccount"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String PayorAccount { get; }
        /// <summary> The PayorPostalCode property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."PayorPostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String PayorPostalCode { get; }
        /// <summary> The PayorCountryCode property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."PayorCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String PayorCountryCode { get; }
        /// <summary> The EmailNotifySender property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."EmailNotifySender"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> EmailNotifySender { get; }
        /// <summary> The EmailNotifyRecipient property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."EmailNotifyRecipient"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> EmailNotifyRecipient { get; }
        /// <summary> The EmailNotifyOther property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."EmailNotifyOther"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> EmailNotifyOther { get; }
        /// <summary> The EmailNotifyOtherAddress property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."EmailNotifyOtherAddress"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String EmailNotifyOtherAddress { get; }
        /// <summary> The EmailNotifyFrom property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."EmailNotifyFrom"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String EmailNotifyFrom { get; }
        /// <summary> The EmailNotifySubject property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."EmailNotifySubject"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> EmailNotifySubject { get; }
        /// <summary> The EmailNotifyMessage property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."EmailNotifyMessage"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 120<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String EmailNotifyMessage { get; }
        /// <summary> The ReturnService property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."ReturnService"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> ReturnService { get; }
        /// <summary> The ReturnUndeliverableEmail property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."ReturnUndeliverableEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ReturnUndeliverableEmail { get; }
        /// <summary> The ReturnContents property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."ReturnContents"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ReturnContents { get; }
        /// <summary> The Endorsement property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."Endorsement"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> Endorsement { get; }
        /// <summary> The Subclassification property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."Subclassification"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> Subclassification { get; }
        /// <summary> The PaperlessAdditionalDocumentation property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."PaperlessAdditionalDocumentation"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> PaperlessAdditionalDocumentation { get; }
        /// <summary> The ShipperRelease property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."ShipperRelease"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> ShipperRelease { get; }
        /// <summary> The CarbonNeutral property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."CarbonNeutral"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> CarbonNeutral { get; }
        /// <summary> The CommercialPaperlessInvoice property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."CommercialPaperlessInvoice"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> CommercialPaperlessInvoice { get; }
        /// <summary> The CostCenter property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."CostCenter"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String CostCenter { get; }
        /// <summary> The IrregularIndicator property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."IrregularIndicator"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> IrregularIndicator { get; }
        /// <summary> The Cn22Number property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."Cn22Number"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String Cn22Number { get; }
        /// <summary> The ShipmentChargeType property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."ShipmentChargeType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> ShipmentChargeType { get; }
        /// <summary> The ShipmentChargeAccount property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."ShipmentChargeAccount"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ShipmentChargeAccount { get; }
        /// <summary> The ShipmentChargePostalCode property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."ShipmentChargePostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ShipmentChargePostalCode { get; }
        /// <summary> The ShipmentChargeCountryCode property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."ShipmentChargeCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ShipmentChargeCountryCode { get; }
        /// <summary> The UspsPackageID property of the Entity UpsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfile"."UspsPackageID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String UspsPackageID { get; }
        
        IShippingProfileEntity ShippingProfile { get; }
        
        
        IEnumerable<IUpsProfilePackageEntity> Packages { get; }

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUpsProfileEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUpsProfileEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'UpsProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial class UpsProfileEntity : IUpsProfileEntity
    {
        IShippingProfileEntity IUpsProfileEntity.ShippingProfile => ShippingProfile;
        
        
        IEnumerable<IUpsProfilePackageEntity> IUpsProfileEntity.Packages => Packages;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsProfileEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IUpsProfileEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IUpsProfileEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyUpsProfileEntity(this, objectMap);
        }
    }
}
