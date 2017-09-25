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
    /// Entity interface which represents the entity 'Store'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IStoreEntity
    {
        
        /// <summary> The StoreID property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."StoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 StoreID { get; }
        /// <summary> The RowVersion property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The License property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."License"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String License { get; }
        /// <summary> The Edition property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."Edition"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Edition { get; }
        /// <summary> The TypeCode property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."TypeCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 TypeCode { get; }
        /// <summary> The Enabled property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."Enabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean Enabled { get; }
        /// <summary> The SetupComplete property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."SetupComplete"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean SetupComplete { get; }
        /// <summary> The StoreName property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."StoreName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 75<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String StoreName { get; }
        /// <summary> The Company property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."Company"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Company { get; }
        /// <summary> The Street1 property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."Street1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Street1 { get; }
        /// <summary> The Street2 property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."Street2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Street2 { get; }
        /// <summary> The Street3 property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."Street3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Street3 { get; }
        /// <summary> The City property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."City"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String City { get; }
        /// <summary> The StateProvCode property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."StateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String StateProvCode { get; }
        /// <summary> The PostalCode property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."PostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PostalCode { get; }
        /// <summary> The CountryCode property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."CountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String CountryCode { get; }
        /// <summary> The Phone property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."Phone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Phone { get; }
        /// <summary> The Fax property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."Fax"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Fax { get; }
        /// <summary> The Email property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Email { get; }
        /// <summary> The Website property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."Website"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Website { get; }
        /// <summary> The AutoDownload property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."AutoDownload"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean AutoDownload { get; }
        /// <summary> The AutoDownloadMinutes property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."AutoDownloadMinutes"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 AutoDownloadMinutes { get; }
        /// <summary> The AutoDownloadOnlyAway property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."AutoDownloadOnlyAway"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean AutoDownloadOnlyAway { get; }
        /// <summary> The AddressValidationSetting property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."AddressValidationSetting"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 AddressValidationSetting { get; }
        /// <summary> The ComputerDownloadPolicy property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."ComputerDownloadPolicy"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ComputerDownloadPolicy { get; }
        /// <summary> The DefaultEmailAccountID property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."DefaultEmailAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 DefaultEmailAccountID { get; }
        /// <summary> The ManualOrderPrefix property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."ManualOrderPrefix"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ManualOrderPrefix { get; }
        /// <summary> The ManualOrderPostfix property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."ManualOrderPostfix"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ManualOrderPostfix { get; }
        /// <summary> The InitialDownloadDays property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."InitialDownloadDays"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> InitialDownloadDays { get; }
        /// <summary> The InitialDownloadOrder property of the Entity Store<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Store"."InitialDownloadOrder"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> InitialDownloadOrder { get; }
        
        
        
        IEnumerable<IOrderSearchEntity> OrderSearch { get; }

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IStoreEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IStoreEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'Store'. <br/><br/>
    /// 
    /// </summary>
    public partial class StoreEntity : IStoreEntity
    {
        
        
        IEnumerable<IOrderSearchEntity> IStoreEntity.OrderSearch => OrderSearch;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IStoreEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IStoreEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyStoreEntity(this, objectMap);
        }
    }
}
