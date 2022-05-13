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
    /// Entity interface which represents the entity 'DhlEcommerceAccount'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IDhlEcommerceAccountEntity
    {
        
        /// <summary> The DhlEcommerceAccountID property of the Entity DhlEcommerceAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceAccount"."DhlEcommerceAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 DhlEcommerceAccountID { get; }
        /// <summary> The RowVersion property of the Entity DhlEcommerceAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceAccount"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The ShipEngineCarrierId property of the Entity DhlEcommerceAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceAccount"."ShipEngineCarrierId"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipEngineCarrierId { get; }
        /// <summary> The ClientId property of the Entity DhlEcommerceAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceAccount"."ClientId"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ClientId { get; }
        /// <summary> The ApiSecret property of the Entity DhlEcommerceAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceAccount"."ApiSecret"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 400<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ApiSecret { get; }
        /// <summary> The PickupNumber property of the Entity DhlEcommerceAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceAccount"."PickupNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PickupNumber { get; }
        /// <summary> The DistributionCenter property of the Entity DhlEcommerceAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceAccount"."DistributionCenter"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 12<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String DistributionCenter { get; }
        /// <summary> The SoldTo property of the Entity DhlEcommerceAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceAccount"."SoldTo"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String SoldTo { get; }
        /// <summary> The Description property of the Entity DhlEcommerceAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceAccount"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Description { get; }
        /// <summary> The FirstName property of the Entity DhlEcommerceAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceAccount"."FirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String FirstName { get; }
        /// <summary> The MiddleName property of the Entity DhlEcommerceAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceAccount"."MiddleName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String MiddleName { get; }
        /// <summary> The LastName property of the Entity DhlEcommerceAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceAccount"."LastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String LastName { get; }
        /// <summary> The Company property of the Entity DhlEcommerceAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceAccount"."Company"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Company { get; }
        /// <summary> The Street1 property of the Entity DhlEcommerceAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceAccount"."Street1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Street1 { get; }
        /// <summary> The City property of the Entity DhlEcommerceAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceAccount"."City"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String City { get; }
        /// <summary> The StateProvCode property of the Entity DhlEcommerceAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceAccount"."StateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String StateProvCode { get; }
        /// <summary> The PostalCode property of the Entity DhlEcommerceAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceAccount"."PostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PostalCode { get; }
        /// <summary> The CountryCode property of the Entity DhlEcommerceAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceAccount"."CountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String CountryCode { get; }
        /// <summary> The Phone property of the Entity DhlEcommerceAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceAccount"."Phone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 26<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Phone { get; }
        /// <summary> The Email property of the Entity DhlEcommerceAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceAccount"."Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Email { get; }
        /// <summary> The CreatedDate property of the Entity DhlEcommerceAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceAccount"."CreatedDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime CreatedDate { get; }
        /// <summary> The Street2 property of the Entity DhlEcommerceAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceAccount"."Street2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Street2 { get; }
        /// <summary> The Street3 property of the Entity DhlEcommerceAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceAccount"."Street3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Street3 { get; }
        /// <summary> The AncillaryEndorsement property of the Entity DhlEcommerceAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceAccount"."AncillaryEndorsement"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 AncillaryEndorsement { get; }
        
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IDhlEcommerceAccountEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IDhlEcommerceAccountEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'DhlEcommerceAccount'. <br/><br/>
    /// 
    /// </summary>
    public partial class DhlEcommerceAccountEntity : IDhlEcommerceAccountEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDhlEcommerceAccountEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IDhlEcommerceAccountEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IDhlEcommerceAccountEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyDhlEcommerceAccountEntity(this, objectMap);
        }

        
    }
}
