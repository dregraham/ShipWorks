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
    /// Read-only representation of the entity 'UpsAccount'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyUpsAccountEntity : IUpsAccountEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyUpsAccountEntity(IUpsAccountEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            UpsAccountID = source.UpsAccountID;
            RowVersion = source.RowVersion;
            Description = source.Description;
            AccountNumber = source.AccountNumber;
            UserID = source.UserID;
            Password = source.Password;
            RateType = source.RateType;
            InvoiceAuth = source.InvoiceAuth;
            FirstName = source.FirstName;
            MiddleName = source.MiddleName;
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
            Email = source.Email;
            Website = source.Website;
            PromoStatus = source.PromoStatus;
            
            
            

            CopyCustomUpsAccountData(source);
        }

        
        /// <summary> The UpsAccountID property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."UpsAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 UpsAccountID { get; }
        /// <summary> The RowVersion property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The Description property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Description { get; }
        /// <summary> The AccountNumber property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."AccountNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String AccountNumber { get; }
        /// <summary> The UserID property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String UserID { get; }
        /// <summary> The Password property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."Password"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Password { get; }
        /// <summary> The RateType property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."RateType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 RateType { get; }
        /// <summary> The InvoiceAuth property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."InvoiceAuth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean InvoiceAuth { get; }
        /// <summary> The FirstName property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."FirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FirstName { get; }
        /// <summary> The MiddleName property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."MiddleName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String MiddleName { get; }
        /// <summary> The LastName property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."LastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String LastName { get; }
        /// <summary> The Company property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."Company"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Company { get; }
        /// <summary> The Street1 property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."Street1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Street1 { get; }
        /// <summary> The Street2 property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."Street2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Street2 { get; }
        /// <summary> The Street3 property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."Street3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Street3 { get; }
        /// <summary> The City property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."City"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String City { get; }
        /// <summary> The StateProvCode property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."StateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String StateProvCode { get; }
        /// <summary> The PostalCode property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."PostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PostalCode { get; }
        /// <summary> The CountryCode property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."CountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CountryCode { get; }
        /// <summary> The Phone property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."Phone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Phone { get; }
        /// <summary> The Email property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Email { get; }
        /// <summary> The Website property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."Website"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Website { get; }
        /// <summary> The PromoStatus property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."PromoStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): TinyInt, 3, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte PromoStatus { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsAccountEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsAccountEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomUpsAccountData(IUpsAccountEntity source);
    }
}
