﻿///////////////////////////////////////////////////////////////
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
    /// Entity interface which represents the entity 'ShippingOrigin'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IShippingOriginEntity
    {
        
        /// <summary> The ShippingOriginID property of the Entity ShippingOrigin<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingOrigin"."ShippingOriginID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ShippingOriginID { get; }
        /// <summary> The RowVersion property of the Entity ShippingOrigin<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingOrigin"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The Description property of the Entity ShippingOrigin<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingOrigin"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Description { get; }
        /// <summary> The FirstName property of the Entity ShippingOrigin<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingOrigin"."FirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String FirstName { get; }
        /// <summary> The MiddleName property of the Entity ShippingOrigin<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingOrigin"."MiddleName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String MiddleName { get; }
        /// <summary> The LastName property of the Entity ShippingOrigin<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingOrigin"."LastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String LastName { get; }
        /// <summary> The Company property of the Entity ShippingOrigin<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingOrigin"."Company"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Company { get; }
        /// <summary> The Street1 property of the Entity ShippingOrigin<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingOrigin"."Street1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Street1 { get; }
        /// <summary> The Street2 property of the Entity ShippingOrigin<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingOrigin"."Street2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Street2 { get; }
        /// <summary> The Street3 property of the Entity ShippingOrigin<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingOrigin"."Street3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Street3 { get; }
        /// <summary> The City property of the Entity ShippingOrigin<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingOrigin"."City"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String City { get; }
        /// <summary> The StateProvCode property of the Entity ShippingOrigin<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingOrigin"."StateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String StateProvCode { get; }
        /// <summary> The PostalCode property of the Entity ShippingOrigin<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingOrigin"."PostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PostalCode { get; }
        /// <summary> The CountryCode property of the Entity ShippingOrigin<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingOrigin"."CountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String CountryCode { get; }
        /// <summary> The Phone property of the Entity ShippingOrigin<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingOrigin"."Phone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Phone { get; }
        /// <summary> The Fax property of the Entity ShippingOrigin<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingOrigin"."Fax"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Fax { get; }
        /// <summary> The Email property of the Entity ShippingOrigin<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingOrigin"."Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Email { get; }
        /// <summary> The Website property of the Entity ShippingOrigin<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingOrigin"."Website"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Website { get; }
        
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShippingOriginEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShippingOriginEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ShippingOrigin'. <br/><br/>
    /// 
    /// </summary>
    public partial class ShippingOriginEntity : IShippingOriginEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShippingOriginEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IShippingOriginEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IShippingOriginEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyShippingOriginEntity(this, objectMap);
        }

        
    }
}
