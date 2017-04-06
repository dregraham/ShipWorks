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
    /// Entity interface which represents the entity 'UpsRateTable'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IUpsRateTableEntity
    {
        
        /// <summary> The UpsRateTableID property of the Entity UpsRateTable<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsRateTable"."UpsRateTableID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 UpsRateTableID { get; }
        /// <summary> The UpsAccountID property of the Entity UpsRateTable<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsRateTable"."UpsAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 UpsAccountID { get; }
        /// <summary> The UploadDate property of the Entity UpsRateTable<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsRateTable"."UploadDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime UploadDate { get; }
        
        
        IUpsAccountEntity UpsAccount { get; }
        
        IEnumerable<IUpsLocalRateEntity> UpsLocalRate { get; }
        IEnumerable<IUpsLocalRateSurchargeEntity> UpsLocalRateSurcharge { get; }

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUpsRateTableEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUpsRateTableEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'UpsRateTable'. <br/><br/>
    /// 
    /// </summary>
    public partial class UpsRateTableEntity : IUpsRateTableEntity
    {
        
        IUpsAccountEntity IUpsRateTableEntity.UpsAccount => UpsAccount;
        
        IEnumerable<IUpsLocalRateEntity> IUpsRateTableEntity.UpsLocalRate => UpsLocalRate;
        IEnumerable<IUpsLocalRateSurchargeEntity> IUpsRateTableEntity.UpsLocalRateSurcharge => UpsLocalRateSurcharge;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsRateTableEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IUpsRateTableEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IUpsRateTableEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyUpsRateTableEntity(this, objectMap);
        }
    }
}
