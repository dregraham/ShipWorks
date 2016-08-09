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
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'ShippingProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IShippingProfileEntity
    {
        
        /// <summary> The ShippingProfileID property of the Entity ShippingProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ShippingProfileID { get; }
        /// <summary> The RowVersion property of the Entity ShippingProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProfile"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The Name property of the Entity ShippingProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProfile"."Name"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Name { get; }
        /// <summary> The ShipmentType property of the Entity ShippingProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProfile"."ShipmentType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipmentType { get; }
        /// <summary> The ShipmentTypePrimary property of the Entity ShippingProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProfile"."ShipmentTypePrimary"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean ShipmentTypePrimary { get; }
        /// <summary> The OriginID property of the Entity ShippingProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProfile"."OriginID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> OriginID { get; }
        /// <summary> The Insurance property of the Entity ShippingProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProfile"."Insurance"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> Insurance { get; }
        /// <summary> The InsuranceInitialValueSource property of the Entity ShippingProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProfile"."InsuranceInitialValueSource"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> InsuranceInitialValueSource { get; }
        /// <summary> The InsuranceInitialValueAmount property of the Entity ShippingProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProfile"."InsuranceInitialValueAmount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Decimal> InsuranceInitialValueAmount { get; }
        /// <summary> The ReturnShipment property of the Entity ShippingProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProfile"."ReturnShipment"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> ReturnShipment { get; }
        /// <summary> The RequestedLabelFormat property of the Entity ShippingProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProfile"."RequestedLabelFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> RequestedLabelFormat { get; }
        
        IAmazonProfileEntity Amazon { get; }
        IBestRateProfileEntity BestRate { get; }
        IFedExProfileEntity FedEx { get; }
        IIParcelProfileEntity IParcel { get; }
        IOnTracProfileEntity OnTrac { get; }
        IOtherProfileEntity Other { get; }
        IPostalProfileEntity Postal { get; }
        IUpsProfileEntity Ups { get; }
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShippingProfileEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShippingProfileEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ShippingProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial class ShippingProfileEntity : IShippingProfileEntity
    {
        IAmazonProfileEntity IShippingProfileEntity.Amazon => Amazon;
        IBestRateProfileEntity IShippingProfileEntity.BestRate => BestRate;
        IFedExProfileEntity IShippingProfileEntity.FedEx => FedEx;
        IIParcelProfileEntity IShippingProfileEntity.IParcel => IParcel;
        IOnTracProfileEntity IShippingProfileEntity.OnTrac => OnTrac;
        IOtherProfileEntity IShippingProfileEntity.Other => Other;
        IPostalProfileEntity IShippingProfileEntity.Postal => Postal;
        IUpsProfileEntity IShippingProfileEntity.Ups => Ups;
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShippingProfileEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IShippingProfileEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IShippingProfileEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyShippingProfileEntity(this, objectMap);
        }
    }
}
