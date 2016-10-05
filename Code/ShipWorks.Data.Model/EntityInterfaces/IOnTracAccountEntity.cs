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
    /// Entity interface which represents the entity 'OnTracAccount'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IOnTracAccountEntity
    {
        
        /// <summary> The OnTracAccountID property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."OnTracAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 OnTracAccountID { get; }
        /// <summary> The RowVersion property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The AccountNumber property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."AccountNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 AccountNumber { get; }
        /// <summary> The Password property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."Password"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Password { get; }
        /// <summary> The Description property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Description { get; }
        /// <summary> The FirstName property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."FirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String FirstName { get; }
        /// <summary> The MiddleName property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."MiddleName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String MiddleName { get; }
        /// <summary> The LastName property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."LastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String LastName { get; }
        /// <summary> The Company property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."Company"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Company { get; }
        /// <summary> The Street1 property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."Street1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 43<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Street1 { get; }
        /// <summary> The City property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."City"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String City { get; }
        /// <summary> The StateProvCode property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."StateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String StateProvCode { get; }
        /// <summary> The PostalCode property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."PostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PostalCode { get; }
        /// <summary> The CountryCode property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."CountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String CountryCode { get; }
        /// <summary> The Email property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Email { get; }
        /// <summary> The Phone property of the Entity OnTracAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracAccount"."Phone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 15<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Phone { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOnTracAccountEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOnTracAccountEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'OnTracAccount'. <br/><br/>
    /// 
    /// </summary>
    public partial class OnTracAccountEntity : IOnTracAccountEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOnTracAccountEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IOnTracAccountEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IOnTracAccountEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyOnTracAccountEntity(this, objectMap);
        }
    }
}
