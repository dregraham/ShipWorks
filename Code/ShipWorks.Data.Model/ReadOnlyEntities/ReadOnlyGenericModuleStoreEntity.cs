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
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Read-only representation of the entity 'GenericModuleStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyGenericModuleStoreEntity : ReadOnlyStoreEntity, IGenericModuleStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyGenericModuleStoreEntity(IGenericModuleStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ModuleUsername = source.ModuleUsername;
            ModulePassword = source.ModulePassword;
            ModuleUrl = source.ModuleUrl;
            ModuleVersion = source.ModuleVersion;
            ModulePlatform = source.ModulePlatform;
            ModuleDeveloper = source.ModuleDeveloper;
            ModuleOnlineStoreCode = source.ModuleOnlineStoreCode;
            ModuleStatusCodes = source.ModuleStatusCodes;
            ModuleDownloadPageSize = source.ModuleDownloadPageSize;
            ModuleRequestTimeout = source.ModuleRequestTimeout;
            ModuleDownloadStrategy = source.ModuleDownloadStrategy;
            ModuleOnlineStatusSupport = source.ModuleOnlineStatusSupport;
            ModuleOnlineStatusDataType = source.ModuleOnlineStatusDataType;
            ModuleOnlineCustomerSupport = source.ModuleOnlineCustomerSupport;
            ModuleOnlineCustomerDataType = source.ModuleOnlineCustomerDataType;
            ModuleOnlineShipmentDetails = source.ModuleOnlineShipmentDetails;
            ModuleHttpExpect100Continue = source.ModuleHttpExpect100Continue;
            ModuleResponseEncoding = source.ModuleResponseEncoding;
            SchemaVersion = source.SchemaVersion;
            
            
            

            CopyCustomGenericModuleStoreData(source);
        }

        
        /// <summary> The ModuleUsername property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleUsername"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ModuleUsername { get; }
        /// <summary> The ModulePassword property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModulePassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 80<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ModulePassword { get; }
        /// <summary> The ModuleUrl property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 350<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ModuleUrl { get; }
        /// <summary> The ModuleVersion property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ModuleVersion { get; }
        /// <summary> The ModulePlatform property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModulePlatform"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ModulePlatform { get; }
        /// <summary> The ModuleDeveloper property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleDeveloper"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ModuleDeveloper { get; }
        /// <summary> The ModuleOnlineStoreCode property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleOnlineStoreCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ModuleOnlineStoreCode { get; }
        /// <summary> The ModuleStatusCodes property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleStatusCodes"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ModuleStatusCodes { get; }
        /// <summary> The ModuleDownloadPageSize property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleDownloadPageSize"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ModuleDownloadPageSize { get; }
        /// <summary> The ModuleRequestTimeout property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleRequestTimeout"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ModuleRequestTimeout { get; }
        /// <summary> The ModuleDownloadStrategy property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleDownloadStrategy"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ModuleDownloadStrategy { get; }
        /// <summary> The ModuleOnlineStatusSupport property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleOnlineStatusSupport"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ModuleOnlineStatusSupport { get; }
        /// <summary> The ModuleOnlineStatusDataType property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleOnlineStatusDataType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ModuleOnlineStatusDataType { get; }
        /// <summary> The ModuleOnlineCustomerSupport property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleOnlineCustomerSupport"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean ModuleOnlineCustomerSupport { get; }
        /// <summary> The ModuleOnlineCustomerDataType property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleOnlineCustomerDataType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ModuleOnlineCustomerDataType { get; }
        /// <summary> The ModuleOnlineShipmentDetails property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleOnlineShipmentDetails"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean ModuleOnlineShipmentDetails { get; }
        /// <summary> The ModuleHttpExpect100Continue property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleHttpExpect100Continue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean ModuleHttpExpect100Continue { get; }
        /// <summary> The ModuleResponseEncoding property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleResponseEncoding"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ModuleResponseEncoding { get; }
        /// <summary> The SchemaVersion property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."SchemaVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SchemaVersion { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IGenericModuleStoreEntity AsReadOnlyGenericModuleStore() => this;

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IGenericModuleStoreEntity AsReadOnlyGenericModuleStore(IDictionary<object, object> objectMap) => this;
        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomGenericModuleStoreData(IGenericModuleStoreEntity source);
    }
}
