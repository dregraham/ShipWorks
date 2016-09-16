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
    /// Entity interface which represents the entity 'EndiciaScanForm'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IEndiciaScanFormEntity
    {
        
        /// <summary> The EndiciaScanFormID property of the Entity EndiciaScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaScanForm"."EndiciaScanFormID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 EndiciaScanFormID { get; }
        /// <summary> The EndiciaAccountID property of the Entity EndiciaScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaScanForm"."EndiciaAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 EndiciaAccountID { get; }
        /// <summary> The EndiciaAccountNumber property of the Entity EndiciaScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaScanForm"."EndiciaAccountNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String EndiciaAccountNumber { get; }
        /// <summary> The SubmissionID property of the Entity EndiciaScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaScanForm"."SubmissionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String SubmissionID { get; }
        /// <summary> The CreatedDate property of the Entity EndiciaScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaScanForm"."CreatedDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime CreatedDate { get; }
        /// <summary> The ScanFormBatchID property of the Entity EndiciaScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaScanForm"."ScanFormBatchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ScanFormBatchID { get; }
        /// <summary> The Description property of the Entity EndiciaScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaScanForm"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Description { get; }
        
        
        IScanFormBatchEntity ScanFormBatch { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IEndiciaScanFormEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IEndiciaScanFormEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'EndiciaScanForm'. <br/><br/>
    /// 
    /// </summary>
    public partial class EndiciaScanFormEntity : IEndiciaScanFormEntity
    {
        
        IScanFormBatchEntity IEndiciaScanFormEntity.ScanFormBatch => ScanFormBatch;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEndiciaScanFormEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IEndiciaScanFormEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IEndiciaScanFormEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyEndiciaScanFormEntity(this, objectMap);
        }
    }
}
