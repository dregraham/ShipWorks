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
    /// Read-only representation of the entity 'EndiciaAccount'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyEndiciaAccountEntity : IEndiciaAccountEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyEndiciaAccountEntity(IEndiciaAccountEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            EndiciaAccountID = source.EndiciaAccountID;
            EndiciaReseller = source.EndiciaReseller;
            AccountNumber = source.AccountNumber;
            SignupConfirmation = source.SignupConfirmation;
            WebPassword = source.WebPassword;
            ApiInitialPassword = source.ApiInitialPassword;
            ApiUserPassword = source.ApiUserPassword;
            AccountType = source.AccountType;
            TestAccount = source.TestAccount;
            CreatedByShipWorks = source.CreatedByShipWorks;
            Description = source.Description;
            FirstName = source.FirstName;
            LastName = source.LastName;
            Company = source.Company;
            Street1 = source.Street1;
            Street2 = source.Street2;
            Street3 = source.Street3;
            City = source.City;
            StateProvCode = source.StateProvCode;
            PostalCode = source.PostalCode;
            CountryCode = source.CountryCode;
            Phone = source.Phone;
            Fax = source.Fax;
            Email = source.Email;
            MailingPostalCode = source.MailingPostalCode;
            ScanFormAddressSource = source.ScanFormAddressSource;
            
            
            

            CopyCustomEndiciaAccountData(source);
        }

        
        /// <summary> The EndiciaAccountID property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."EndiciaAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 EndiciaAccountID { get; }
        /// <summary> The EndiciaReseller property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."EndiciaReseller"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 EndiciaReseller { get; }
        /// <summary> The AccountNumber property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."AccountNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String AccountNumber { get; }
        /// <summary> The SignupConfirmation property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."SignupConfirmation"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SignupConfirmation { get; }
        /// <summary> The WebPassword property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."WebPassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 250<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String WebPassword { get; }
        /// <summary> The ApiInitialPassword property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."ApiInitialPassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 250<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ApiInitialPassword { get; }
        /// <summary> The ApiUserPassword property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."ApiUserPassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 250<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ApiUserPassword { get; }
        /// <summary> The AccountType property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."AccountType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 AccountType { get; }
        /// <summary> The TestAccount property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."TestAccount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean TestAccount { get; }
        /// <summary> The CreatedByShipWorks property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."CreatedByShipWorks"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean CreatedByShipWorks { get; }
        /// <summary> The Description property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Description { get; }
        /// <summary> The FirstName property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."FirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FirstName { get; }
        /// <summary> The LastName property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."LastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String LastName { get; }
        /// <summary> The Company property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."Company"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Company { get; }
        /// <summary> The Street1 property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."Street1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Street1 { get; }
        /// <summary> The Street2 property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."Street2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Street2 { get; }
        /// <summary> The Street3 property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."Street3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Street3 { get; }
        /// <summary> The City property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."City"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String City { get; }
        /// <summary> The StateProvCode property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."StateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String StateProvCode { get; }
        /// <summary> The PostalCode property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."PostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PostalCode { get; }
        /// <summary> The CountryCode property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."CountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CountryCode { get; }
        /// <summary> The Phone property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."Phone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Phone { get; }
        /// <summary> The Fax property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."Fax"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Fax { get; }
        /// <summary> The Email property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Email { get; }
        /// <summary> The MailingPostalCode property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."MailingPostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String MailingPostalCode { get; }
        /// <summary> The ScanFormAddressSource property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."ScanFormAddressSource"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ScanFormAddressSource { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEndiciaAccountEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEndiciaAccountEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomEndiciaAccountData(IEndiciaAccountEntity source);
    }
}
