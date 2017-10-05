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
    /// Read-only representation of the entity 'OnTracAccount'. <br/><br/>
    ///
    /// </summary>
    [Serializable]
    public partial class ReadOnlyOnTracAccountEntity : IOnTracAccountEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyOnTracAccountEntity(IOnTracAccountEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }

            OnTracAccountID = source.OnTracAccountID;
            RowVersion = source.RowVersion;
            AccountNumber = source.AccountNumber;
            Password = source.Password;
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

            CopyCustomOnTracAccountData(source);
        }

        /// <summary> The OnTracAccountID property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."OnTracAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 OnTracAccountID { get; }
        /// <summary> The RowVersion property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The AccountNumber property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."AccountNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 AccountNumber { get; }
        /// <summary> The Password property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."Password"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Password { get; }
        /// <summary> The Description property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Description { get; }
        /// <summary> The FirstName property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."FirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FirstName { get; }
        /// <summary> The MiddleName property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."MiddleName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String MiddleName { get; }
        /// <summary> The LastName property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."LastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String LastName { get; }
        /// <summary> The Company property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."Company"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Company { get; }
        /// <summary> The Street1 property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."Street1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 43<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Street1 { get; }
        /// <summary> The City property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."City"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String City { get; }
        /// <summary> The StateProvCode property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."StateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String StateProvCode { get; }
        /// <summary> The PostalCode property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."PostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PostalCode { get; }
        /// <summary> The CountryCode property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."CountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CountryCode { get; }
        /// <summary> The Email property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Email { get; }
        /// <summary> The Phone property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."Phone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 15<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Phone { get; }

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOnTracAccountEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOnTracAccountEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomOnTracAccountData(IOnTracAccountEntity source);
    }
}
