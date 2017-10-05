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
    /// Read-only representation of the entity 'DhlExpressAccount'. <br/><br/>
    ///
    /// </summary>
    [Serializable]
    public partial class ReadOnlyDhlExpressAccountEntity : IDhlExpressAccountEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyDhlExpressAccountEntity(IDhlExpressAccountEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }

            DhlExpressAccountID = source.DhlExpressAccountID;
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

            CopyCustomDhlExpressAccountData(source);
        }

        /// <summary> The DhlExpressAccountID property of the Entity DhlExpressAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressAccount"."DhlExpressAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 DhlExpressAccountID { get; }
        /// <summary> The RowVersion property of the Entity DhlExpressAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressAccount"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The AccountNumber property of the Entity DhlExpressAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressAccount"."AccountNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 AccountNumber { get; }
        /// <summary> The ShipEngineCarrierId property of the Entity DhlExpressAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressAccount"."ShipEngineCarrierId"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 12<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipEngineCarrierId { get; }
        /// <summary> The Description property of the Entity DhlExpressAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressAccount"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Description { get; }
        /// <summary> The FirstName property of the Entity DhlExpressAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressAccount"."FirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FirstName { get; }
        /// <summary> The MiddleName property of the Entity DhlExpressAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressAccount"."MiddleName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String MiddleName { get; }
        /// <summary> The LastName property of the Entity DhlExpressAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressAccount"."LastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String LastName { get; }
        /// <summary> The Company property of the Entity DhlExpressAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressAccount"."Company"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Company { get; }
        /// <summary> The Street1 property of the Entity DhlExpressAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressAccount"."Street1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 43<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Street1 { get; }
        /// <summary> The City property of the Entity DhlExpressAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressAccount"."City"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String City { get; }
        /// <summary> The StateProvCode property of the Entity DhlExpressAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressAccount"."StateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String StateProvCode { get; }
        /// <summary> The PostalCode property of the Entity DhlExpressAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressAccount"."PostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PostalCode { get; }
        /// <summary> The CountryCode property of the Entity DhlExpressAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressAccount"."CountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CountryCode { get; }
        /// <summary> The Email property of the Entity DhlExpressAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressAccount"."Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Email { get; }
        /// <summary> The Phone property of the Entity DhlExpressAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressAccount"."Phone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 15<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Phone { get; }

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDhlExpressAccountEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDhlExpressAccountEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomDhlExpressAccountData(IDhlExpressAccountEntity source);
    }
}
