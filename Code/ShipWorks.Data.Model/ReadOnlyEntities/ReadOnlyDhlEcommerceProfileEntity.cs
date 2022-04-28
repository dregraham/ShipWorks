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
    /// Read-only representation of the entity 'DhlEcommerceProfile'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyDhlEcommerceProfileEntity : IDhlEcommerceProfileEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyDhlEcommerceProfileEntity(IDhlEcommerceProfileEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShippingProfileID = source.ShippingProfileID;
            DhlEcommerceAccountID = source.DhlEcommerceAccountID;
            Service = source.Service;
            DeliveryDutyPaid = source.DeliveryDutyPaid;
            NonMachinable = source.NonMachinable;
            SaturdayDelivery = source.SaturdayDelivery;
            Contents = source.Contents;
            NonDelivery = source.NonDelivery;
            ResidentialDelivery = source.ResidentialDelivery;
            CustomsRecipientTin = source.CustomsRecipientTin;
            CustomsTaxIdType = source.CustomsTaxIdType;
            CustomsTinIssuingAuthority = source.CustomsTinIssuingAuthority;
            PackagingType = source.PackagingType;
            Reference1 = source.Reference1;
            AncillaryEndorsement = source.AncillaryEndorsement;
            
            ShippingProfile = (IShippingProfileEntity) source.ShippingProfile?.AsReadOnly(objectMap);
            
            

            CopyCustomDhlEcommerceProfileData(source);
        }

        
        /// <summary> The ShippingProfileID property of the Entity DhlEcommerceProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ShippingProfileID { get; }
        /// <summary> The DhlEcommerceAccountID property of the Entity DhlEcommerceProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceProfile"."DhlEcommerceAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> DhlEcommerceAccountID { get; }
        /// <summary> The Service property of the Entity DhlEcommerceProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceProfile"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> Service { get; }
        /// <summary> The DeliveryDutyPaid property of the Entity DhlEcommerceProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceProfile"."DeliveryDutyPaid"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> DeliveryDutyPaid { get; }
        /// <summary> The NonMachinable property of the Entity DhlEcommerceProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceProfile"."NonMachinable"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> NonMachinable { get; }
        /// <summary> The SaturdayDelivery property of the Entity DhlEcommerceProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceProfile"."SaturdayDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> SaturdayDelivery { get; }
        /// <summary> The Contents property of the Entity DhlEcommerceProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceProfile"."Contents"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> Contents { get; }
        /// <summary> The NonDelivery property of the Entity DhlEcommerceProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceProfile"."NonDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> NonDelivery { get; }
        /// <summary> The ResidentialDelivery property of the Entity DhlEcommerceProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceProfile"."ResidentialDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> ResidentialDelivery { get; }
        /// <summary> The CustomsRecipientTin property of the Entity DhlEcommerceProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceProfile"."CustomsRecipientTin"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String CustomsRecipientTin { get; }
        /// <summary> The CustomsTaxIdType property of the Entity DhlEcommerceProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceProfile"."CustomsTaxIdType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> CustomsTaxIdType { get; }
        /// <summary> The CustomsTinIssuingAuthority property of the Entity DhlEcommerceProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceProfile"."CustomsTinIssuingAuthority"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String CustomsTinIssuingAuthority { get; }
        /// <summary> The PackagingType property of the Entity DhlEcommerceProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceProfile"."PackagingType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> PackagingType { get; }
        /// <summary> The Reference1 property of the Entity DhlEcommerceProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceProfile"."Reference1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String Reference1 { get; }
        /// <summary> The AncillaryEndorsement property of the Entity DhlEcommerceProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceProfile"."AncillaryEndorsement"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String AncillaryEndorsement { get; }
        
        public IShippingProfileEntity ShippingProfile { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDhlEcommerceProfileEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDhlEcommerceProfileEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomDhlEcommerceProfileData(IDhlEcommerceProfileEntity source);
    }
}
