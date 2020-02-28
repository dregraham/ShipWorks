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
    /// Read-only representation of the entity 'AmazonSWAAccount'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyAmazonSWAAccountEntity : IAmazonSWAAccountEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyAmazonSWAAccountEntity(IAmazonSWAAccountEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            AmazonSWAAccountID = source.AmazonSWAAccountID;
            RowVersion = source.RowVersion;
            ShipEngineCarrierId = source.ShipEngineCarrierId;
            Description = source.Description;
            FirstName = source.FirstName;
            MiddleName = source.MiddleName;
            LastName = source.LastName;
            Company = source.Company;
            Street1 = source.Street1;
            City = source.City;
            StateProvCode = source.StateProvCode;
            PostalCode = source.PostalCode;
            CountryCode = source.CountryCode;
            Email = source.Email;
            Phone = source.Phone;
            
            
            

            CopyCustomAmazonSWAAccountData(source);
        }

        
        /// <summary> The AmazonSWAAccountID property of the Entity AmazonSWAAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAAccount"."AmazonSWAAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 AmazonSWAAccountID { get; }
        /// <summary> The RowVersion property of the Entity AmazonSWAAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAAccount"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The ShipEngineCarrierId property of the Entity AmazonSWAAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAAccount"."ShipEngineCarrierId"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipEngineCarrierId { get; }
        /// <summary> The Description property of the Entity AmazonSWAAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAAccount"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Description { get; }
        /// <summary> The FirstName property of the Entity AmazonSWAAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAAccount"."FirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FirstName { get; }
        /// <summary> The MiddleName property of the Entity AmazonSWAAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAAccount"."MiddleName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String MiddleName { get; }
        /// <summary> The LastName property of the Entity AmazonSWAAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAAccount"."LastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String LastName { get; }
        /// <summary> The Company property of the Entity AmazonSWAAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAAccount"."Company"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Company { get; }
        /// <summary> The Street1 property of the Entity AmazonSWAAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAAccount"."Street1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 43<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Street1 { get; }
        /// <summary> The City property of the Entity AmazonSWAAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAAccount"."City"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String City { get; }
        /// <summary> The StateProvCode property of the Entity AmazonSWAAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAAccount"."StateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String StateProvCode { get; }
        /// <summary> The PostalCode property of the Entity AmazonSWAAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAAccount"."PostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PostalCode { get; }
        /// <summary> The CountryCode property of the Entity AmazonSWAAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAAccount"."CountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CountryCode { get; }
        /// <summary> The Email property of the Entity AmazonSWAAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAAccount"."Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Email { get; }
        /// <summary> The Phone property of the Entity AmazonSWAAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSWAAccount"."Phone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 15<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Phone { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAmazonSWAAccountEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAmazonSWAAccountEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomAmazonSWAAccountData(IAmazonSWAAccountEntity source);
    }
}
