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
    /// Entity interface which represents the entity 'DhlEcommerceScanForm'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IDhlEcommerceScanFormEntity
    {
        
        /// <summary> The DhlEcommerceScanFormID property of the Entity DhlEcommerceScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceScanForm"."DhlEcommerceScanFormID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 DhlEcommerceScanFormID { get; }
        /// <summary> The DhlEcommerceAccountID property of the Entity DhlEcommerceScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceScanForm"."DhlEcommerceAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 DhlEcommerceAccountID { get; }
        /// <summary> The ScanFormTransactionID property of the Entity DhlEcommerceScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceScanForm"."ScanFormTransactionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ScanFormTransactionID { get; }
        /// <summary> The ScanFormUrl property of the Entity DhlEcommerceScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceScanForm"."ScanFormUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 2048<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ScanFormUrl { get; }
        /// <summary> The CreatedDate property of the Entity DhlEcommerceScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceScanForm"."CreatedDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime CreatedDate { get; }
        /// <summary> The ScanFormBatchID property of the Entity DhlEcommerceScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceScanForm"."ScanFormBatchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ScanFormBatchID { get; }
        /// <summary> The Description property of the Entity DhlEcommerceScanForm<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlEcommerceScanForm"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Description { get; }
        
        
        IScanFormBatchEntity ScanFormBatch { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IDhlEcommerceScanFormEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IDhlEcommerceScanFormEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'DhlEcommerceScanForm'. <br/><br/>
    /// 
    /// </summary>
    public partial class DhlEcommerceScanFormEntity : IDhlEcommerceScanFormEntity
    {
        
        IScanFormBatchEntity IDhlEcommerceScanFormEntity.ScanFormBatch => ScanFormBatch;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDhlEcommerceScanFormEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IDhlEcommerceScanFormEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IDhlEcommerceScanFormEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyDhlEcommerceScanFormEntity(this, objectMap);
        }

        
    }
}
