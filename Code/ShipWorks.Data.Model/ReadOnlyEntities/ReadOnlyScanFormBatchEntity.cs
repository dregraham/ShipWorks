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
    /// Read-only representation of the entity 'ScanFormBatch'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyScanFormBatchEntity : IScanFormBatchEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyScanFormBatchEntity(IScanFormBatchEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ScanFormBatchID = source.ScanFormBatchID;
            ShipmentType = source.ShipmentType;
            CreatedDate = source.CreatedDate;
            ShipmentCount = source.ShipmentCount;
            
            
            
            EndiciaScanForms = source.EndiciaScanForms?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IEndiciaScanFormEntity>();
            EndiciaShipment = source.EndiciaShipment?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IEndiciaShipmentEntity>();
            UspsScanForms = source.UspsScanForms?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IUspsScanFormEntity>();
            UspsShipment = source.UspsShipment?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IUspsShipmentEntity>();

            CopyCustomScanFormBatchData(source);
        }

        
        /// <summary> The ScanFormBatchID property of the Entity ScanFormBatch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ScanFormBatch"."ScanFormBatchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ScanFormBatchID { get; }
        /// <summary> The ShipmentType property of the Entity ScanFormBatch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ScanFormBatch"."ShipmentType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipmentType { get; }
        /// <summary> The CreatedDate property of the Entity ScanFormBatch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ScanFormBatch"."CreatedDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime CreatedDate { get; }
        /// <summary> The ShipmentCount property of the Entity ScanFormBatch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ScanFormBatch"."ShipmentCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipmentCount { get; }
        
        
        
        public IEnumerable<IEndiciaScanFormEntity> EndiciaScanForms { get; }
        
        public IEnumerable<IEndiciaShipmentEntity> EndiciaShipment { get; }
        
        public IEnumerable<IUspsScanFormEntity> UspsScanForms { get; }
        
        public IEnumerable<IUspsShipmentEntity> UspsShipment { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IScanFormBatchEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IScanFormBatchEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomScanFormBatchData(IScanFormBatchEntity source);
    }
}
