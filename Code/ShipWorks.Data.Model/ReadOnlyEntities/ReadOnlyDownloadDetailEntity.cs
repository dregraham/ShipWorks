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
    /// Read-only representation of the entity 'DownloadDetail'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyDownloadDetailEntity : IDownloadDetailEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyDownloadDetailEntity(IDownloadDetailEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            DownloadedDetailID = source.DownloadedDetailID;
            DownloadID = source.DownloadID;
            OrderID = source.OrderID;
            InitialDownload = source.InitialDownload;
            OrderNumber = source.OrderNumber;
            ExtraBigIntData1 = source.ExtraBigIntData1;
            ExtraBigIntData2 = source.ExtraBigIntData2;
            ExtraBigIntData3 = source.ExtraBigIntData3;
            ExtraStringData1 = source.ExtraStringData1;
            
            
            DownloadLog = source.DownloadLog?.AsReadOnly(objectMap);
            

            CopyCustomDownloadDetailData(source);
        }

        
        /// <summary> The DownloadedDetailID property of the Entity DownloadDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DownloadDetail"."DownloadedDetailID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 DownloadedDetailID { get; }
        /// <summary> The DownloadID property of the Entity DownloadDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DownloadDetail"."DownloadID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 DownloadID { get; }
        /// <summary> The OrderID property of the Entity DownloadDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DownloadDetail"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The InitialDownload property of the Entity DownloadDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DownloadDetail"."InitialDownload"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean InitialDownload { get; }
        /// <summary> The OrderNumber property of the Entity DownloadDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DownloadDetail"."OrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> OrderNumber { get; }
        /// <summary> The ExtraBigIntData1 property of the Entity DownloadDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DownloadDetail"."ExtraBigIntData1"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> ExtraBigIntData1 { get; }
        /// <summary> The ExtraBigIntData2 property of the Entity DownloadDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DownloadDetail"."ExtraBigIntData2"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> ExtraBigIntData2 { get; }
        /// <summary> The ExtraBigIntData3 property of the Entity DownloadDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DownloadDetail"."ExtraBigIntData3"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> ExtraBigIntData3 { get; }
        /// <summary> The ExtraStringData1 property of the Entity DownloadDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DownloadDetail"."ExtraStringData1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ExtraStringData1 { get; }
        
        
        public IDownloadEntity DownloadLog { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDownloadDetailEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDownloadDetailEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomDownloadDetailData(IDownloadDetailEntity source);
    }
}
