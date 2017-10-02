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
    /// Entity interface which represents the entity 'UspsScanForm'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IUspsScanFormEntity
    {
        
        /// <summary> The UspsScanFormID property of the Entity UspsScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsScanForm"."UspsScanFormID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 UspsScanFormID { get; }
        /// <summary> The UspsAccountID property of the Entity UspsScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsScanForm"."UspsAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 UspsAccountID { get; }
        /// <summary> The ScanFormTransactionID property of the Entity UspsScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsScanForm"."ScanFormTransactionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ScanFormTransactionID { get; }
        /// <summary> The ScanFormUrl property of the Entity UspsScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsScanForm"."ScanFormUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 2048<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ScanFormUrl { get; }
        /// <summary> The CreatedDate property of the Entity UspsScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsScanForm"."CreatedDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime CreatedDate { get; }
        /// <summary> The ScanFormBatchID property of the Entity UspsScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsScanForm"."ScanFormBatchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ScanFormBatchID { get; }
        /// <summary> The Description property of the Entity UspsScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsScanForm"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Description { get; }
        
        
        IScanFormBatchEntity ScanFormBatch { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUspsScanFormEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUspsScanFormEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'UspsScanForm'. <br/><br/>
    /// 
    /// </summary>
    public partial class UspsScanFormEntity : IUspsScanFormEntity
    {
        
        IScanFormBatchEntity IUspsScanFormEntity.ScanFormBatch => ScanFormBatch;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUspsScanFormEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IUspsScanFormEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IUspsScanFormEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyUspsScanFormEntity(this, objectMap);
        }

        
    }
}
