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
    /// Entity interface which represents the entity 'UpsAccount'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IUpsAccountEntity
    {
        
        /// <summary> The UpsAccountID property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."UpsAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 UpsAccountID { get; }
        /// <summary> The RowVersion property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The Description property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Description { get; }
        /// <summary> The AccountNumber property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."AccountNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AccountNumber { get; }
        /// <summary> The UserID property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String UserID { get; }
        /// <summary> The Password property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."Password"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Password { get; }
        /// <summary> The RateType property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."RateType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 RateType { get; }
        /// <summary> The InvoiceAuth property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."InvoiceAuth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean InvoiceAuth { get; }
        /// <summary> The FirstName property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."FirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String FirstName { get; }
        /// <summary> The MiddleName property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."MiddleName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String MiddleName { get; }
        /// <summary> The LastName property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."LastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String LastName { get; }
        /// <summary> The Company property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."Company"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Company { get; }
        /// <summary> The Street1 property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."Street1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Street1 { get; }
        /// <summary> The Street2 property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."Street2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Street2 { get; }
        /// <summary> The Street3 property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."Street3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Street3 { get; }
        /// <summary> The City property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."City"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String City { get; }
        /// <summary> The StateProvCode property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."StateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String StateProvCode { get; }
        /// <summary> The PostalCode property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."PostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PostalCode { get; }
        /// <summary> The CountryCode property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."CountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String CountryCode { get; }
        /// <summary> The Phone property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."Phone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Phone { get; }
        /// <summary> The Email property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Email { get; }
        /// <summary> The Website property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."Website"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Website { get; }
        /// <summary> The PromoStatus property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."PromoStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): TinyInt, 3, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte PromoStatus { get; }
        /// <summary> The LocalRatingEnabled property of the Entity UpsAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsAccount"."LocalRatingEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean LocalRatingEnabled { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUpsAccountEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUpsAccountEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'UpsAccount'. <br/><br/>
    /// 
    /// </summary>
    public partial class UpsAccountEntity : IUpsAccountEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsAccountEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IUpsAccountEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IUpsAccountEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyUpsAccountEntity(this, objectMap);
        }
    }
}
