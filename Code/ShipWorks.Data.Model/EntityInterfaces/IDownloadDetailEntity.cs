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
    /// Entity interface which represents the entity 'DownloadDetail'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IDownloadDetailEntity
    {
        
        /// <summary> The DownloadedDetailID property of the Entity DownloadDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DownloadDetail"."DownloadedDetailID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 DownloadedDetailID { get; }
        /// <summary> The DownloadID property of the Entity DownloadDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DownloadDetail"."DownloadID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 DownloadID { get; }
        /// <summary> The OrderID property of the Entity DownloadDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DownloadDetail"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The InitialDownload property of the Entity DownloadDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DownloadDetail"."InitialDownload"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean InitialDownload { get; }
        /// <summary> The OrderNumber property of the Entity DownloadDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DownloadDetail"."OrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> OrderNumber { get; }
        /// <summary> The ExtraBigIntData1 property of the Entity DownloadDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DownloadDetail"."ExtraBigIntData1"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> ExtraBigIntData1 { get; }
        /// <summary> The ExtraBigIntData2 property of the Entity DownloadDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DownloadDetail"."ExtraBigIntData2"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> ExtraBigIntData2 { get; }
        /// <summary> The ExtraBigIntData3 property of the Entity DownloadDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DownloadDetail"."ExtraBigIntData3"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> ExtraBigIntData3 { get; }
        /// <summary> The ExtraStringData1 property of the Entity DownloadDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DownloadDetail"."ExtraStringData1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ExtraStringData1 { get; }
        
        
        IDownloadEntity DownloadLog { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IDownloadDetailEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IDownloadDetailEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'DownloadDetail'. <br/><br/>
    /// 
    /// </summary>
    public partial class DownloadDetailEntity : IDownloadDetailEntity
    {
        
        IDownloadEntity IDownloadDetailEntity.DownloadLog => DownloadLog;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDownloadDetailEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IDownloadDetailEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IDownloadDetailEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyDownloadDetailEntity(this, objectMap);
        }
    }
}
