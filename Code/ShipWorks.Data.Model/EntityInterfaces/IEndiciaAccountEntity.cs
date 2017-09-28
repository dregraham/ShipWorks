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
    /// Entity interface which represents the entity 'EndiciaAccount'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IEndiciaAccountEntity
    {
        
        /// <summary> The EndiciaAccountID property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."EndiciaAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 EndiciaAccountID { get; }
        /// <summary> The EndiciaReseller property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."EndiciaReseller"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 EndiciaReseller { get; }
        /// <summary> The AccountNumber property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."AccountNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String AccountNumber { get; }
        /// <summary> The SignupConfirmation property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."SignupConfirmation"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String SignupConfirmation { get; }
        /// <summary> The WebPassword property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."WebPassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 250<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String WebPassword { get; }
        /// <summary> The ApiInitialPassword property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."ApiInitialPassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 250<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ApiInitialPassword { get; }
        /// <summary> The ApiUserPassword property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."ApiUserPassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 250<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ApiUserPassword { get; }
        /// <summary> The AccountType property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."AccountType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 AccountType { get; }
        /// <summary> The TestAccount property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."TestAccount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean TestAccount { get; }
        /// <summary> The CreatedByShipWorks property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."CreatedByShipWorks"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean CreatedByShipWorks { get; }
        /// <summary> The Description property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Description { get; }
        /// <summary> The FirstName property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."FirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String FirstName { get; }
        /// <summary> The LastName property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."LastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String LastName { get; }
        /// <summary> The Company property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."Company"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Company { get; }
        /// <summary> The Street1 property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."Street1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Street1 { get; }
        /// <summary> The Street2 property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."Street2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Street2 { get; }
        /// <summary> The Street3 property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."Street3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Street3 { get; }
        /// <summary> The City property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."City"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String City { get; }
        /// <summary> The StateProvCode property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."StateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String StateProvCode { get; }
        /// <summary> The PostalCode property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."PostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PostalCode { get; }
        /// <summary> The CountryCode property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."CountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String CountryCode { get; }
        /// <summary> The Phone property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."Phone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Phone { get; }
        /// <summary> The Fax property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."Fax"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Fax { get; }
        /// <summary> The Email property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Email { get; }
        /// <summary> The MailingPostalCode property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."MailingPostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String MailingPostalCode { get; }
        /// <summary> The ScanFormAddressSource property of the Entity EndiciaAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaAccount"."ScanFormAddressSource"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ScanFormAddressSource { get; }
        
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IEndiciaAccountEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IEndiciaAccountEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'EndiciaAccount'. <br/><br/>
    /// 
    /// </summary>
    public partial class EndiciaAccountEntity : IEndiciaAccountEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEndiciaAccountEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IEndiciaAccountEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IEndiciaAccountEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyEndiciaAccountEntity(this, objectMap);
        }

        
    }
}
