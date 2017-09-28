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
    /// Entity interface which represents the entity 'ValidatedAddress'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IValidatedAddressEntity
    {
        
        /// <summary> The ValidatedAddressID property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."ValidatedAddressID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ValidatedAddressID { get; }
        /// <summary> The ConsumerID property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."ConsumerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ConsumerID { get; }
        /// <summary> The AddressPrefix property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."AddressPrefix"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AddressPrefix { get; }
        /// <summary> The IsOriginal property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."IsOriginal"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean IsOriginal { get; }
        /// <summary> The Street1 property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."Street1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Street1 { get; }
        /// <summary> The Street2 property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."Street2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Street2 { get; }
        /// <summary> The Street3 property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."Street3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Street3 { get; }
        /// <summary> The City property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."City"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String City { get; }
        /// <summary> The StateProvCode property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."StateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String StateProvCode { get; }
        /// <summary> The PostalCode property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."PostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PostalCode { get; }
        /// <summary> The CountryCode property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."CountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String CountryCode { get; }
        /// <summary> The ResidentialStatus property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."ResidentialStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ResidentialStatus { get; }
        /// <summary> The POBox property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."POBox"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 POBox { get; }
        /// <summary> The USTerritory property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."USTerritory"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 USTerritory { get; }
        /// <summary> The MilitaryAddress property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."MilitaryAddress"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 MilitaryAddress { get; }
        
        
        IOrderEntity Order { get; }
        IShipmentEntity Shipment { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IValidatedAddressEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IValidatedAddressEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ValidatedAddress'. <br/><br/>
    /// 
    /// </summary>
    public partial class ValidatedAddressEntity : IValidatedAddressEntity
    {
        
        IOrderEntity IValidatedAddressEntity.Order => Order;
        IShipmentEntity IValidatedAddressEntity.Shipment => Shipment;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IValidatedAddressEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IValidatedAddressEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IValidatedAddressEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyValidatedAddressEntity(this, objectMap);
        }

        
    }
}
