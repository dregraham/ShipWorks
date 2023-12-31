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
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'ScanFormBatch'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IScanFormBatchEntity
    {
        
        /// <summary> The ScanFormBatchID property of the Entity ScanFormBatch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ScanFormBatch"."ScanFormBatchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ScanFormBatchID { get; }
        /// <summary> The ShipmentType property of the Entity ScanFormBatch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ScanFormBatch"."ShipmentType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipmentType { get; }
        /// <summary> The CreatedDate property of the Entity ScanFormBatch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ScanFormBatch"."CreatedDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime CreatedDate { get; }
        /// <summary> The ShipmentCount property of the Entity ScanFormBatch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ScanFormBatch"."ShipmentCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipmentCount { get; }
        
        
        
        IEnumerable<IEndiciaScanFormEntity> EndiciaScanForms { get; }
        IEnumerable<IEndiciaShipmentEntity> EndiciaShipment { get; }
        IEnumerable<IUspsScanFormEntity> UspsScanForms { get; }
        IEnumerable<IUspsShipmentEntity> UspsShipment { get; }

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IScanFormBatchEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IScanFormBatchEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ScanFormBatch'. <br/><br/>
    /// 
    /// </summary>
    public partial class ScanFormBatchEntity : IScanFormBatchEntity
    {
        
        
        IEnumerable<IEndiciaScanFormEntity> IScanFormBatchEntity.EndiciaScanForms => EndiciaScanForms;
        IEnumerable<IEndiciaShipmentEntity> IScanFormBatchEntity.EndiciaShipment => EndiciaShipment;
        IEnumerable<IUspsScanFormEntity> IScanFormBatchEntity.UspsScanForms => UspsScanForms;
        IEnumerable<IUspsShipmentEntity> IScanFormBatchEntity.UspsShipment => UspsShipment;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IScanFormBatchEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IScanFormBatchEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IScanFormBatchEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyScanFormBatchEntity(this, objectMap);
        }

        
    }
}
