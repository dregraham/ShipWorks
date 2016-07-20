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
    /// Read-only representation of the entity 'ValidatedAddress'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyValidatedAddressEntity : IValidatedAddressEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyValidatedAddressEntity(IValidatedAddressEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ValidatedAddressID = source.ValidatedAddressID;
            ConsumerID = source.ConsumerID;
            AddressPrefix = source.AddressPrefix;
            IsOriginal = source.IsOriginal;
            Street1 = source.Street1;
            Street2 = source.Street2;
            Street3 = source.Street3;
            City = source.City;
            StateProvCode = source.StateProvCode;
            PostalCode = source.PostalCode;
            CountryCode = source.CountryCode;
            ResidentialStatus = source.ResidentialStatus;
            POBox = source.POBox;
            USTerritory = source.USTerritory;
            MilitaryAddress = source.MilitaryAddress;
            
            
            Order = source.Order?.AsReadOnly(objectMap);
            Shipment = source.Shipment?.AsReadOnly(objectMap);
            

            CopyCustomValidatedAddressData(source);
        }

        
        /// <summary> The ValidatedAddressID property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."ValidatedAddressID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ValidatedAddressID { get; }
        /// <summary> The ConsumerID property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."ConsumerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ConsumerID { get; }
        /// <summary> The AddressPrefix property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."AddressPrefix"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String AddressPrefix { get; }
        /// <summary> The IsOriginal property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."IsOriginal"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean IsOriginal { get; }
        /// <summary> The Street1 property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."Street1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Street1 { get; }
        /// <summary> The Street2 property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."Street2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Street2 { get; }
        /// <summary> The Street3 property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."Street3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Street3 { get; }
        /// <summary> The City property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."City"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String City { get; }
        /// <summary> The StateProvCode property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."StateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String StateProvCode { get; }
        /// <summary> The PostalCode property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."PostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PostalCode { get; }
        /// <summary> The CountryCode property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."CountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CountryCode { get; }
        /// <summary> The ResidentialStatus property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."ResidentialStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ResidentialStatus { get; }
        /// <summary> The POBox property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."POBox"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 POBox { get; }
        /// <summary> The USTerritory property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."USTerritory"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 USTerritory { get; }
        /// <summary> The MilitaryAddress property of the Entity ValidatedAddress<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ValidatedAddress"."MilitaryAddress"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 MilitaryAddress { get; }
        
        
        public IOrderEntity Order { get; }
        
        public IShipmentEntity Shipment { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IValidatedAddressEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IValidatedAddressEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomValidatedAddressData(IValidatedAddressEntity source);
    }
}
