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
    /// Read-only representation of the entity 'UpsRateTable'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyUpsRateTableEntity : IUpsRateTableEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyUpsRateTableEntity(IUpsRateTableEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            UpsRateTableID = source.UpsRateTableID;
            UploadDate = source.UploadDate;
            
            
            
            UpsAccount = source.UpsAccount?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IUpsAccountEntity>();
            UpsLetterRate = source.UpsLetterRate?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IUpsLetterRateEntity>();
            UpsPackageRate = source.UpsPackageRate?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IUpsPackageRateEntity>();
            UpsPricePerPound = source.UpsPricePerPound?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IUpsPricePerPoundEntity>();
            UpsRateSurcharge = source.UpsRateSurcharge?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IUpsRateSurchargeEntity>();

            CopyCustomUpsRateTableData(source);
        }

        
        /// <summary> The UpsRateTableID property of the Entity UpsRateTable<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsRateTable"."UpsRateTableID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 UpsRateTableID { get; }
        /// <summary> The UploadDate property of the Entity UpsRateTable<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsRateTable"."UploadDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime2, 7, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime UploadDate { get; }
        
        
        
        public IEnumerable<IUpsAccountEntity> UpsAccount { get; }
        
        public IEnumerable<IUpsLetterRateEntity> UpsLetterRate { get; }
        
        public IEnumerable<IUpsPackageRateEntity> UpsPackageRate { get; }
        
        public IEnumerable<IUpsPricePerPoundEntity> UpsPricePerPound { get; }
        
        public IEnumerable<IUpsRateSurchargeEntity> UpsRateSurcharge { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsRateTableEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsRateTableEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomUpsRateTableData(IUpsRateTableEntity source);
    }
}
