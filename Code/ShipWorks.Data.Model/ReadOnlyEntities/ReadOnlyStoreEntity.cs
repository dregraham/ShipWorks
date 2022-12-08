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
    /// Read-only representation of the entity 'Store'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyStoreEntity : IStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyStoreEntity(IStoreEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            StoreID = source.StoreID;
            RowVersion = source.RowVersion;
            License = source.License;
            Edition = source.Edition;
            TypeCode = source.TypeCode;
            Enabled = source.Enabled;
            SetupComplete = source.SetupComplete;
            StoreName = source.StoreName;
            Company = source.Company;
            Street1 = source.Street1;
            Street2 = source.Street2;
            Street3 = source.Street3;
            City = source.City;
            StateProvCode = source.StateProvCode;
            PostalCode = source.PostalCode;
            CountryCode = source.CountryCode;
            Phone = source.Phone;
            Fax = source.Fax;
            Email = source.Email;
            Website = source.Website;
            AutoDownload = source.AutoDownload;
            AutoDownloadMinutes = source.AutoDownloadMinutes;
            AutoDownloadOnlyAway = source.AutoDownloadOnlyAway;
            DomesticAddressValidationSetting = source.DomesticAddressValidationSetting;
            InternationalAddressValidationSetting = source.InternationalAddressValidationSetting;
            ComputerDownloadPolicy = source.ComputerDownloadPolicy;
            DefaultEmailAccountID = source.DefaultEmailAccountID;
            ManualOrderPrefix = source.ManualOrderPrefix;
            ManualOrderPostfix = source.ManualOrderPostfix;
            InitialDownloadDays = source.InitialDownloadDays;
            InitialDownloadOrder = source.InitialDownloadOrder;
            InsureShipClientID = source.InsureShipClientID;
            InsureShipApiKey = source.InsureShipApiKey;
            WarehouseStoreID = source.WarehouseStoreID;
            ManagedInHub = source.ManagedInHub;
            OrderSourceID = source.OrderSourceID;
            PlatformAmazonCarrierID = source.PlatformAmazonCarrierID;
            ContinuationToken = source.ContinuationToken;
            
            
            
            OrderSearch = source.OrderSearch?.Select(x => x.AsReadOnly(objectMap)).OfType<IOrderSearchEntity>().ToReadOnly() ??
                Enumerable.Empty<IOrderSearchEntity>();

            CopyCustomStoreData(source);
        }

        
        /// <summary> The StoreID property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."StoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 StoreID { get; }
        /// <summary> The RowVersion property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The License property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."License"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String License { get; }
        /// <summary> The Edition property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."Edition"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Edition { get; }
        /// <summary> The TypeCode property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."TypeCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 TypeCode { get; }
        /// <summary> The Enabled property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."Enabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean Enabled { get; }
        /// <summary> The SetupComplete property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."SetupComplete"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean SetupComplete { get; }
        /// <summary> The StoreName property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."StoreName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 75<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String StoreName { get; }
        /// <summary> The Company property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."Company"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Company { get; }
        /// <summary> The Street1 property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."Street1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Street1 { get; }
        /// <summary> The Street2 property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."Street2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Street2 { get; }
        /// <summary> The Street3 property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."Street3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Street3 { get; }
        /// <summary> The City property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."City"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String City { get; }
        /// <summary> The StateProvCode property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."StateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String StateProvCode { get; }
        /// <summary> The PostalCode property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."PostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PostalCode { get; }
        /// <summary> The CountryCode property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."CountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CountryCode { get; }
        /// <summary> The Phone property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."Phone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Phone { get; }
        /// <summary> The Fax property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."Fax"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Fax { get; }
        /// <summary> The Email property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Email { get; }
        /// <summary> The Website property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."Website"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Website { get; }
        /// <summary> The AutoDownload property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."AutoDownload"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean AutoDownload { get; }
        /// <summary> The AutoDownloadMinutes property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."AutoDownloadMinutes"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 AutoDownloadMinutes { get; }
        /// <summary> The AutoDownloadOnlyAway property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."AutoDownloadOnlyAway"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean AutoDownloadOnlyAway { get; }
        /// <summary> The DomesticAddressValidationSetting property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."DomesticAddressValidationSetting"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public Interapptive.Shared.Enums.AddressValidationStoreSettingType DomesticAddressValidationSetting { get; }
        /// <summary> The InternationalAddressValidationSetting property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."InternationalAddressValidationSetting"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public Interapptive.Shared.Enums.AddressValidationStoreSettingType InternationalAddressValidationSetting { get; }
        /// <summary> The ComputerDownloadPolicy property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."ComputerDownloadPolicy"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ComputerDownloadPolicy { get; }
        /// <summary> The DefaultEmailAccountID property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."DefaultEmailAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 DefaultEmailAccountID { get; }
        /// <summary> The ManualOrderPrefix property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."ManualOrderPrefix"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ManualOrderPrefix { get; }
        /// <summary> The ManualOrderPostfix property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."ManualOrderPostfix"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ManualOrderPostfix { get; }
        /// <summary> The InitialDownloadDays property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."InitialDownloadDays"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> InitialDownloadDays { get; }
        /// <summary> The InitialDownloadOrder property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."InitialDownloadOrder"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> InitialDownloadOrder { get; }
        /// <summary> The InsureShipClientID property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."InsureShipClientID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> InsureShipClientID { get; }
        /// <summary> The InsureShipApiKey property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."InsureShipApiKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String InsureShipApiKey { get; }
        /// <summary> The WarehouseStoreID property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."WarehouseStoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Guid> WarehouseStoreID { get; }
        /// <summary> The ManagedInHub property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."ManagedInHub"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean ManagedInHub { get; }
        /// <summary> The OrderSourceID property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."OrderSourceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String OrderSourceID { get; }
        /// <summary> The PlatformAmazonCarrierID property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."PlatformAmazonCarrierID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String PlatformAmazonCarrierID { get; }
        /// <summary> The ContinuationToken property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."ContinuationToken"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2048<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ContinuationToken { get; }
        
        
        
        public IEnumerable<IOrderSearchEntity> OrderSearch { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomStoreData(IStoreEntity source);
    }
}
