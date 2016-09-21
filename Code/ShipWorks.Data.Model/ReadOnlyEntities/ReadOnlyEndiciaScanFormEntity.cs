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
    /// Read-only representation of the entity 'EndiciaScanForm'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyEndiciaScanFormEntity : IEndiciaScanFormEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyEndiciaScanFormEntity(IEndiciaScanFormEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            EndiciaScanFormID = source.EndiciaScanFormID;
            EndiciaAccountID = source.EndiciaAccountID;
            EndiciaAccountNumber = source.EndiciaAccountNumber;
            SubmissionID = source.SubmissionID;
            CreatedDate = source.CreatedDate;
            ScanFormBatchID = source.ScanFormBatchID;
            Description = source.Description;
            
            
            ScanFormBatch = source.ScanFormBatch?.AsReadOnly(objectMap);
            

            CopyCustomEndiciaScanFormData(source);
        }

        
        /// <summary> The EndiciaScanFormID property of the Entity EndiciaScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaScanForm"."EndiciaScanFormID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 EndiciaScanFormID { get; }
        /// <summary> The EndiciaAccountID property of the Entity EndiciaScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaScanForm"."EndiciaAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 EndiciaAccountID { get; }
        /// <summary> The EndiciaAccountNumber property of the Entity EndiciaScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaScanForm"."EndiciaAccountNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String EndiciaAccountNumber { get; }
        /// <summary> The SubmissionID property of the Entity EndiciaScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaScanForm"."SubmissionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SubmissionID { get; }
        /// <summary> The CreatedDate property of the Entity EndiciaScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaScanForm"."CreatedDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime CreatedDate { get; }
        /// <summary> The ScanFormBatchID property of the Entity EndiciaScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaScanForm"."ScanFormBatchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ScanFormBatchID { get; }
        /// <summary> The Description property of the Entity EndiciaScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaScanForm"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Description { get; }
        
        
        public IScanFormBatchEntity ScanFormBatch { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEndiciaScanFormEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEndiciaScanFormEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomEndiciaScanFormData(IEndiciaScanFormEntity source);
    }
}
