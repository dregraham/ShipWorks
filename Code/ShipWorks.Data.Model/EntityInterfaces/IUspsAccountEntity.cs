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
    /// Entity interface which represents the entity 'UspsAccount'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IUspsAccountEntity
    {
        
        /// <summary> The UspsAccountID property of the Entity UspsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsAccount"."UspsAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 UspsAccountID { get; }
        /// <summary> The RowVersion property of the Entity UspsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsAccount"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The Description property of the Entity UspsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsAccount"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Description { get; }
        /// <summary> The Username property of the Entity UspsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsAccount"."Username"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Username { get; }
        /// <summary> The Password property of the Entity UspsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsAccount"."Password"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Password { get; }
        /// <summary> The FirstName property of the Entity UspsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsAccount"."FirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String FirstName { get; }
        /// <summary> The MiddleName property of the Entity UspsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsAccount"."MiddleName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String MiddleName { get; }
        /// <summary> The LastName property of the Entity UspsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsAccount"."LastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String LastName { get; }
        /// <summary> The Company property of the Entity UspsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsAccount"."Company"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Company { get; }
        /// <summary> The Street1 property of the Entity UspsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsAccount"."Street1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Street1 { get; }
        /// <summary> The Street2 property of the Entity UspsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsAccount"."Street2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Street2 { get; }
        /// <summary> The Street3 property of the Entity UspsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsAccount"."Street3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Street3 { get; }
        /// <summary> The City property of the Entity UspsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsAccount"."City"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String City { get; }
        /// <summary> The StateProvCode property of the Entity UspsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsAccount"."StateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String StateProvCode { get; }
        /// <summary> The PostalCode property of the Entity UspsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsAccount"."PostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PostalCode { get; }
        /// <summary> The CountryCode property of the Entity UspsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsAccount"."CountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String CountryCode { get; }
        /// <summary> The Phone property of the Entity UspsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsAccount"."Phone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Phone { get; }
        /// <summary> The Email property of the Entity UspsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsAccount"."Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Email { get; }
        /// <summary> The Website property of the Entity UspsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsAccount"."Website"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Website { get; }
        /// <summary> The MailingPostalCode property of the Entity UspsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsAccount"."MailingPostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String MailingPostalCode { get; }
        /// <summary> The UspsReseller property of the Entity UspsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsAccount"."UspsReseller"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 UspsReseller { get; }
        /// <summary> The ContractType property of the Entity UspsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsAccount"."ContractType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ContractType { get; }
        /// <summary> The CreatedDate property of the Entity UspsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsAccount"."CreatedDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime CreatedDate { get; }
        /// <summary> The PendingInitialAccount property of the Entity UspsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsAccount"."PendingInitialAccount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 PendingInitialAccount { get; }
        /// <summary> The GlobalPostAvailability property of the Entity UspsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsAccount"."GlobalPostAvailability"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 GlobalPostAvailability { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUspsAccountEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUspsAccountEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'UspsAccount'. <br/><br/>
    /// 
    /// </summary>
    public partial class UspsAccountEntity : IUspsAccountEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUspsAccountEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IUspsAccountEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IUspsAccountEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyUspsAccountEntity(this, objectMap);
        }
    }
}
