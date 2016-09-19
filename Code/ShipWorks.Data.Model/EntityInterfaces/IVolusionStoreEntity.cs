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
    /// Entity interface which represents the entity 'VolusionStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IVolusionStoreEntity: IStoreEntity
    {
        
        /// <summary> The StoreUrl property of the Entity VolusionStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "VolusionStore"."StoreUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String StoreUrl { get; }
        /// <summary> The WebUserName property of the Entity VolusionStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "VolusionStore"."WebUserName"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String WebUserName { get; }
        /// <summary> The WebPassword property of the Entity VolusionStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "VolusionStore"."WebPassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 70<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String WebPassword { get; }
        /// <summary> The ApiPassword property of the Entity VolusionStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "VolusionStore"."ApiPassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ApiPassword { get; }
        /// <summary> The PaymentMethods property of the Entity VolusionStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "VolusionStore"."PaymentMethods"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PaymentMethods { get; }
        /// <summary> The ShipmentMethods property of the Entity VolusionStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "VolusionStore"."ShipmentMethods"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipmentMethods { get; }
        /// <summary> The DownloadOrderStatuses property of the Entity VolusionStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "VolusionStore"."DownloadOrderStatuses"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String DownloadOrderStatuses { get; }
        /// <summary> The ServerTimeZone property of the Entity VolusionStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "VolusionStore"."ServerTimeZone"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ServerTimeZone { get; }
        /// <summary> The ServerTimeZoneDST property of the Entity VolusionStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "VolusionStore"."ServerTimeZoneDST"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean ServerTimeZoneDST { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IVolusionStoreEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IVolusionStoreEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'VolusionStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class VolusionStoreEntity : IVolusionStoreEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IVolusionStoreEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IVolusionStoreEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IVolusionStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyVolusionStoreEntity(this, objectMap);
        }
    }
}
