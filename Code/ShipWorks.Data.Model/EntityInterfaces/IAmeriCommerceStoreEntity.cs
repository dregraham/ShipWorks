﻿///////////////////////////////////////////////////////////////
// This is generated code.
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
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
    /// Entity interface which represents the entity 'AmeriCommerceStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IAmeriCommerceStoreEntity: IStoreEntity
    {
        
        /// <summary> The Username property of the Entity AmeriCommerceStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmeriCommerceStore"."Username"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 70<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Username { get; }
        /// <summary> The Password property of the Entity AmeriCommerceStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmeriCommerceStore"."Password"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 70<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Password { get; }
        /// <summary> The StoreUrl property of the Entity AmeriCommerceStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmeriCommerceStore"."StoreUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 350<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String StoreUrl { get; }
        /// <summary> The StoreCode property of the Entity AmeriCommerceStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmeriCommerceStore"."StoreCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 StoreCode { get; }
        /// <summary> The StatusCodes property of the Entity AmeriCommerceStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmeriCommerceStore"."StatusCodes"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String StatusCodes { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IAmeriCommerceStoreEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IAmeriCommerceStoreEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'AmeriCommerceStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class AmeriCommerceStoreEntity : IAmeriCommerceStoreEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IAmeriCommerceStoreEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IAmeriCommerceStoreEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IAmeriCommerceStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyAmeriCommerceStoreEntity(this, objectMap);
        }
    }
}