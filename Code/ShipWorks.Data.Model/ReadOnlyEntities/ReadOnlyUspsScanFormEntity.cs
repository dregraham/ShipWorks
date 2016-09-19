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
    /// Read-only representation of the entity 'UspsScanForm'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyUspsScanFormEntity : IUspsScanFormEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyUspsScanFormEntity(IUspsScanFormEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            UspsScanFormID = source.UspsScanFormID;
            UspsAccountID = source.UspsAccountID;
            ScanFormTransactionID = source.ScanFormTransactionID;
            ScanFormUrl = source.ScanFormUrl;
            CreatedDate = source.CreatedDate;
            ScanFormBatchID = source.ScanFormBatchID;
            Description = source.Description;
            
            
            ScanFormBatch = source.ScanFormBatch?.AsReadOnly(objectMap);
            

            CopyCustomUspsScanFormData(source);
        }

        
        /// <summary> The UspsScanFormID property of the Entity UspsScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsScanForm"."UspsScanFormID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 UspsScanFormID { get; }
        /// <summary> The UspsAccountID property of the Entity UspsScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsScanForm"."UspsAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 UspsAccountID { get; }
        /// <summary> The ScanFormTransactionID property of the Entity UspsScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsScanForm"."ScanFormTransactionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ScanFormTransactionID { get; }
        /// <summary> The ScanFormUrl property of the Entity UspsScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsScanForm"."ScanFormUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 2048<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ScanFormUrl { get; }
        /// <summary> The CreatedDate property of the Entity UspsScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsScanForm"."CreatedDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime CreatedDate { get; }
        /// <summary> The ScanFormBatchID property of the Entity UspsScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsScanForm"."ScanFormBatchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ScanFormBatchID { get; }
        /// <summary> The Description property of the Entity UspsScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsScanForm"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Description { get; }
        
        
        public IScanFormBatchEntity ScanFormBatch { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUspsScanFormEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUspsScanFormEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomUspsScanFormData(IUspsScanFormEntity source);
    }
}
