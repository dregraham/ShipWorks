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
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Read-only representation of the entity 'Configuration'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyConfigurationEntity : IConfigurationEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyConfigurationEntity(IConfigurationEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ConfigurationID = source.ConfigurationID;
            RowVersion = source.RowVersion;
            LogOnMethod = source.LogOnMethod;
            AddressCasing = source.AddressCasing;
            CustomerCompareEmail = source.CustomerCompareEmail;
            CustomerCompareAddress = source.CustomerCompareAddress;
            CustomerUpdateBilling = source.CustomerUpdateBilling;
            CustomerUpdateShipping = source.CustomerUpdateShipping;
            CustomerUpdateModifiedBilling = source.CustomerUpdateModifiedBilling;
            CustomerUpdateModifiedShipping = source.CustomerUpdateModifiedShipping;
            AuditNewOrders = source.AuditNewOrders;
            AuditDeletedOrders = source.AuditDeletedOrders;
            CustomerKey = source.CustomerKey;
            UseParallelActionQueue = source.UseParallelActionQueue;
            AllowEbayCombineLocally = source.AllowEbayCombineLocally;
            ArchivalSettingsXml = source.ArchivalSettingsXml;
            AuditEnabled = source.AuditEnabled;
            DefaultPickListTemplateID = source.DefaultPickListTemplateID;
            AutoUpdateDayOfWeek = source.AutoUpdateDayOfWeek;
            AutoUpdateHourOfDay = source.AutoUpdateHourOfDay;
            AutoUpdateStartDate = source.AutoUpdateStartDate;
            WarehouseID = source.WarehouseID;
            WarehouseName = source.WarehouseName;
            LegacyCustomerKey = source.LegacyCustomerKey;
            
            
            

            CopyCustomConfigurationData(source);
        }

        
        /// <summary> The ConfigurationID property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."ConfigurationID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Boolean ConfigurationID { get; }
        /// <summary> The RowVersion property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The LogOnMethod property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."LogOnMethod"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 LogOnMethod { get; }
        /// <summary> The AddressCasing property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."AddressCasing"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean AddressCasing { get; }
        /// <summary> The CustomerCompareEmail property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."CustomerCompareEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean CustomerCompareEmail { get; }
        /// <summary> The CustomerCompareAddress property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."CustomerCompareAddress"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean CustomerCompareAddress { get; }
        /// <summary> The CustomerUpdateBilling property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."CustomerUpdateBilling"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean CustomerUpdateBilling { get; }
        /// <summary> The CustomerUpdateShipping property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."CustomerUpdateShipping"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean CustomerUpdateShipping { get; }
        /// <summary> The CustomerUpdateModifiedBilling property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."CustomerUpdateModifiedBilling"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 CustomerUpdateModifiedBilling { get; }
        /// <summary> The CustomerUpdateModifiedShipping property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."CustomerUpdateModifiedShipping"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 CustomerUpdateModifiedShipping { get; }
        /// <summary> The AuditNewOrders property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."AuditNewOrders"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean AuditNewOrders { get; }
        /// <summary> The AuditDeletedOrders property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."AuditDeletedOrders"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean AuditDeletedOrders { get; }
        /// <summary> The CustomerKey property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."CustomerKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CustomerKey { get; }
        /// <summary> The UseParallelActionQueue property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."UseParallelActionQueue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean UseParallelActionQueue { get; }
        /// <summary> The AllowEbayCombineLocally property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."AllowEbayCombineLocally"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean AllowEbayCombineLocally { get; }
        /// <summary> The ArchivalSettingsXml property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."ArchivalSettingsXml"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ArchivalSettingsXml { get; }
        /// <summary> The AuditEnabled property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."AuditEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean AuditEnabled { get; }
        /// <summary> The DefaultPickListTemplateID property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."DefaultPickListTemplateID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> DefaultPickListTemplateID { get; }
        /// <summary> The AutoUpdateDayOfWeek property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."AutoUpdateDayOfWeek"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DayOfWeek AutoUpdateDayOfWeek { get; }
        /// <summary> The AutoUpdateHourOfDay property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."AutoUpdateHourOfDay"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 AutoUpdateHourOfDay { get; }
        /// <summary> The AutoUpdateStartDate property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."AutoUpdateStartDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime2, 7, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime AutoUpdateStartDate { get; }
        /// <summary> The WarehouseID property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."WarehouseID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String WarehouseID { get; }
        /// <summary> The WarehouseName property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."WarehouseName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String WarehouseName { get; }
        /// <summary> The LegacyCustomerKey property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."LegacyCustomerKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String LegacyCustomerKey { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IConfigurationEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IConfigurationEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomConfigurationData(IConfigurationEntity source);
    }
}
