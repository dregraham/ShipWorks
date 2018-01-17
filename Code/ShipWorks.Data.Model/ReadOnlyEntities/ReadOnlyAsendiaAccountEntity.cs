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
    /// Read-only representation of the entity 'AsendiaAccount'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyAsendiaAccountEntity : IAsendiaAccountEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyAsendiaAccountEntity(IAsendiaAccountEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            AsendiaAccountID = source.AsendiaAccountID;
            RowVersion = source.RowVersion;
            AccountNumber = source.AccountNumber;
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
            
            
            

            CopyCustomAsendiaAccountData(source);
        }

        
        /// <summary> The AsendiaAccountID property of the Entity AsendiaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaAccount"."AsendiaAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 AsendiaAccountID { get; }
        /// <summary> The RowVersion property of the Entity AsendiaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaAccount"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The AccountNumber property of the Entity AsendiaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaAccount"."AccountNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 AccountNumber { get; }
        /// <summary> The ShipEngineCarrierId property of the Entity AsendiaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaAccount"."ShipEngineCarrierId"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 12<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipEngineCarrierId { get; }
        /// <summary> The Description property of the Entity AsendiaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaAccount"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Description { get; }
        /// <summary> The FirstName property of the Entity AsendiaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaAccount"."FirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FirstName { get; }
        /// <summary> The MiddleName property of the Entity AsendiaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaAccount"."MiddleName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String MiddleName { get; }
        /// <summary> The LastName property of the Entity AsendiaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaAccount"."LastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String LastName { get; }
        /// <summary> The Company property of the Entity AsendiaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaAccount"."Company"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Company { get; }
        /// <summary> The Street1 property of the Entity AsendiaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaAccount"."Street1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 43<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Street1 { get; }
        /// <summary> The City property of the Entity AsendiaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaAccount"."City"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String City { get; }
        /// <summary> The StateProvCode property of the Entity AsendiaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaAccount"."StateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String StateProvCode { get; }
        /// <summary> The PostalCode property of the Entity AsendiaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaAccount"."PostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PostalCode { get; }
        /// <summary> The CountryCode property of the Entity AsendiaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaAccount"."CountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CountryCode { get; }
        /// <summary> The Email property of the Entity AsendiaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaAccount"."Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Email { get; }
        /// <summary> The Phone property of the Entity AsendiaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaAccount"."Phone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 15<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Phone { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAsendiaAccountEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAsendiaAccountEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomAsendiaAccountData(IAsendiaAccountEntity source);
    }
}
