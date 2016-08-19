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
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Read-only representation of the entity 'OdbcStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyOdbcStoreEntity : ReadOnlyStoreEntity, IOdbcStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyOdbcStoreEntity(IOdbcStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ImportConnectionString = source.ImportConnectionString;
            ImportMap = source.ImportMap;
            ImportStrategy = source.ImportStrategy;
            ImportColumnSourceType = source.ImportColumnSourceType;
            ImportColumnSource = source.ImportColumnSource;
            UploadStrategy = source.UploadStrategy;
            UploadMap = source.UploadMap;
            UploadColumnSourceType = source.UploadColumnSourceType;
            UploadColumnSource = source.UploadColumnSource;
            UploadConnectionString = source.UploadConnectionString;
            
            
            

            CopyCustomOdbcStoreData(source);
        }

        
        /// <summary> The ImportConnectionString property of the Entity OdbcStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OdbcStore"."ImportConnectionString"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2048<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ImportConnectionString { get; }
        /// <summary> The ImportMap property of the Entity OdbcStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OdbcStore"."ImportMap"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ImportMap { get; }
        /// <summary> The ImportStrategy property of the Entity OdbcStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OdbcStore"."ImportStrategy"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ImportStrategy { get; }
        /// <summary> The ImportColumnSourceType property of the Entity OdbcStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OdbcStore"."ImportColumnSourceType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ImportColumnSourceType { get; }
        /// <summary> The ImportColumnSource property of the Entity OdbcStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OdbcStore"."ImportColumnSource"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2048<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ImportColumnSource { get; }
        /// <summary> The UploadStrategy property of the Entity OdbcStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OdbcStore"."UploadStrategy"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 UploadStrategy { get; }
        /// <summary> The UploadMap property of the Entity OdbcStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OdbcStore"."UploadMap"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String UploadMap { get; }
        /// <summary> The UploadColumnSourceType property of the Entity OdbcStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OdbcStore"."UploadColumnSourceType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 UploadColumnSourceType { get; }
        /// <summary> The UploadColumnSource property of the Entity OdbcStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OdbcStore"."UploadColumnSource"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2048<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String UploadColumnSource { get; }
        /// <summary> The UploadConnectionString property of the Entity OdbcStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OdbcStore"."UploadConnectionString"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2048<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String UploadConnectionString { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IOdbcStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IOdbcStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomOdbcStoreData(IOdbcStoreEntity source);
    }
}
