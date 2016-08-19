///////////////////////////////////////////////////////////////
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
    /// Entity interface which represents the entity 'OdbcStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IOdbcStoreEntity: IStoreEntity
    {
        
        /// <summary> The ImportConnectionString property of the Entity OdbcStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OdbcStore"."ImportConnectionString"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2048<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ImportConnectionString { get; }
        /// <summary> The ImportMap property of the Entity OdbcStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OdbcStore"."ImportMap"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ImportMap { get; }
        /// <summary> The ImportStrategy property of the Entity OdbcStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OdbcStore"."ImportStrategy"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ImportStrategy { get; }
        /// <summary> The ImportColumnSourceType property of the Entity OdbcStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OdbcStore"."ImportColumnSourceType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ImportColumnSourceType { get; }
        /// <summary> The ImportColumnSource property of the Entity OdbcStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OdbcStore"."ImportColumnSource"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2048<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ImportColumnSource { get; }
        /// <summary> The UploadStrategy property of the Entity OdbcStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OdbcStore"."UploadStrategy"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 UploadStrategy { get; }
        /// <summary> The UploadMap property of the Entity OdbcStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OdbcStore"."UploadMap"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String UploadMap { get; }
        /// <summary> The UploadColumnSourceType property of the Entity OdbcStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OdbcStore"."UploadColumnSourceType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 UploadColumnSourceType { get; }
        /// <summary> The UploadColumnSource property of the Entity OdbcStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OdbcStore"."UploadColumnSource"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2048<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String UploadColumnSource { get; }
        /// <summary> The UploadConnectionString property of the Entity OdbcStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OdbcStore"."UploadConnectionString"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2048<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String UploadConnectionString { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IOdbcStoreEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IOdbcStoreEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'OdbcStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class OdbcStoreEntity : IOdbcStoreEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IOdbcStoreEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IOdbcStoreEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IOdbcStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyOdbcStoreEntity(this, objectMap);
        }
    }
}
