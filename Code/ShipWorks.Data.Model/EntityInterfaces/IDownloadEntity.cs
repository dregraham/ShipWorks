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
    /// Entity interface which represents the entity 'Download'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IDownloadEntity
    {
        
        /// <summary> The DownloadID property of the Entity Download<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Download"."DownloadID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 DownloadID { get; }
        /// <summary> The RowVersion property of the Entity Download<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Download"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The StoreID property of the Entity Download<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Download"."StoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 StoreID { get; }
        /// <summary> The ComputerID property of the Entity Download<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Download"."ComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ComputerID { get; }
        /// <summary> The UserID property of the Entity Download<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Download"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 UserID { get; }
        /// <summary> The InitiatedBy property of the Entity Download<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Download"."InitiatedBy"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 InitiatedBy { get; }
        /// <summary> The Started property of the Entity Download<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Download"."Started"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime Started { get; }
        /// <summary> The Ended property of the Entity Download<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Download"."Ended"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.DateTime> Ended { get; }
        /// <summary> The Duration property of the Entity Download<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Download"."Duration"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> Duration { get; }
        /// <summary> The QuantityTotal property of the Entity Download<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Download"."QuantityTotal"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> QuantityTotal { get; }
        /// <summary> The QuantityNew property of the Entity Download<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Download"."QuantityNew"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> QuantityNew { get; }
        /// <summary> The Result property of the Entity Download<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Download"."Result"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Result { get; }
        /// <summary> The ErrorMessage property of the Entity Download<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Download"."ErrorMessage"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ErrorMessage { get; }
        
        
        IComputerEntity Computer { get; }
        IStoreEntity Store { get; }
        IUserEntity User { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IDownloadEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IDownloadEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'Download'. <br/><br/>
    /// 
    /// </summary>
    public partial class DownloadEntity : IDownloadEntity
    {
        
        IComputerEntity IDownloadEntity.Computer => Computer;
        IStoreEntity IDownloadEntity.Store => Store;
        IUserEntity IDownloadEntity.User => User;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDownloadEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IDownloadEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IDownloadEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyDownloadEntity(this, objectMap);
        }
    }
}
