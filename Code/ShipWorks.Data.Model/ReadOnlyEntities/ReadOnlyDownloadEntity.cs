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
    /// Read-only representation of the entity 'Download'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyDownloadEntity : IDownloadEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyDownloadEntity(IDownloadEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            DownloadID = source.DownloadID;
            RowVersion = source.RowVersion;
            StoreID = source.StoreID;
            ComputerID = source.ComputerID;
            UserID = source.UserID;
            InitiatedBy = source.InitiatedBy;
            Started = source.Started;
            Ended = source.Ended;
            Duration = source.Duration;
            QuantityTotal = source.QuantityTotal;
            QuantityNew = source.QuantityNew;
            Result = source.Result;
            ErrorMessage = source.ErrorMessage;
            
            
            Computer = (IComputerEntity) source.Computer?.AsReadOnly(objectMap);
            Store = (IStoreEntity) source.Store?.AsReadOnly(objectMap);
            User = (IUserEntity) source.User?.AsReadOnly(objectMap);
            

            CopyCustomDownloadData(source);
        }

        
        /// <summary> The DownloadID property of the Entity Download<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Download"."DownloadID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 DownloadID { get; }
        /// <summary> The RowVersion property of the Entity Download<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Download"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The StoreID property of the Entity Download<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Download"."StoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 StoreID { get; }
        /// <summary> The ComputerID property of the Entity Download<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Download"."ComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ComputerID { get; }
        /// <summary> The UserID property of the Entity Download<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Download"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 UserID { get; }
        /// <summary> The InitiatedBy property of the Entity Download<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Download"."InitiatedBy"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 InitiatedBy { get; }
        /// <summary> The Started property of the Entity Download<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Download"."Started"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime Started { get; }
        /// <summary> The Ended property of the Entity Download<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Download"."Ended"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.DateTime> Ended { get; }
        /// <summary> The Duration property of the Entity Download<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Download"."Duration"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> Duration { get; }
        /// <summary> The QuantityTotal property of the Entity Download<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Download"."QuantityTotal"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> QuantityTotal { get; }
        /// <summary> The QuantityNew property of the Entity Download<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Download"."QuantityNew"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> QuantityNew { get; }
        /// <summary> The Result property of the Entity Download<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Download"."Result"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Result { get; }
        /// <summary> The ErrorMessage property of the Entity Download<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Download"."ErrorMessage"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ErrorMessage { get; }
        
        
        public IComputerEntity Computer { get; }
        
        public IStoreEntity Store { get; }
        
        public IUserEntity User { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDownloadEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDownloadEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomDownloadData(IDownloadEntity source);
    }
}
